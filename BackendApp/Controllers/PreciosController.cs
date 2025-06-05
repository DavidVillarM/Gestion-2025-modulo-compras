// Controllers/PreciosController.cs
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModuloCompras.Data;
using ModuloCompras.Models;

namespace ModuloCompras.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PreciosController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public PreciosController(ApplicationDbContext db) => _db = db;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Precio>>> GetAll()
        {
            var list = await _db.Precios
                .Include(p => p.Producto)
                .ToListAsync();
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Precio>> Get(long id)
        {
            var precio = await _db.Precios.FindAsync(id);
            if (precio == null) return NotFound();
            return Ok(precio);
        }

        [HttpPost]
        public async Task<ActionResult<Precio>> Create([FromBody] Precio model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            _db.Precios.Add(model);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = model.IdPrecio }, model);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, [FromBody] Precio model)
        {
            if (id != model.IdPrecio) return BadRequest();
            _db.Entry(model).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var precio = await _db.Precios.FindAsync(id);
            if (precio == null) return NotFound();
            _db.Precios.Remove(precio);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
