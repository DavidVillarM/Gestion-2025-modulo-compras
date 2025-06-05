using System.Collections.Generic;
using System.Linq;
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
    public class CategoriasController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public CategoriasController(ApplicationDbContext db) => _db = db;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Categoria>>> GetAll()
            => Ok(await _db.Categorias.ToListAsync());

        [HttpGet("{id}")]
        public async Task<ActionResult<Categoria>> Get(int id)
        {
            var cat = await _db.Categorias.FindAsync(id);
            if (cat == null) return NotFound();
            return Ok(cat);
        }

        [HttpPost]
        public async Task<ActionResult<Categoria>> Create([FromBody] CategoriaCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var cat = new Categoria
            {
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion
            };
            _db.Categorias.Add(cat);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(Get),
                                   new { id = cat.IdCategoria },
                                   cat);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Categoria cat)
        {
            if (id != cat.IdCategoria) return BadRequest();
            _db.Entry(cat).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var cat = await _db.Categorias.FindAsync(id);
            if (cat == null) return NotFound();
            _db.Categorias.Remove(cat);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        // Asignar una categoría a un proveedor
        [HttpPost("asignar")]
        public async Task<IActionResult> Asignar([FromBody] CategoriaProveedorCreateDto dto)
        {
            if (!await _db.Proveedores.AnyAsync(p => p.IdProveedor == dto.IdProveedor))
                return BadRequest($"Proveedor {dto.IdProveedor} no existe.");
            if (!await _db.Categorias.AnyAsync(c => c.IdCategoria == dto.IdCategoria))
                return BadRequest($"Categoría {dto.IdCategoria} no existe.");

            var already = await _db.CategoriaProveedores
                .AnyAsync(cp => cp.IdProveedor == dto.IdProveedor
                             && cp.IdCategoria == dto.IdCategoria);
            if (already) return Conflict("Asignación ya existe.");

            var cp = new CategoriaProveedor
            {
                IdProveedor = dto.IdProveedor,
                IdCategoria = dto.IdCategoria,
                Estado = dto.Estado
            };
            _db.CategoriaProveedores.Add(cp);
            await _db.SaveChangesAsync();
            return Ok(cp);
        }

        // Desasignar
        [HttpDelete("desasignar")]
        public async Task<IActionResult> Desasignar([FromBody] CategoriaProveedorCreateDto dto)
        {
            var cp = await _db.CategoriaProveedores
                .FirstOrDefaultAsync(x => x.IdProveedor == dto.IdProveedor
                                       && x.IdCategoria == dto.IdCategoria);
            if (cp == null) return NotFound();

            _db.CategoriaProveedores.Remove(cp);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
