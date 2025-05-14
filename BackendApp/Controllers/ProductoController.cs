
using BackendApp.Data;
using BackendApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace BackendApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class ProductoController : ControllerBase
    {
        private readonly AppDbContext _context;


        public ProductoController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<Producto>> PostProducto(Producto producto)
        {
            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetProductosById), new { id = producto.IdProducto }, producto);
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Producto>>> GetProductos()
        {
            return await _context.Productos.ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Producto>> GetProductosById(long id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }
            return producto;
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Producto>> PutProducto(Producto producto, long id)
        {
            var productoEdit = await _context.Productos.FindAsync(id);
            if (productoEdit == null)
            {
                return NotFound();
            }
            productoEdit.Nombre = producto.Nombre;
            productoEdit.Marca = producto.Marca;
            productoEdit.IdCategoria = producto.IdCategoria;
            productoEdit.CantidadTotal = producto.CantidadTotal;
            productoEdit.CantidadMinima = producto.CantidadMinima;

            await _context.SaveChangesAsync();

            return Ok(productoEdit);

        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<Producto>>DeleteProducto (long id)
        {

            var productoDelete = await _context.Productos.FindAsync(id);
            if (productoDelete == null)
            {
                return NotFound();
            }


            _context.Productos.Remove(productoDelete);
            await _context.SaveChangesAsync();
            return Ok(productoDelete);

        }

    }

}