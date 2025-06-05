//using System.Collections.Generic;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using ModuloCompras.Data;
//using ModuloCompras.Models;

//namespace ModuloCompras.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class ProductoProveedorController : ControllerBase
//    {
//        private readonly ApplicationDbContext _db;
//        public ProductoProveedorController(ApplicationDbContext db) => _db = db;

//        // GET: api/productoproveedor
//        [HttpGet]
//        public async Task<ActionResult<IEnumerable<ProductoProveedor>>> GetAll()
//        {
//            var list = await _db.ProductoProveedor
//                .Include(pp => pp.Producto)
//                .Include(pp => pp.Proveedor)
//                .ToListAsync();
//            return Ok(list);
//        }

//        // POST: api/productoproveedor
//        [HttpPost]
//        public async Task<ActionResult<ProductoProveedor>> Create([FromBody] ProductoProveedor dto)
//        {
//            if (!ModelState.IsValid) return BadRequest(ModelState);

//            var validProd = await _db.Productos.AnyAsync(p => p.IdProducto == dto.IdProducto);
//            var validProv = await _db.Proveedores.AnyAsync(p => p.IdProveedor == dto.IdProveedor);
//            if (!validProd || !validProv)
//                return BadRequest("Producto o Proveedor inválido.");

//            _db.ProductoProveedor.Add(dto);
//            await _db.SaveChangesAsync();
//            return CreatedAtAction(nameof(GetAll),
//                                   new { id = dto.IdProductoProveedor },
//                                   dto);
//        }

//        // DELETE: api/productoproveedor/5
//        [HttpDelete("{id}")]
//        public async Task<IActionResult> Delete(int id)
//        {
//            var ent = await _db.ProductoProveedor.FindAsync(id);
//            if (ent == null) return NotFound();
//            _db.ProductoProveedor.Remove(ent);
//            await _db.SaveChangesAsync();
//            return NoContent();
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
    public class ProductoProveedorController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public ProductoProveedorController(ApplicationDbContext db) => _db = db;

        // ─── GET: api/productoproveedor ───
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductoProveedorReadDto>>> GetAll()
        {
            var list = await _db.ProductoProveedor
                .Include(pp => pp.Producto)
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

        // ─── POST: api/productoproveedor ───
        [HttpPost]
        public async Task<ActionResult<ProductoProveedorReadDto>> Create([FromBody] ProductoProveedorCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var validProd = await _db.Productos.AnyAsync(p => p.IdProducto == dto.IdProducto);
            var validProv = await _db.Proveedores.AnyAsync(p => p.IdProveedor == dto.IdProveedor);
            if (!validProd || !validProv)
                return BadRequest(new { Message = "Producto o Proveedor inválido." });

            var entidad = new ProductoProveedor
            {
                IdProducto = dto.IdProducto,
                IdProveedor = dto.IdProveedor,
                FechaCompra = dto.FechaCompra,
                Cantidad = dto.Cantidad
            };

            _db.ProductoProveedor.Add(entidad);
            await _db.SaveChangesAsync();

            await _db.Entry(entidad).Reference(pp => pp.Producto).LoadAsync();
            await _db.Entry(entidad).Reference(pp => pp.Proveedor).LoadAsync();

            var readDto = new ProductoProveedorReadDto
            {
                IdProductoProveedor = entidad.IdProductoProveedor,
                IdProducto = entidad.IdProducto,
                NombreProducto = entidad.Producto.Nombre,
                IdProveedor = entidad.IdProveedor,
                NombreProveedor = entidad.Proveedor.Nombre,
                FechaCompra = entidad.FechaCompra,
                Cantidad = entidad.Cantidad
            };

            return CreatedAtAction(nameof(GetAll), new { id = entidad.IdProductoProveedor }, readDto);
        }

        // ─── DELETE: api/productoproveedor/{id} ───
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ent = await _db.ProductoProveedor.FindAsync(id);
            if (ent == null)
                return NotFound(new { Message = $"ProductoProveedor {id} no existe." });

            _db.ProductoProveedor.Remove(ent);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
