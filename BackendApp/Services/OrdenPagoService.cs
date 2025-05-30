
// File: Services/OrdenPagoService.cs
using BackendApp.Models;
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
        public DateOnly? FechaPedido { get; set; }
        public decimal MontoTotal { get; set; }
        public string Estado { get; set; }
    }


    // DTO para generar presupuesto
    public class PresupuestoDetalleDto
    {
        public long IdProducto { get; set; }
        public long IdProveedor { get; set; }
        public decimal Cotizacion { get; set; }
        public int Cantidad { get; set; }
        public decimal Iva { get; set; }
    }

    public class PresupuestoDto
    {
        public long OrdenId { get; set; }
        public List<PresupuestoDetalleDto> Detalles { get; set; }
    }

    public class OrdenPagoService
    {
        private readonly PostgresContext _context;

        public OrdenPagoService(PostgresContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Trae todas las órdenes con su fecha de pedido,
        /// el monto total de su único pedido, y el estado de la orden.
        /// </summary>
        public async Task<List<OrdenPagoDto>> GetOrdenesPagoAsync()
        {
            var query = _context.Ordenes
                .AsNoTracking()
                .Select(o => new OrdenPagoDto
                {
                    IdOrden = o.IdOrden,
                    Estado = o.Estado,
                    MontoTotal = o.Pedidos
                        .Sum(p => p.MontoTotal ?? 0m),
                    FechaPedido = o.Pedidos
                     .Select(p => p.FechaPedido)      
                     .FirstOrDefault()                
                });

            return await query.ToListAsync();
        }

        /// <summary>
        /// Elimina la orden, junto con sus detalles y su pedido asociado.
        /// </summary>

        public async Task<List<PedidoDetalle>> GetDetallesByOrdenAsync(long idOrden)
        {
            var pedido = await _context.Pedidos
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.IdOrden == idOrden);

            if (pedido == null)
                return new List<PedidoDetalle>();

            return await _context.PedidoDetalles
                .AsNoTracking()
                .Where(d => d.IdPedido == pedido.IdPedido)
                .Include(d => d.IdProductoNavigation)
                .Include(d => d.IdProveedorNavigation)
                .ToListAsync();
        }
        public async Task<bool> CreatePresupuestoAsync(PresupuestoDto dto)
        {
            // 1) Validar que la orden exista
            var orden = await _context.Ordenes.FindAsync(dto.OrdenId);
            if (orden == null)
                return false;

            using var tx = await _context.Database.BeginTransactionAsync();
            try
            {
                // 2) Crear el pedido (todavía sin monto total)
                var pedido = new Pedido
                {
                    IdOrden = dto.OrdenId,
                    FechaPedido = DateOnly.FromDateTime(DateTime.Now),
                    FechaEntrega = null,
                    Estado = "Pendiente",
                    MontoTotal = 0m   // inicialmente 0, luego lo actualizamos
                };
                await _context.Pedidos.AddAsync(pedido);
                await _context.SaveChangesAsync();
                // con esto EF ya llenó pedido.IdPedido

                // 3) Crear los detalles del pedido
                var detalles = dto.Detalles.Select(d => new PedidoDetalle
                {
                    IdPedido = pedido.IdPedido,
                    IdProducto = d.IdProducto,
                    IdProveedor = d.IdProveedor,
                    Cotizacion = d.Cotizacion,
                    Cantidad = d.Cantidad,
                    Iva = d.Iva
                }).ToList();

                await _context.PedidoDetalles.AddRangeAsync(detalles);
                await _context.SaveChangesAsync();

                // 4) Calcular y actualizar el monto total
                pedido.MontoTotal = detalles.Sum(x => (x.Cotizacion * x.Cantidad) + x.Iva);
                _context.Pedidos.Update(pedido);

                // 5) Cambiar estado de la orden
                orden.Estado = "Pendiente";
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
        public async Task<bool> DeleteOrdenAsync(long idOrden)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var orden = await _context.Ordenes
                    .Include(o => o.Pedidos)
                    .Include(o => o.OrdenDetalles)
                    .FirstOrDefaultAsync(o => o.IdOrden == idOrden);

                if (orden == null)
                    return false;

                // Eliminar detalles de orden si existen
                if (orden.OrdenDetalles.Any())
                {
                    _context.OrdenDetalles.RemoveRange(orden.OrdenDetalles);
                }

                // Eliminar pedidos relacionados y sus detalles
                foreach (var pedido in orden.Pedidos)
                {
                    var detalles = await _context.PedidoDetalles
                        .Where(d => d.IdPedido == pedido.IdPedido)
                        .ToListAsync();

                    if (detalles.Any())
                    {
                        _context.PedidoDetalles.RemoveRange(detalles);
                    }

                    _context.Pedidos.Remove(pedido);
                }

                // Finalmente eliminar la orden
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
