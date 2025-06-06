using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackendApp.Data;
using BackendApp.DTOs;
using BackendApp.Modelss;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BackendApp.Controllers
{
    [ApiController]
    [Route("api/pedidos/{pedidoId}/detalles")]
    public class PedidoDetallesController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public PedidoDetallesController(ApplicationDbContext db) => _db = db;

        // ─── GET: api/pedidos/{pedidoId}/detalles ───
        [HttpGet]
        public async Task<IActionResult> GetAll(int pedidoId)
        {
            var existe = await _db.Pedidos.AnyAsync(p => p.IdPedido == pedidoId);
            if (!existe)
                return NotFound(new { Message = $"El pedido {pedidoId} no existe." });

            var detalles = await _db.PedidoDetalles
                .Where(d => d.IdPedido == pedidoId)
                .Include(d => d.Producto)
                .ToListAsync();

            var result = detalles.Select(d => new PedidoDetalleReadDto
            {
                IdPedidoDetalle = d.IdPedidoDetalle,
                IdProducto = d.IdProducto,
                Cantidad = d.Cantidad,
                Cotizacion = d.Cotizacion,
                Iva = d.Iva,
                NombreProducto = d.Producto.Nombre
            }).ToList();

            return Ok(result);
        }

        // ─── GET: api/pedidos/{pedidoId}/detalles/{id} ───
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int pedidoId, int id)
        {
            var detalle = await _db.PedidoDetalles
                .Include(d => d.Producto)
                .FirstOrDefaultAsync(d => d.IdPedidoDetalle == id && d.IdPedido == pedidoId);
            if (detalle == null)
                return NotFound(new { Message = $"El detalle {id} no existe para el pedido {pedidoId}." });

            var dto = new PedidoDetalleReadDto
            {
                IdPedidoDetalle = detalle.IdPedidoDetalle,
                IdProducto = detalle.IdProducto,
                Cantidad = detalle.Cantidad,
                Cotizacion = detalle.Cotizacion,
                Iva = detalle.Iva,
                NombreProducto = detalle.Producto.Nombre
            };
            return Ok(dto);
        }

        // ─── POST: api/pedidos/{pedidoId}/detalles ───
        [HttpPost]
        public async Task<IActionResult> Create(int pedidoId, [FromBody] PedidoDetalleCreateDto dto)
        {
            var pedido = await _db.Pedidos.FindAsync(pedidoId);
            if (pedido == null)
                return NotFound(new { Message = $"El pedido {pedidoId} no existe." });

            var prod = await _db.Productos.FindAsync(dto.IdProducto);
            if (prod == null)
                return BadRequest(new { Message = $"Producto {dto.IdProducto} no encontrado." });

            var entity = new PedidoDetalle
            {
                IdPedido = pedidoId,
                IdProducto = dto.IdProducto,
                Cantidad = dto.Cantidad,
                Cotizacion = dto.Cotizacion,
                Iva = dto.Iva
            };

            _db.PedidoDetalles.Add(entity);
            await _db.SaveChangesAsync();

            var readDto = new PedidoDetalleReadDto
            {
                IdPedidoDetalle = entity.IdPedidoDetalle,
                IdProducto = entity.IdProducto,
                Cantidad = entity.Cantidad,
                Cotizacion = entity.Cotizacion,
                Iva = entity.Iva,
                NombreProducto = prod.Nombre
            };

            return CreatedAtAction(
                nameof(GetById),
                new { pedidoId = pedidoId, id = entity.IdPedidoDetalle },
                readDto);
        }

        // ─── PUT: api/pedidos/{pedidoId}/detalles/{id} ───
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int pedidoId, int id, [FromBody] PedidoDetalleCreateDto dto)
        {
            var pedido = await _db.Pedidos.FindAsync(pedidoId);
            if (pedido == null)
                return NotFound(new { Message = $"El pedido {pedidoId} no existe." });

            var entity = await _db.PedidoDetalles
                .FirstOrDefaultAsync(d => d.IdPedidoDetalle == id && d.IdPedido == pedidoId);
            if (entity == null)
                return NotFound(new { Message = $"El detalle {id} no existe para el pedido {pedidoId}." });

            var prod = await _db.Productos.FindAsync(dto.IdProducto);
            if (prod == null)
                return BadRequest(new { Message = $"Producto {dto.IdProducto} no encontrado." });

            entity.IdProducto = dto.IdProducto;
            entity.Cantidad = dto.Cantidad;
            entity.Cotizacion = dto.Cotizacion;
            entity.Iva = dto.Iva;

            await _db.SaveChangesAsync();
            return NoContent();
        }

        // ─── DELETE: api/pedidos/{pedidoId}/detalles/{id} ───
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int pedidoId, int id)
        {
            var entity = await _db.PedidoDetalles
                .FirstOrDefaultAsync(d => d.IdPedidoDetalle == id && d.IdPedido == pedidoId);
            if (entity == null)
                return NotFound(new { Message = $"El detalle {id} no existe para el pedido {pedidoId}." });

            _db.PedidoDetalles.Remove(entity);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
