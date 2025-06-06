using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackendApp.Data;
using BackendApp.DTOs;
using BackendApp.Modelss;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BackendApp.Controllers
{
    [ApiController]
    [Route("api/ordenes/{ordenId}/[controller]")]
    public class PresupuestoController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public PresupuestoController(ApplicationDbContext db) => _db = db;

        // ─── GET /api/ordenes/{ordenId}/presupuestos ───
        [HttpGet]
        public async Task<IActionResult> GetAll(int ordenId)
        {
            var ordenExiste = await _db.Ordenes.AnyAsync(o => o.IdOrden == ordenId);
            if (!ordenExiste)
                return NotFound(new { Message = $"La orden {ordenId} no existe." });

            var presupuestos = await _db.Presupuestos
                .Where(p => p.IdOrden == ordenId)
                .Include(p => p.Proveedor)
                .Include(p => p.Detalles)
                    .ThenInclude(d => d.Producto)
                .ToListAsync();

            var result = presupuestos.Select(p => new PresupuestoReadDto
            {
                IdPresupuesto = p.IdPresupuesto,
                IdProveedor = p.IdProveedor,
                NombreProveedor = p.Proveedor.Nombre,
                FechaEntrega = p.FechaEntrega,
                Subtotal = p.Subtotal,
                Iva5 = p.Iva5,
                Iva10 = p.Iva10,
                Total = p.Total,
                Detalles = p.Detalles.Select(d => new PresupuestoDetalleReadDto
                {
                    IdPresupuestoDetalle = d.IdPresupuestoDetalle,
                    IdProducto = d.IdProducto,
                    Cantidad = d.Cantidad,
                    Precio = d.Precio,
                    Iva5 = d.Iva5,
                    Iva10 = d.Iva10,
                    NombreProducto = d.Producto.Nombre
                }).ToList()
            }).ToList();

            return Ok(result);
        }
        [HttpGet]
        [Route("/api/presupuestos")]
        public async Task<IActionResult> GetAllGlobal()
        {
            // Recuperar TODOS los presupuestos en la base de datos, junto con su orden, proveedor, detalles y producto.
            var presupuestos = await _db.Presupuestos
                .Include(p => p.Proveedor)
                .Include(p => p.Detalles)
                    .ThenInclude(d => d.Producto)
                .Include(p => p.Orden)  // Si quisieras mostrar datos de la orden
                .ToListAsync();

            var result = presupuestos.Select(p => new
            {
                p.IdPresupuesto,
                p.IdOrden,
                p.IdProveedor,
                NombreProveedor = p.Proveedor.Nombre,
                FechaEntrega = p.FechaEntrega,
                Subtotal = p.Subtotal,
                Iva5 = p.Iva5,
                Iva10 = p.Iva10,
                Total = p.Total,
                Detalles = p.Detalles.Select(d => new
                {
                    d.IdPresupuestoDetalle,
                    d.IdProducto,
                    NombreProducto = d.Producto.Nombre,
                    d.Cantidad,
                    d.Precio,
                    d.Iva5,
                    d.Iva10
                }).ToList()
            });

            return Ok(result);
        }
        // ─── GET /api/ordenes/{ordenId}/presupuestos/{idPresu} ───
        [HttpGet("{idPresu}")]
        public async Task<IActionResult> Get(int ordenId, int idPresu)
        {
            var presu = await _db.Presupuestos
                .Include(p => p.Proveedor)
                .Include(p => p.Detalles)
                    .ThenInclude(d => d.Producto)
                .FirstOrDefaultAsync(p => p.IdPresupuesto == idPresu && p.IdOrden == ordenId);

            if (presu == null)
                return NotFound(new { Message = $"El presupuesto {idPresu} para la orden {ordenId} no existe." });

            var dto = new PresupuestoReadDto
            {
                IdPresupuesto = presu.IdPresupuesto,
                IdProveedor = presu.IdProveedor,
                NombreProveedor = presu.Proveedor.Nombre,
                FechaEntrega = presu.FechaEntrega,
                Subtotal = presu.Subtotal,
                Iva5 = presu.Iva5,
                Iva10 = presu.Iva10,
                Total = presu.Total,
                Detalles = presu.Detalles.Select(d => new PresupuestoDetalleReadDto
                {
                    IdPresupuestoDetalle = d.IdPresupuestoDetalle,
                    IdProducto = d.IdProducto,
                    Cantidad = d.Cantidad,
                    Precio = d.Precio,
                    Iva5 = d.Iva5,
                    Iva10 = d.Iva10,
                    NombreProducto = d.Producto.Nombre
                }).ToList()
            };

            return Ok(dto);
        }

        // ─── POST /api/ordenes/{ordenId}/presupuestos ───
        [HttpPost]
        public async Task<IActionResult> Create(int ordenId, [FromBody] PresupuestoCreateDto dto)
        {
            var orden = await _db.Ordenes
                .Include(o => o.OrdenDetalles)
                    .ThenInclude(d => d.Producto)
                .FirstOrDefaultAsync(o => o.IdOrden == ordenId);
            if (orden == null)
                return NotFound(new { Message = $"La orden {ordenId} no existe." });

            var prov = await _db.Proveedores
                .Include(p => p.CategoriaProveedores)
                .FirstOrDefaultAsync(p => p.IdProveedor == dto.IdProveedor);
            if (prov == null)
                return NotFound(new { Message = $"El proveedor {dto.IdProveedor} no existe." });

            // Validar que el proveedor cubra al menos una categoría de la orden
            var categoriasOrden = orden.OrdenDetalles
                                        .Select(d => d.Producto.IdCategoria)
                                        .Distinct()
                                        .ToList();
            var categoriasProv = prov.CategoriaProveedores
                                     .Where(cp => cp.Estado == "ACTIVO")
                                     .Select(cp => cp.IdCategoria)
                                     .ToList();
            if (!categoriasOrden.Any(c => categoriasProv.Contains(c)))
                return BadRequest(new { Message = $"El proveedor {dto.IdProveedor} no vende ninguna de las categorías solicitadas." });

            // Validar productos en detalles
            var prodIds = dto.Detalles.Select(d => d.IdProducto).Distinct().ToList();
            var existentes = await _db.Productos
                .Where(p => prodIds.Contains(p.IdProducto))
                .Select(p => p.IdProducto)
                .ToListAsync();
            var faltantes = prodIds.Except(existentes).ToList();
            if (faltantes.Any())
                return BadRequest(new { Message = $"Productos no encontrados: {string.Join(", ", faltantes)}" });

            var presu = new Presupuesto
            {
                IdOrden = ordenId,
                IdProveedor = dto.IdProveedor,
                FechaEntrega = dto.FechaEntrega,
                Subtotal = dto.Subtotal,
                Iva5 = dto.Iva5,
                Iva10 = dto.Iva10,
                Total = dto.Total
            };

            foreach (var d in dto.Detalles)
            {
                presu.Detalles.Add(new PresupuestoDetalle
                {
                    IdProducto = d.IdProducto,
                    Cantidad = d.Cantidad,
                    Precio = d.Precio,
                    Iva5 = d.Iva5,
                    Iva10 = d.Iva10
                });
            }

            _db.Presupuestos.Add(presu);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAll), new { ordenId = ordenId }, new { presu.IdPresupuesto });
        }

        // ─── PUT /api/ordenes/{ordenId}/presupuestos/{idPresu} ───
        [HttpPut("{idPresu}")]
        public async Task<IActionResult> Update(int ordenId, int idPresu, [FromBody] PresupuestoCreateDto dto)
        {
            var presu = await _db.Presupuestos
                .Include(p => p.Detalles)
                .FirstOrDefaultAsync(p => p.IdPresupuesto == idPresu && p.IdOrden == ordenId);

            if (presu == null)
                return NotFound(new { Message = $"El presupuesto {idPresu} para la orden {ordenId} no existe." });

            var prov = await _db.Proveedores
                .Include(p => p.CategoriaProveedores)
                .FirstOrDefaultAsync(p => p.IdProveedor == dto.IdProveedor);
            if (prov == null)
                return NotFound(new { Message = $"El proveedor {dto.IdProveedor} no existe." });

            var orden = await _db.Ordenes
                .Include(o => o.OrdenDetalles)
                    .ThenInclude(d => d.Producto)
                .FirstOrDefaultAsync(o => o.IdOrden == ordenId);
            var categoriasOrden = orden.OrdenDetalles
                                        .Select(d => d.Producto.IdCategoria)
                                        .Distinct()
                                        .ToList();
            var categoriasProv = prov.CategoriaProveedores
                                     .Where(cp => cp.Estado == "ACTIVO")
                                     .Select(cp => cp.IdCategoria)
                                     .ToList();
            if (!categoriasOrden.Any(c => categoriasProv.Contains(c)))
                return BadRequest(new { Message = $"El proveedor {dto.IdProveedor} no vende ninguna de las categorías solicitadas." });

            var prodIds = dto.Detalles.Select(d => d.IdProducto).Distinct().ToList();
            var existentes = await _db.Productos
                .Where(p => prodIds.Contains(p.IdProducto))
                .Select(p => p.IdProducto)
                .ToListAsync();
            var faltantes = prodIds.Except(existentes).ToList();
            if (faltantes.Any())
                return BadRequest(new { Message = $"Productos no encontrados: {string.Join(", ", faltantes)}" });

            // Actualizar encabezado
            presu.IdProveedor = dto.IdProveedor;
            presu.FechaEntrega = dto.FechaEntrega;
            presu.Subtotal = dto.Subtotal;
            presu.Iva5 = dto.Iva5;
            presu.Iva10 = dto.Iva10;
            presu.Total = dto.Total;

            // Reemplazar detalles
            _db.PresupuestoDetalles.RemoveRange(presu.Detalles);
            presu.Detalles.Clear();

            foreach (var d in dto.Detalles)
            {
                presu.Detalles.Add(new PresupuestoDetalle
                {
                    IdProducto = d.IdProducto,
                    Cantidad = d.Cantidad,
                    Precio = d.Precio,
                    Iva5 = d.Iva5,
                    Iva10 = d.Iva10
                });
            }

            await _db.SaveChangesAsync();
            return NoContent();
        }

        // ─── DELETE /api/ordenes/{ordenId}/presupuestos/{idPresu} ───
        [HttpDelete("{idPresu}")]
        public async Task<IActionResult> Delete(int ordenId, int idPresu)
        {
            var presu = await _db.Presupuestos
                .FirstOrDefaultAsync(p => p.IdPresupuesto == idPresu && p.IdOrden == ordenId);

            if (presu == null)
                return NotFound(new { Message = $"El presupuesto {idPresu} para la orden {ordenId} no existe." });

            _db.Presupuestos.Remove(presu);
            await _db.SaveChangesAsync();
            return NoContent();
        }


    }
}
