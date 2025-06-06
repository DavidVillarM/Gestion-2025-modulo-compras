using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackendApp.Data;
using BackendApp.DTOs;
using BackendApp.Modelss;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BackendApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidosController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public PedidosController(ApplicationDbContext db) => _db = db;

        // ─── 1) GET /api/pedidos ───
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var pedidos = await _db.Pedidos
                .Include(p => p.Proveedor)
                .Include(p => p.Detalles)
                    .ThenInclude(d => d.Producto)
                .ToListAsync();

            var result = pedidos.Select(p => new PedidoReadDto
            {
                IdPedido = p.IdPedido,
                IdOrden = p.IdOrden,
                IdProveedor = p.IdProveedor,
                NombreProveedor = p.Proveedor.Nombre,
                MontoTotal = p.MontoTotal,
                FechaPedido = p.FechaPedido,
                FechaEntrega = p.FechaEntrega,
                Estado = p.Estado,
                Detalles = p.Detalles.Select(d => new PedidoDetalleReadDto
                {
                    IdPedidoDetalle = d.IdPedidoDetalle,
                    IdProducto = d.IdProducto,
                    Cantidad = d.Cantidad,
                    Cotizacion = d.Cotizacion,
                    Iva = d.Iva,
                    NombreProducto = d.Producto.Nombre
                }).ToList()
            }).ToList();

            return Ok(result);
        }

        // ─── 2) GET /api/pedidos/{id} ───
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var pedido = await _db.Pedidos
                .Include(p => p.Proveedor)
                .Include(p => p.Detalles)
                    .ThenInclude(d => d.Producto)
                .FirstOrDefaultAsync(p => p.IdPedido == id);

            if (pedido == null)
                return NotFound(new { Message = $"El pedido {id} no existe." });

            var dto = new PedidoReadDto
            {
                IdPedido = pedido.IdPedido,
                IdOrden = pedido.IdOrden,
                IdProveedor = pedido.IdProveedor,
                NombreProveedor = pedido.Proveedor.Nombre,
                MontoTotal = pedido.MontoTotal,
                FechaPedido = pedido.FechaPedido,
                FechaEntrega = pedido.FechaEntrega,
                Estado = pedido.Estado,
                Detalles = pedido.Detalles.Select(d => new PedidoDetalleReadDto
                {
                    IdPedidoDetalle = d.IdPedidoDetalle,
                    IdProducto = d.IdProducto,
                    Cantidad = d.Cantidad,
                    Cotizacion = d.Cotizacion,
                    Iva = d.Iva,
                    NombreProducto = d.Producto.Nombre
                }).ToList()
            };

            return Ok(dto);
        }

        // ─── 3) POST /api/pedidos ─── (opcional, creación manual)
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PedidoCreateDto dto)
        {
            var orden = await _db.Ordenes.FindAsync(dto.IdOrden);
            if (orden == null)
                return NotFound(new { Message = $"La orden {dto.IdOrden} no existe." });

            var prov = await _db.Proveedores.FindAsync(dto.IdProveedor);
            if (prov == null)
                return NotFound(new { Message = $"El proveedor {dto.IdProveedor} no existe." });

            var prodIds = dto.Detalles.Select(d => d.IdProducto).Distinct().ToList();
            var existentes = await _db.Productos
                .Where(p => prodIds.Contains(p.IdProducto))
                .Select(p => p.IdProducto)
                .ToListAsync();
            var faltantes = prodIds.Except(existentes).ToList();
            if (faltantes.Any())
                return BadRequest(new { Message = $"Productos no encontrados: {string.Join(", ", faltantes)}" });

            var pedido = new Pedido
            {
                IdOrden = dto.IdOrden,
                IdProveedor = dto.IdProveedor,
                MontoTotal = dto.MontoTotal,
                FechaPedido = dto.FechaPedido,
                FechaEntrega = dto.FechaEntrega,
                Estado = dto.Estado
            };

            foreach (var d in dto.Detalles)
            {
                pedido.Detalles.Add(new PedidoDetalle
                {
                    IdProducto = d.IdProducto,
                    Cantidad = d.Cantidad,
                    Cotizacion = d.Cotizacion,
                    Iva = d.Iva
                });
            }

            _db.Pedidos.Add(pedido);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = pedido.IdPedido }, new { pedido.IdPedido });
        }

        // ─── 4) DELETE /api/pedidos/{id} ───
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var pedido = await _db.Pedidos.FindAsync(id);
            if (pedido == null)
                return NotFound(new { Message = $"El pedido {id} no existe." });

            _db.Pedidos.Remove(pedido);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        // ─── 5) POST /api/pedidos/{idOrden}/generar-auto ───
        [HttpPost("{idOrden}/generar-auto")]
        public async Task<IActionResult> GenerarAuto(int idOrden)
        {
            var orden = await _db.Ordenes.FindAsync(idOrden);
            if (orden == null)
                return NotFound(new { Message = $"La orden {idOrden} no existe." });

            var todosDetalles = await _db.PresupuestoDetalles
                .Where(d => d.Presupuesto.IdOrden == idOrden)
                .Include(d => d.Presupuesto)
                .ToListAsync();

            if (!todosDetalles.Any())
                return BadRequest(new { Message = "No hay cotizaciones cargadas para esta orden." });

            var mejoresPorProducto = todosDetalles
                .GroupBy(d => d.IdProducto)
                .Select(g => g.OrderBy(x => x.Precio).First())
                .ToList();

            var agrupados = mejoresPorProducto
                .GroupBy(d => d.Presupuesto.IdProveedor)
                .ToList();

            var listaPedidos = new List<PedidoReadDto>();

            foreach (var grupo in agrupados)
            {
                var proveedorId = grupo.Key;
                var pedido = new Pedido
                {
                    IdOrden = idOrden,
                    IdProveedor = proveedorId,
                    MontoTotal = 0m,
                    FechaPedido = DateTime.UtcNow,
                    FechaEntrega = DateTime.UtcNow.AddDays(7),
                    Estado = "GENERADA"
                };

                foreach (var detalleMejor in grupo)
                {
                    var cantidad = detalleMejor.Cantidad;
                    var precio = detalleMejor.Precio;
                    var ivaTotal = detalleMejor.Iva5 + detalleMejor.Iva10;

                    pedido.Detalles.Add(new PedidoDetalle
                    {
                        IdProducto = detalleMejor.IdProducto,
                        Cantidad = cantidad,
                        Cotizacion = precio,
                        Iva = ivaTotal
                    });

                    pedido.MontoTotal += (precio * cantidad) + ivaTotal;
                }

                _db.Pedidos.Add(pedido);
                await _db.SaveChangesAsync();

                await _db.Entry(pedido).Reference(p => p.Proveedor).LoadAsync();
                await _db.Entry(pedido)
                    .Collection(p => p.Detalles)
                    .Query()
                    .Include(d => d.Producto)
                    .LoadAsync();

                var pedidoDto = new PedidoReadDto
                {
                    IdPedido = pedido.IdPedido,
                    IdOrden = pedido.IdOrden,
                    IdProveedor = pedido.IdProveedor,
                    NombreProveedor = pedido.Proveedor.Nombre,
                    MontoTotal = pedido.MontoTotal,
                    FechaPedido = pedido.FechaPedido,
                    FechaEntrega = pedido.FechaEntrega,
                    Estado = pedido.Estado,
                    Detalles = pedido.Detalles.Select(d => new PedidoDetalleReadDto
                    {
                        IdPedidoDetalle = d.IdPedidoDetalle,
                        IdProducto = d.IdProducto,
                        Cantidad = d.Cantidad,
                        Cotizacion = d.Cotizacion,
                        Iva = d.Iva,
                        NombreProducto = d.Producto.Nombre
                    }).ToList()
                };

                listaPedidos.Add(pedidoDto);
            }

            return Ok(listaPedidos);
        }
    }
}
