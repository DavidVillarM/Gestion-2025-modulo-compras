using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackendApp.Data;
using BackendApp.DTOs;
using BackendApp.Modelss;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ModuloCompras.Controllers
{
    [ApiController]
    [Route("api/ordenes/{ordenId}/detalles")]
    public class OrdenDetallesController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public OrdenDetallesController(ApplicationDbContext db) => _db = db;

        // ─── GET: api/ordenes/{ordenId}/detalles ───
        [HttpGet]
        public async Task<IActionResult> GetAll(int ordenId)
        {
            var existe = await _db.Ordenes.AnyAsync(o => o.IdOrden == ordenId);
            if (!existe)
                return NotFound(new { Message = $"La orden {ordenId} no existe." });

            var detalles = await _db.OrdenDetalles
                .Where(d => d.IdOrden == ordenId)
                .Include(d => d.Producto)
                    .ThenInclude(p => p.Categoria)
                .ToListAsync();

            var result = detalles.Select(d => new OrdenDetalleReadDto
            {
                IdOrdenDetalle = d.IdOrdenDetalle,
                IdProducto = d.IdProducto,
                Cantidad = d.Cantidad,
                NombreProducto = d.Producto.Nombre,
                IdCategoria = d.Producto.IdCategoria,
                CategoriaNombre = d.Producto.Categoria.Nombre
            }).ToList();

            return Ok(result);
        }

        // ─── GET: api/ordenes/{ordenId}/detalles/{id} ───
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int ordenId, int id)
        {
            var detalle = await _db.OrdenDetalles
                .Include(d => d.Producto)
                    .ThenInclude(p => p.Categoria)
                .FirstOrDefaultAsync(d => d.IdOrdenDetalle == id && d.IdOrden == ordenId);

            if (detalle == null)
                return NotFound(new { Message = $"El detalle {id} no existe para la orden {ordenId}." });

            var dto = new OrdenDetalleReadDto
            {
                IdOrdenDetalle = detalle.IdOrdenDetalle,
                IdProducto = detalle.IdProducto,
                Cantidad = detalle.Cantidad,
                NombreProducto = detalle.Producto.Nombre,
                IdCategoria = detalle.Producto.IdCategoria,
                CategoriaNombre = detalle.Producto.Categoria.Nombre
            };
            return Ok(dto);
        }

        // ─── POST: api/ordenes/{ordenId}/detalles ───
        [HttpPost]
        public async Task<IActionResult> Create(int ordenId, [FromBody] OrdenDetalleCreateDto dto)
        {
            var orden = await _db.Ordenes.FindAsync(ordenId);
            if (orden == null)
                return NotFound(new { Message = $"La orden {ordenId} no existe." });

            var prod = await _db.Productos
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(p => p.IdProducto == dto.IdProducto);
            if (prod == null)
                return BadRequest(new { Message = $"Producto {dto.IdProducto} no encontrado." });

            var entity = new OrdenDetalle
            {
                IdOrden = ordenId,
                IdProducto = dto.IdProducto,
                Cantidad = dto.Cantidad
            };

            _db.OrdenDetalles.Add(entity);
            await _db.SaveChangesAsync();

            var readDto = new OrdenDetalleReadDto
            {
                IdOrdenDetalle = entity.IdOrdenDetalle,
                IdProducto = entity.IdProducto,
                Cantidad = entity.Cantidad,
                NombreProducto = prod.Nombre,
                IdCategoria = prod.IdCategoria,
                CategoriaNombre = prod.Categoria.Nombre
            };
            return CreatedAtAction(
                nameof(GetById),
                new { ordenId = ordenId, id = entity.IdOrdenDetalle },
                readDto);
        }

        // ─── PUT: api/ordenes/{ordenId}/detalles/{id} ───
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int ordenId, int id, [FromBody] OrdenDetalleCreateDto dto)
        {
            var orden = await _db.Ordenes.FindAsync(ordenId);
            if (orden == null)
                return NotFound(new { Message = $"La orden {ordenId} no existe." });

            var entity = await _db.OrdenDetalles
                .FirstOrDefaultAsync(d => d.IdOrdenDetalle == id && d.IdOrden == ordenId);
            if (entity == null)
                return NotFound(new { Message = $"El detalle {id} no existe para la orden {ordenId}." });

            var prod = await _db.Productos.FindAsync(dto.IdProducto);
            if (prod == null)
                return BadRequest(new { Message = $"Producto {dto.IdProducto} no encontrado." });

            entity.IdProducto = dto.IdProducto;
            entity.Cantidad = dto.Cantidad;
            await _db.SaveChangesAsync();
            return NoContent();
        }

        // ─── DELETE: api/ordenes/{ordenId}/detalles/{id} ───
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int ordenId, int id)
        {
            var entity = await _db.OrdenDetalles
                .FirstOrDefaultAsync(d => d.IdOrdenDetalle == id && d.IdOrden == ordenId);
            if (entity == null)
                return NotFound(new { Message = $"El detalle {id} no existe para la orden {ordenId}." });

            _db.OrdenDetalles.Remove(entity);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
