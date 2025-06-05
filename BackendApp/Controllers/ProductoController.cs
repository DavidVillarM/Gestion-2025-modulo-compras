//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using ModuloCompras.Data;
//using ModuloCompras.DTOs;
//using ModuloCompras.Models;

//namespace ModuloCompras.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class ProductosController : ControllerBase
//    {
//        private readonly ApplicationDbContext _context;
//        public ProductosController(ApplicationDbContext context) => _context = context;

//        // GET: api/productos
//        [HttpGet]
//        public async Task<ActionResult<IEnumerable<Producto>>> GetAll()
//        {
//            var products = await _context.Productos
//                .Include(p => p.Categoria)
//                .ToListAsync();
//            return Ok(products);
//        }

//        // GET: api/productos/{id}
//        [HttpGet("{id}")]
//        public async Task<ActionResult<Producto>> Get(int id)
//        {
//            var product = await _context.Productos
//                .Include(p => p.Categoria)
//                .FirstOrDefaultAsync(p => p.IdProducto == id);
//            if (product == null) return NotFound();
//            return Ok(product);
//        }

//        // POST: api/productos
//        [HttpPost]
//        public async Task<ActionResult<Producto>> Create([FromBody] ProductoCreateDto dto)
//        {
//            if (!ModelState.IsValid)
//                return BadRequest(ModelState);

//            // Asegúrate de que dto.IdCategoria tenga valor
//            if (!dto.IdCategoria.HasValue)
//                return BadRequest("IdCategoria es requerido.");

//            var product = new Producto
//            {
//                Nombre = dto.Nombre,
//                Marca = dto.Marca,
//                IdCategoria = dto.IdCategoria.Value,
//                CantidadTotal = dto.CantidadTotal,
//                CantidadMinima = dto.CantidadMinima
//            };

//            _context.Productos.Add(product);
//            await _context.SaveChangesAsync();

//            // Cargamos la navegación
//            await _context.Entry(product)
//                .Reference(p => p.Categoria)
//                .LoadAsync();

//            return CreatedAtAction(nameof(Get),
//                                   new { id = product.IdProducto },
//                                   product);
//        }

//        // PUT: api/productos/{id}
//        [HttpPut("{id}")]
//        public async Task<IActionResult> Update(int id, [FromBody] ProductoCreateDto dto)
//        {
//            if (!ModelState.IsValid)
//                return BadRequest(ModelState);

//            var product = await _context.Productos.FindAsync(id);
//            if (product == null) return NotFound();

//            if (!dto.IdCategoria.HasValue)
//                return BadRequest("IdCategoria es requerido.");

//            product.Nombre = dto.Nombre;
//            product.Marca = dto.Marca;
//            product.IdCategoria = dto.IdCategoria.Value;
//            product.CantidadTotal = dto.CantidadTotal;
//            product.CantidadMinima = dto.CantidadMinima;

//            await _context.SaveChangesAsync();
//            return NoContent();
//        }

//        // DELETE: api/productos/{id}
//        [HttpDelete("{id}")]
//        public async Task<IActionResult> Delete(int id)
//        {
//            var product = await _context.Productos.FindAsync(id);
//            if (product == null) return NotFound();

//            _context.Productos.Remove(product);
//            await _context.SaveChangesAsync();
//            return NoContent();
//        }

//        // GET: api/productos/{id}/proveedores
//        [HttpGet("{id}/proveedores")]
//        public async Task<ActionResult<IEnumerable<ProductoProveedor>>> GetProveedores(int id)
//        {
//            if (!await _context.Productos.AnyAsync(p => p.IdProducto == id))
//                return NotFound();

//            var list = await _context.ProductoProveedor
//                .Where(pp => pp.IdProducto == id)
//                .Include(pp => pp.Proveedor)
//                .ToListAsync();
//            return Ok(list);
//        }
//    }
//}
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModuloCompras.Data;
using ModuloCompras.Models;
using ModuloCompras.DTOs;

namespace ModuloCompras.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public ProductosController(ApplicationDbContext context) => _context = context;

        // ─── GET: api/productos ───
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<ProductoReadDto>>> GetAll()
        //{
        //    var productos = await _context.Productos
        //        .Include(p => p.Categoria)
        //        .ToListAsync();

        //    var dtos = productos.Select(p => new ProductoReadDto
        //    {
        //        IdProducto = p.IdProducto,
        //        Nombre = p.Nombre,
        //        Marca = p.Marca,
        //        IdCategoria = p.IdCategoria,
        //        NombreCategoria = p.Categoria.Nombre,
        //        CantidadTotal = p.CantidadTotal,
        //        CantidadMinima = p.CantidadMinima
        //    }).ToList();

        //    return Ok(dtos);
        //}

        [HttpGet]
        public async Task<ActionResult<List<ProductoReadDto>>> GetAll()
        {
            var lista = await _context.Productos
                .Select(p => new ProductoReadDto
                {
                    IdProducto = p.IdProducto,
                    Nombre = p.Nombre,
                    Marca = p.Marca,
                    IdCategoria = p.IdCategoria,
                    NombreCategoria = p.Categoria.Nombre,
                    CantidadTotal = p.CantidadTotal,
                    CantidadMinima = p.CantidadMinima
                })
                .ToListAsync();
            return Ok(lista);
        }

        // ─── GET: api/productos/{id} ───
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductoReadDto>> Get(int id)
        {
            var producto = await _context.Productos
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(p => p.IdProducto == id);

            if (producto == null)
                return NotFound(new { Message = $"Producto {id} no existe." });

            var dto = new ProductoReadDto
            {
                IdProducto = producto.IdProducto,
                Nombre = producto.Nombre,
                Marca = producto.Marca,
                IdCategoria = producto.IdCategoria,
                NombreCategoria = producto.Categoria.Nombre,
                CantidadTotal = producto.CantidadTotal,
                CantidadMinima = producto.CantidadMinima
            };
            return Ok(dto);
        }

        // ─── POST: api/productos ───
        [HttpPost]
        public async Task<ActionResult<ProductoReadDto>> Create([FromBody] ProductoCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!dto.IdCategoria.HasValue)
                return BadRequest(new { Message = "IdCategoria es requerido." });

            var cat = await _context.Categorias.FindAsync(dto.IdCategoria.Value);
            if (cat == null)
                return BadRequest(new { Message = $"Categoría {dto.IdCategoria} no existe." });

            var producto = new Producto
            {
                Nombre = dto.Nombre,
                Marca = dto.Marca,
                IdCategoria = dto.IdCategoria.Value,
                CantidadTotal = dto.CantidadTotal,
                CantidadMinima = dto.CantidadMinima
            };

            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();

            await _context.Entry(producto)
                .Reference(p => p.Categoria)
                .LoadAsync();

            var readDto = new ProductoReadDto
            {
                IdProducto = producto.IdProducto,
                Nombre = producto.Nombre,
                Marca = producto.Marca,
                IdCategoria = producto.IdCategoria,
                NombreCategoria = producto.Categoria.Nombre,
                CantidadTotal = producto.CantidadTotal,
                CantidadMinima = producto.CantidadMinima
            };

            return CreatedAtAction(nameof(Get), new { id = producto.IdProducto }, readDto);
        }

        // ─── PUT: api/productos/{id} ───
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProductoCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
                return NotFound(new { Message = $"Producto {id} no existe." });

            if (!dto.IdCategoria.HasValue)
                return BadRequest(new { Message = "IdCategoria es requerido." });

            var cat = await _context.Categorias.FindAsync(dto.IdCategoria.Value);
            if (cat == null)
                return BadRequest(new { Message = $"Categoría {dto.IdCategoria} no existe." });

            producto.Nombre = dto.Nombre;
            producto.Marca = dto.Marca;
            producto.IdCategoria = dto.IdCategoria.Value;
            producto.CantidadTotal = dto.CantidadTotal;
            producto.CantidadMinima = dto.CantidadMinima;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // ─── DELETE: api/productos/{id} ───
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
                return NotFound(new { Message = $"Producto {id} no existe." });

            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // ─── GET: api/productos/{id}/proveedores ───
        [HttpGet("{id}/proveedores")]
        public async Task<ActionResult<IEnumerable<ProductoProveedorReadDto>>> GetProveedores(int id)
        {
            var exists = await _context.Productos.AnyAsync(p => p.IdProducto == id);
            if (!exists)
                return NotFound(new { Message = $"Producto {id} no existe." });

            var list = await _context.ProductoProveedor
                .Where(pp => pp.IdProducto == id)
                .Include(pp => pp.Proveedor)
                .ToListAsync();

            var dtos = list.Select(pp => new ProductoProveedorReadDto
            {
                IdProductoProveedor = pp.IdProductoProveedor,
                IdProducto = pp.IdProducto,
                NombreProducto = pp.Producto.Nombre,
                IdProveedor = pp.IdProveedor,
                NombreProveedor = pp.Proveedor.Nombre,
                FechaCompra = pp.FechaCompra,
                Cantidad = pp.Cantidad
            }).ToList();

            return Ok(dtos);
        }

        [HttpGet("stock-bajo")]
        public async Task<ActionResult<List<ProductoReadDto>>> GetProductosStockBajo()
        {
            var lista = await _context.Productos
                .Where(p => p.CantidadTotal < p.CantidadMinima)
                .Select(p => new ProductoReadDto
                {
                    IdProducto = p.IdProducto,
                    Nombre = p.Nombre,
                    Marca = p.Marca,
                    IdCategoria = p.IdCategoria,
                    NombreCategoria = p.Categoria.Nombre,
                    CantidadTotal = p.CantidadTotal,
                    CantidadMinima = p.CantidadMinima
                })
                .ToListAsync();
            return Ok(lista);
        }


    }
}
