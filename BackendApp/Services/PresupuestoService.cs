// File: Services/PresupuestoService.cs
using BackendApp.Models;
using BackendApp.Services.Dtos;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendApp.Services
{
    public class PresupuestoService
    {
        private readonly PostgresContext _context;

        public PresupuestoService(PostgresContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retorna todos los Presupuestos (cabecera + detalles) para la orden con Id = idOrden.
        /// </summary>
        public async Task<List<PresupuestoConDetallesDto>> GetPresupuestosByOrdenAsync(long idOrden)
        {
            // 1) Traer todos los Presupuestos que pertenecen a la orden, 
            //    incluyendo Proveedor (para nombre) y cada detalle con Producto.
            var presupuestosConNav = await _context.Presupuestos
                .AsNoTracking()
                .Where(p => p.IdOrden == idOrden)
                .Include(p => p.IdProveedorNavigation)                     // Para obtener nombre del proveedor
                .Include(p => p.PresupuestoDetalles)                       // Para obtener cada detalle
                    .ThenInclude(d => d.IdProductoNavigation)             // Para obtener nombre del producto
                .ToListAsync();

            // 2) Mapear cada entidad a nuestro DTO
            var resultado = presupuestosConNav.Select(presupuesto =>
            {
                var dto = new PresupuestoConDetallesDto
                {
                    IdPresupuesto = presupuesto.IdPresupuesto,
                    IdProveedor = presupuesto.IdProveedor,
                    Proveedor = presupuesto.IdProveedorNavigation?.Nombre ?? "",
                    FechaEntrega = presupuesto.FechaEntrega,
                    Subtotal = presupuesto.Subtotal ?? 0m,
                    Iva5 = presupuesto.Iva5 ?? 0m,
                    Iva10 = presupuesto.Iva10 ?? 0m,
                    Total = presupuesto.Total ?? 0m,
                    Detalles = presupuesto.PresupuestoDetalles.Select(det => new PresupuestoDetalleDto
                    {
                        IdPresupuestoDetalle = det.IdPresupuestoDetalle,
                        IdProducto = det.IdProducto,  // 👈 NUEVA LÍNEA
                        Producto = det.IdProductoNavigation?.Nombre ?? "",
                        Cantidad = det.Cantidad ?? 0,
                        Precio = det.Precio ?? 0m,
                        Iva5 = det.Iva5 ?? 0m,
                        Iva10 = det.Iva10 ?? 0m,
                        Subtotal = ((det.Precio ?? 0m) * (det.Cantidad ?? 0))
                     + (det.Iva5 ?? 0m) + (det.Iva10 ?? 0m)
                    }).ToList()
                };
                return dto;
            })
            .ToList();

            return resultado;
        }
    }
}
