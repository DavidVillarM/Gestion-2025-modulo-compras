using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModuloCompras.Data;
using ModuloCompras.Models;
using ModuloCompras.DTOs;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ModuloCompras.Controllers
{
    [ApiController]
    [Route("api/presupuestos/{presupuestoId}/detalles")]
    public class PresupuestoDetallesController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public PresupuestoDetallesController(ApplicationDbContext db) => _db = db;

        // ─── GET: api/presupuestos/{presupuestoId}/detalles ───
        [HttpGet]
        public async Task<IActionResult> GetAll(int presupuestoId)
        {
            var existe = await _db.Presupuestos.AnyAsync(p => p.IdPresupuesto == presupuestoId);
            if (!existe)
                return NotFound(new { Message = $"El presupuesto {presupuestoId} no existe." });

            var detalles = await _db.PresupuestoDetalles
                .Where(d => d.IdPresupuesto == presupuestoId)
                .Include(d => d.Producto)
                .ToListAsync();

            var result = detalles.Select(d => new PresupuestoDetalleReadDto
            {
                IdPresupuestoDetalle = d.IdPresupuestoDetalle,
                IdProducto = d.IdProducto,
                Cantidad = d.Cantidad,
                Precio = d.Precio,
                Iva5 = d.Iva5,
                Iva10 = d.Iva10,
                NombreProducto = d.Producto.Nombre
            }).ToList();

            return Ok(result);
        }

        // ─── GET: api/presupuestos/{presupuestoId}/detalles/{id} ───
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int presupuestoId, int id)
        {
            var detalle = await _db.PresupuestoDetalles
                .Include(d => d.Producto)
                .FirstOrDefaultAsync(d => d.IdPresupuestoDetalle == id && d.IdPresupuesto == presupuestoId);

            if (detalle == null)
                return NotFound(new { Message = $"El detalle {id} no existe para el presupuesto {presupuestoId}." });

            var dto = new PresupuestoDetalleReadDto
            {
                IdPresupuestoDetalle = detalle.IdPresupuestoDetalle,
                IdProducto = detalle.IdProducto,
                Cantidad = detalle.Cantidad,
                Precio = detalle.Precio,
                Iva5 = detalle.Iva5,
                Iva10 = detalle.Iva10,
                NombreProducto = detalle.Producto.Nombre
            };
            return Ok(dto);
        }

        // ─── POST: api/presupuestos/{presupuestoId}/detalles ───
        [HttpPost]
        public async Task<IActionResult> Create(int presupuestoId, [FromBody] PresupuestoDetalleCreateDto dto)
        {
            var presu = await _db.Presupuestos.FindAsync(presupuestoId);
            if (presu == null)
                return NotFound(new { Message = $"El presupuesto {presupuestoId} no existe." });

            var prod = await _db.Productos.FindAsync(dto.IdProducto);
            if (prod == null)
                return BadRequest(new { Message = $"Producto {dto.IdProducto} no encontrado." });

            var entity = new PresupuestoDetalle
            {
                IdPresupuesto = presupuestoId,
                IdProducto = dto.IdProducto,
                Cantidad = dto.Cantidad,
                Precio = dto.Precio,
                Iva5 = dto.Iva5,
                Iva10 = dto.Iva10
            };

            _db.PresupuestoDetalles.Add(entity);
            await _db.SaveChangesAsync();

            var readDto = new PresupuestoDetalleReadDto
            {
                IdPresupuestoDetalle = entity.IdPresupuestoDetalle,
                IdProducto = entity.IdProducto,
                Cantidad = entity.Cantidad,
                Precio = entity.Precio,
                Iva5 = entity.Iva5,
                Iva10 = entity.Iva10,
                NombreProducto = prod.Nombre
            };

            return CreatedAtAction(
                nameof(GetById),
                new { presupuestoId = presupuestoId, id = entity.IdPresupuestoDetalle },
                readDto);
        }

        // ─── PUT: api/presupuestos/{presupuestoId}/detalles/{id} ───
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int presupuestoId, int id, [FromBody] PresupuestoDetalleCreateDto dto)
        {
            var presu = await _db.Presupuestos.FindAsync(presupuestoId);
            if (presu == null)
                return NotFound(new { Message = $"El presupuesto {presupuestoId} no existe." });

            var entity = await _db.PresupuestoDetalles
                .FirstOrDefaultAsync(d => d.IdPresupuestoDetalle == id && d.IdPresupuesto == presupuestoId);
            if (entity == null)
                return NotFound(new { Message = $"El detalle {id} no existe para el presupuesto {presupuestoId}." });

            var prod = await _db.Productos.FindAsync(dto.IdProducto);
            if (prod == null)
                return BadRequest(new { Message = $"Producto {dto.IdProducto} no encontrado." });

            entity.IdProducto = dto.IdProducto;
            entity.Cantidad = dto.Cantidad;
            entity.Precio = dto.Precio;
            entity.Iva5 = dto.Iva5;
            entity.Iva10 = dto.Iva10;

            await _db.SaveChangesAsync();
            return NoContent();
        }

        // ─── DELETE: api/presupuestos/{presupuestoId}/detalles/{id} ───
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int presupuestoId, int id)
        {
            var entity = await _db.PresupuestoDetalles
                .FirstOrDefaultAsync(d => d.IdPresupuestoDetalle == id && d.IdPresupuesto == presupuestoId);
            if (entity == null)
                return NotFound(new { Message = $"El detalle {id} no existe para el presupuesto {presupuestoId}." });

            _db.PresupuestoDetalles.Remove(entity);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
