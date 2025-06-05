using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModuloCompras.Data;
using ModuloCompras.DTOs;
using ModuloCompras.Models;

namespace ModuloCompras.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaProveedorController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public CategoriaProveedorController(ApplicationDbContext db) => _db = db;

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CategoriaProveedorCreateDto dto)
        {
            if (!await _db.Proveedores.AnyAsync(p => p.IdProveedor == dto.IdProveedor))
                return BadRequest($"Proveedor {dto.IdProveedor} no existe.");
            if (!await _db.Categorias.AnyAsync(c => c.IdCategoria == dto.IdCategoria))
                return BadRequest($"Categoría {dto.IdCategoria} no existe.");

            var exists = await _db.CategoriaProveedores
                .AnyAsync(cp => cp.IdProveedor == dto.IdProveedor
                             && cp.IdCategoria == dto.IdCategoria);
            if (exists) return Conflict("Asignación ya existe.");

            var cp = new CategoriaProveedor
            {
                IdProveedor = dto.IdProveedor,
                IdCategoria = dto.IdCategoria,
                Estado = dto.Estado
            };
            _db.CategoriaProveedores.Add(cp);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetByIds),
                new { proveedorId = cp.IdProveedor, categoriaId = cp.IdCategoria },
                cp);
        }

        [HttpGet("{proveedorId}/{categoriaId}")]
        public async Task<IActionResult> GetByIds(int proveedorId, int categoriaId)
        {
            var cp = await _db.CategoriaProveedores
                .Include(x => x.Categoria)
                .Include(x => x.Proveedor)
                .FirstOrDefaultAsync(x =>
                    x.IdProveedor == proveedorId &&
                    x.IdCategoria == categoriaId);
            if (cp == null) return NotFound();
            return Ok(cp);
        }

        [HttpDelete("{proveedorId}/{categoriaId}")]
        public async Task<IActionResult> Delete(int proveedorId, int categoriaId)
        {
            var cp = await _db.CategoriaProveedores
                .FirstOrDefaultAsync(x =>
                    x.IdProveedor == proveedorId &&
                    x.IdCategoria == categoriaId);
            if (cp == null) return NotFound();

            _db.CategoriaProveedores.Remove(cp);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
