using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackendApp.Data;
using BackendApp.DTOs;
using BackendApp.Modelss;

namespace BackendApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacturasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FacturasController(ApplicationDbContext context)
            => _context = context;

        // GET: api/Facturas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Factura>>> GetAll()
        {
            // Incluimos los detalles para devolverlos juntos
            var list = await _context.Facturas
                                     .Include(f => f.Detalles)
                                     .ToListAsync();
            return Ok(list);
        }

        // GET: api/Facturas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Factura>> Get(int id)
        {
            var factura = await _context.Facturas
                                        .Include(f => f.Detalles)
                                        .FirstOrDefaultAsync(f => f.Id_Factura == id);
            if (factura == null) return NotFound();
            return Ok(factura);
        }

        // POST: api/Facturas
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Factura factura)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (factura.Detalles == null || !factura.Detalles.Any())
                return BadRequest(new { error = "Debe especificar al menos un detalle." });

            // Agregamos la factura y sus detalles
            _context.Facturas.Add(factura);
            await _context.SaveChangesAsync();

            // Recargamos los detalles para la respuesta
            await _context.Entry(factura)
                          .Collection(f => f.Detalles)
                          .LoadAsync();

            return CreatedAtAction(nameof(Get),
                                   new { id = factura.Id_Factura },
                                   factura);
        }

        // PUT: api/Facturas/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Factura factura)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != factura.Id_Factura)
                return BadRequest();

            // Marcamos la cabecera como modificada
            _context.Entry(factura).State = EntityState.Modified;

            // Y sincronizamos cada detalle: los nuevos Added, los existentes Modified
            foreach (var d in factura.Detalles)
            {
                if (d.Id_Factura_Detalle == 0)
                    _context.Entry(d).State = EntityState.Added;
                else
                    _context.Entry(d).State = EntityState.Modified;
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/Facturas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var factura = await _context.Facturas.FindAsync(id);
            if (factura == null) return NotFound();

            _context.Facturas.Remove(factura);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
