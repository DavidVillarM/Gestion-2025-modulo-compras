using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModuloCompras.Data;
using ModuloCompras.DTOs;
using ModuloCompras.Models;
using System.Linq;
using System.Threading.Tasks;

namespace ModuloCompras.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdenesController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public OrdenesController(ApplicationDbContext db)
        {
            _db = db;
        }

        // ----------------------------------------------------------------------
        // 1) GET: /api/ordenes            → Obtener todas las órdenes
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _db.Ordenes
                .Include(o => o.OrdenDetalles)
                    .ThenInclude(d => d.Producto)
                .ToListAsync();

            // Mapear a DTO si quieres
            var dtos = list.Select(o => new OrdenReadDto
            {
                IdOrden = o.IdOrden,
                Fecha = o.Fecha,
                Estado = o.Estado,
                Detalles = o.OrdenDetalles.Select(d => new OrdenDetalleReadDto
                {
                    IdOrdenDetalle = d.IdOrdenDetalle,
                    IdProducto = d.IdProducto,
                    NombreProducto = d.Producto.Nombre,
                    Cantidad = d.Cantidad
                }).ToList()
            }).ToList();

            return Ok(dtos);
        }

        // ----------------------------------------------------------------------
        // 2) GET: /api/ordenes/{id}       → Obtener una orden por ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var orden = await _db.Ordenes
                .Include(o => o.OrdenDetalles)
                    .ThenInclude(d => d.Producto)
                .FirstOrDefaultAsync(o => o.IdOrden == id);

            if (orden == null)
                return NotFound(new { Message = $"La orden {id} no existe." });

            var dto = new OrdenReadDto
            {
                IdOrden = orden.IdOrden,
                Fecha = orden.Fecha,
                Estado = orden.Estado,
                Detalles = orden.OrdenDetalles.Select(d => new OrdenDetalleReadDto
                {
                    IdOrdenDetalle = d.IdOrdenDetalle,
                    IdProducto = d.IdProducto,
                    NombreProducto = d.Producto.Nombre,
                    Cantidad = d.Cantidad
                }).ToList()
            };

            return Ok(dto);
        }

        // ----------------------------------------------------------------------
        // 3) POST: /api/ordenes          → Crear nueva orden (estado INCOMPLETA)
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] OrdenCreateDto dto)
        {
            if (dto == null || dto.Detalles == null || !dto.Detalles.Any())
                return BadRequest(new { Message = "Debe incluir al menos un detalle de orden." });

            var orden = new Orden
            {
                Estado = dto.Estado, // debe ser “INCOMPLETA”
                Fecha = DateTime.UtcNow
            };

            foreach (var d in dto.Detalles)
            {
                // Opcionalmente: puedes validar que el producto exista
                var producto = await _db.Productos.FindAsync(d.IdProducto);
                if (producto == null)
                    return BadRequest(new { Message = $"El producto {d.IdProducto} no existe." });

                orden.OrdenDetalles.Add(new OrdenDetalle
                {
                    IdProducto = d.IdProducto,
                    Cantidad = d.Cantidad
                });
            }

            _db.Ordenes.Add(orden);
            await _db.SaveChangesAsync();

            // Retorna DTO de la orden creada:
            var resultado = new OrdenReadDto
            {
                IdOrden = orden.IdOrden,
                Fecha = orden.Fecha,
                Estado = orden.Estado,
                Detalles = orden.OrdenDetalles.Select(d => new OrdenDetalleReadDto
                {
                    IdOrdenDetalle = d.IdOrdenDetalle,
                    IdProducto = d.IdProducto,
                    NombreProducto = d.Producto.Nombre,
                    Cantidad = d.Cantidad
                }).ToList()
            };

            return CreatedAtAction(nameof(GetById), new { id = orden.IdOrden }, resultado);
        }

        // ----------------------------------------------------------------------
        // 4) PUT: /api/ordenes/{id}       → Actualizar una orden (opcional)
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] OrdenUpdateDto dto)
        {
            var orden = await _db.Ordenes.Include(o => o.OrdenDetalles)
                                         .FirstOrDefaultAsync(o => o.IdOrden == id);
            if (orden == null)
                return NotFound(new { Message = $"La orden {id} no existe." });

            // Por simplicidad aquí solo actualizamos el estado:
            orden.Estado = dto.Estado;
            await _db.SaveChangesAsync();

            return NoContent();
        }

        // ----------------------------------------------------------------------
        // 5) DELETE: /api/ordenes/{id}    → Eliminar orden (opcional)
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var orden = await _db.Ordenes.FindAsync(id);
            if (orden == null)
                return NotFound(new { Message = $"La orden {id} no existe." });

            _db.Ordenes.Remove(orden);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        // ----------------------------------------------------------------------
        // 6) GET: /api/ordenes/{id}/proveedores
        //    Devuelve la lista de proveedores válidos según las categorías de los productos
        //    NO cambia el estado de la orden.
        [HttpGet("{id}/proveedores")]
        public async Task<IActionResult> GetProveedores(int id)
        {
            var orden = await _db.Ordenes
                .Include(o => o.OrdenDetalles)
                    .ThenInclude(d => d.Producto)
                .FirstOrDefaultAsync(o => o.IdOrden == id);

            if (orden == null)
                return NotFound(new { Message = $"La orden {id} no existe." });

            // Tomamos todos los IdCategoria de los productos en esta orden
            var categoriaIds = orden.OrdenDetalles
                                    .Select(d => d.Producto.IdCategoria)
                                    .Distinct()
                                    .ToList();

            // Buscamos los proveedores que tengan una relación activa con esas categorías
            var proveedores = await _db.CategoriaProveedores
                .Where(cp => categoriaIds.Contains(cp.IdCategoria) && cp.Estado == "ACTIVO")
                .Select(cp => cp.Proveedor)
                .Distinct()
                .ToListAsync();

            var resultado = proveedores.Select(p => new ProveedorSimpleDto
            {
                IdProveedor = p.IdProveedor,
                Nombre = p.Nombre,
                Ruc = p.Ruc
            }).ToList();

            return Ok(resultado);
        }

        // ----------------------------------------------------------------------
        // 7) PUT: /api/ordenes/{id}/marcar-pendiente
        //    Cambia ESTADO de la orden de “INCOMPLETA” a “PENDIENTE”
        [HttpPut("{id}/marcar-pendiente")]
        public async Task<IActionResult> MarcarPendiente(int id)
        {
            var orden = await _db.Ordenes.FindAsync(id);
            if (orden == null)
                return NotFound(new { Message = $"La orden {id} no existe." });

            if (orden.Estado != "INCOMPLETA")
                return BadRequest(new { Message = "Solo se puede marcar como PENDIENTE una orden INCOMPLETA." });

            orden.Estado = "PENDIENTE";
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
