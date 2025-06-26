// File: Services/OrdenPagoService.cs
using BackendApp.Models;
using BackendApp.Services.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendApp.Services
{
    // DTO para transferencia al front-end
    public class OrdenPagoDto
    {
        public long IdOrden { get; set; }

        // Antes era FechaPedido del pedido; ahora será la fecha de la orden
        public DateOnly? FechaPedido { get; set; }

        // Ahora suma todos los montos de los pedidos relacionados a esta orden
        public decimal MontoTotal { get; set; }

        public string Estado { get; set; }
    }

    // DTO para cada línea de “presupuesto” enviada desde el frontend


    public class PedidoDetalleSimpleDto
    {
        public long IdPedidoDetalle { get; set; }
        public string Producto { get; set; } = "";
        public decimal Cotizacion { get; set; }
        public int Cantidad { get; set; }
        public decimal Iva { get; set; }
        public decimal Subtotal { get; set; }
    }

    public class PedidoConDetallesDto
    {
        public long IdPedido { get; set; }
        public string Proveedor { get; set; } = "";
        public DateOnly FechaPedido { get; set; }
        public DateOnly? FechaEntrega { get; set; }

        public string? CorreoProveedor { get; set; }
        public List<PedidoDetalleSimpleDto> Detalles { get; set; } = new();
        public decimal MontoTotal { get; set; }
    }

    // DTO que recibe la lista de líneas (agrupadas luego por proveedor)
    public class PedidoDetalleCreacionDto
    {
        public long IdProducto { get; set; }
        public long IdProveedor { get; set; }
        public decimal Cotizacion { get; set; }
        public int Cantidad { get; set; }
        public decimal Iva { get; set; }
        public DateOnly FechaEntrega { get; set; }
    }

    // DTO que agrupa la orden con todas las líneas seleccionadas
    public class CrearPedidosDto
    {
        public long OrdenId { get; set; }
        public List<PedidoDetalleCreacionDto> Detalles { get; set; } = new();
    }

    public class OrdenPagoService
    {
        private readonly PostgresContext _context;

        public OrdenPagoService(PostgresContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Trae todas las órdenes con:
        /// - Fecha de la orden (campo “Fecha” recién agregado en la entidad Orden).
        /// - El monto total: suma de todos los pedidos que pertenecen a esa orden.
        /// - El estado de la orden.
        /// </summary>
        public async Task<List<OrdenPagoDto>> GetOrdenesPagoAsync()
        {
            var query = _context.Ordenes
                .AsNoTracking()
                .Select(o => new OrdenPagoDto
                {
                    IdOrden = o.IdOrden,

                    // Ahora mapeamos FechaPedido ← o.Fecha (la nueva columna de Orden)
                    FechaPedido = o.Fecha,

                    // Suma de todos los pedidos que referencian esta orden
                    MontoTotal = o.Pedidos.Sum(p => p.MontoTotal ?? 0m),

                    Estado = o.Estado
                });

            return await query.ToListAsync();
        }

        /// <summary>
        /// Devuelve todos los PedidoDetalle de todos los Pedidos que pertenecen a una determinada orden.
        /// Antes solo se buscaba el primer pedido; ahora una orden puede tener varios pedidos.
        /// </summary>
        public async Task<List<PedidoDetalle>> GetDetallesByOrdenAsync(long idOrden)
        {
            // Buscamos todos los pedidos para esa orden:
            var pedidosIds = await _context.Pedidos
                .AsNoTracking()
                .Where(p => p.IdOrden == idOrden)
                .Select(p => p.IdPedido)
                .ToListAsync();

            if (!pedidosIds.Any())
                return new List<PedidoDetalle>();

            // Traemos todos los detalles de todos esos pedidos
            return await _context.PedidoDetalles
                .AsNoTracking()
                .Where(d => pedidosIds.Contains(d.IdPedido))
                .Include(d => d.IdProductoNavigation)
            
                .ToListAsync();
        }

        /// <summary>
        /// Crea un presupuesto a partir de un PresupuestoDto.
        /// Ahora:
        /// - Se agrupa cada línea por IdProveedor,
        /// - Se crea un Pedido para cada proveedor distinto,
        /// - Se crean sus respectivos PedidoDetalle (sin “IdProveedor” aquí, porque ya está en Pedido),
        /// - Se calcula MontoTotal por Pedido,
        /// - Se actualiza el estado de la orden a “Pendiente”.
        /// </summary>
        public async Task<bool> CreatePedidosAsync(CrearPedidosDto dto)
        {
            // 1) Validar que la orden exista
            var orden = await _context.Ordenes.FindAsync(dto.OrdenId);
            if (orden == null)
                return false;

            using var tx = await _context.Database.BeginTransactionAsync();
            try
            {
                // 2) Agrupar las líneas de creación por IdProveedor
                var lineasPorProveedor = dto.Detalles
                    .GroupBy(d => d.IdProveedor)
                    .ToDictionary(
                        g => g.Key,
                        g => g.ToList()
                    );

                // 3) Para cada proveedor, crear un Pedido y sus detalles
                foreach (var proveedorId in lineasPorProveedor.Keys)
                {
                    var lineasParaEsteProveedor = lineasPorProveedor[proveedorId];

                    // 3.1) Obtener la FechaEntrega común para este proveedor:
                    //      asumimos que todas las líneas del mismo proveedor tienen la misma FechaEntrega.
                    var fechaEntregaProveedor = lineasParaEsteProveedor
                        .Select(d => d.FechaEntrega)
                        .FirstOrDefault();

                    // 3.2) Crear el Pedido (sin monto aún)
                    var nuevoPedido = new Pedido
                    {
                        IdOrden = dto.OrdenId,
                        IdProveedor = proveedorId,
                        FechaPedido = DateOnly.FromDateTime(DateTime.Today),
                        FechaEntrega = fechaEntregaProveedor,
                        Estado = "PENDIENTE",    // o el estado que corresponda
                        MontoTotal = 0m          // se calculará luego
                    };
                    await _context.Pedidos.AddAsync(nuevoPedido);
                    await _context.SaveChangesAsync();
                    // EF asigna nuevoPedido.IdPedido

                    // 3.3) Crear todas las líneas (PedidoDetalle) para este pedido
                    var detallesPedidos = lineasParaEsteProveedor
                        .Select(d => new PedidoDetalle
                        {
                            IdPedido = nuevoPedido.IdPedido,
                            IdProducto = d.IdProducto,
                            Cotizacion = d.Cotizacion,
                            Cantidad = d.Cantidad,
                            Iva = d.Iva
                        })
                        .ToList();

                    await _context.PedidoDetalles.AddRangeAsync(detallesPedidos);
                    await _context.SaveChangesAsync();

                    // 3.4) Calcular y actualizar MontoTotal de este pedido
                    nuevoPedido.MontoTotal = detallesPedidos
                        .Sum(x => (x.Cotizacion * x.Cantidad) + x.Iva);
                    _context.Pedidos.Update(nuevoPedido);
                    await _context.SaveChangesAsync();
                }

                // 4) Cambiar el estado de la orden a “Completa”
                orden.Estado = "COMPLETA";
                _context.Ordenes.Update(orden);
                await _context.SaveChangesAsync();

                await tx.CommitAsync();
                return true;
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }

        public async Task<List<PedidoConDetallesDto>> GetPedidosConDetallesByOrdenAsync(long idOrden)
        {
            // 1) Traer todos los Pedidos que referencian esta Orden, incluyendo Proveedor y sus Detalles con Producto
            var pedidosConNav = await _context.Pedidos
                .AsNoTracking()
                .Where(p => p.IdOrden == idOrden)
                .Include(p => p.IdProveedorNavigation)                 // Para obtener nombre del proveedor
                .Include(p => p.PedidoDetalles)                         // Para obtener cada detalle
                    .ThenInclude(d => d.IdProductoNavigation)           // Para obtener nombre del producto
                .ToListAsync();

            // 2) Mapear cada Pedido a PedidoConDetallesDto
            var resultado = pedidosConNav.Select(pedido =>
            {
                var dto = new PedidoConDetallesDto
                {
                    IdPedido = pedido.IdPedido,
                    Proveedor = pedido.IdProveedorNavigation?.Nombre ?? "",
                    FechaPedido = pedido.FechaPedido ?? default(DateOnly),
                    FechaEntrega = pedido.FechaEntrega,
                    MontoTotal = pedido.MontoTotal ?? 0m,
                    CorreoProveedor = pedido.IdProveedorNavigation?.Correo ?? "", // 👈 Agregado
                    Detalles = pedido.PedidoDetalles.Select(d => new PedidoDetalleSimpleDto
                    {
                        IdPedidoDetalle = d.IdPedidoDetalle,
                        Producto = d.IdProductoNavigation?.Nombre ?? "",
                        Cotizacion = d.Cotizacion ?? 0m,
                        Cantidad = d.Cantidad ?? 0,
                        Iva = d.Iva ?? 0m,
                        Subtotal = ((d.Cotizacion ?? 0m) * (d.Cantidad ?? 0)) + (d.Iva ?? 0m)
                    }).ToList()
                };
                return dto;
            })
            .ToList();

            return resultado;
        }

        /// <summary>
        /// Elimina la orden, todos sus pedidos y todos los PedidoDetalle relacionados.
        /// (Se eliminó cualquier referencia a OrdenDetalle, pues ya no existe.)
        /// </summary>
        public async Task<bool> DeleteOrdenAsync(long idOrden)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Incluir únicamente los pedidos; ya no hay OrdenDetalles en la entidad Orden
                var orden = await _context.Ordenes
                    .Include(o => o.Pedidos)
                    .FirstOrDefaultAsync(o => o.IdOrden == idOrden);

                if (orden == null)
                    return false;

                // Eliminar todos los pedidos y sus detalles
                foreach (var pedido in orden.Pedidos)
                {
                    var detalles = await _context.PedidoDetalles
                        .Where(d => d.IdPedido == pedido.IdPedido)
                        .ToListAsync();

                    if (detalles.Any())
                        _context.PedidoDetalles.RemoveRange(detalles);

                    _context.Pedidos.Remove(pedido);
                }

                // Finalmente, eliminar la orden
                _context.Ordenes.Remove(orden);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
