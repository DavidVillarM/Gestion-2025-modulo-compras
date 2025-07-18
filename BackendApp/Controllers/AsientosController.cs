using BackendApp.Models;
using BackendApp.Data;  // Ajusta el namespace a donde esté tu DbContext
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AsientosController : ControllerBase
    {
        private readonly PostgresContext _context;

        public AsientosController(PostgresContext context)
        {
            _context = context;
        }

        // GET: api/Asientos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AsientoDto>>> GetAsientos()
        {
            // Generar automáticamente los asientos faltantes
            await GenerarAsientosDeFacturasAsync();
            await GenerarAsientosDeNotasCreditoAsync();

            // Recuperar todos los asientos con sus detalles y datos de factura/proveedor
            var asientos = await _context.Asientos
                .Include(a => a.IdProveedorNavigation)
                .Include(a => a.AsientoDetalles)
                .Include(a => a.IdFacturaNavigation)
                .Include(a => a.IdNotaNavigation)
                .ToListAsync();

            // Mapear a DTO
            var result = asientos.Select(a => new AsientoDto
            {
                IdAsiento = a.IdAsiento,
                Fecha = a.Fecha,
                MontoTotal = a.MontoTotal,
                Proveedor = a.IdProveedorNavigation.Nombre,
                IdFactura = a.IdFactura,
                IdNota = a.IdNota,
                Detalles = a.AsientoDetalles.Select(d => new AsientoDetalleDto
                {
                    CuentaContable = d.CuentaContable,
                    Debe = d.Debe,
                    Haber = d.Haber
                }).ToList()
            }).ToList();

            return Ok(result);
        }

        private async Task GenerarAsientosDeFacturasAsync()
        {
            var facturas = await _context.Facturas
                .Include(f => f.IdProveedorNavigation)
                .Include(f => f.FacturaDetalles)
                    .ThenInclude(fd => fd.IdProductoNavigation)
        // 1) No hemos generado aún asiento para esta factura …
        .Where(f => !_context.Asientos.Any(a => a.IdFactura == f.IdFactura))
        // 2) … y además NO exista ninguna nota de crédito para ella
        .Where(f => !_context.NotasCreditos.Any(nc => nc.IdFactura == f.IdFactura))
        .ToListAsync();

            foreach (var factura in facturas)
            {
                // Crear asiento principal
                var asiento = new Asiento
                {
                    IdProveedor = factura.IdProveedor,
                    IdFactura = factura.IdFactura,
                    IdNota = null,
                    Fecha = factura.Fecha ?? DateOnly.FromDateTime(DateTime.Now),
                    MontoTotal = factura.MontoTotal
                };
                _context.Asientos.Add(asiento);
                await _context.SaveChangesAsync();

                var detalles = new List<AsientoDetalle>();

                // Crear detalle por cada línea de factura
                foreach (var fd in factura.FacturaDetalles)
                {
                    detalles.Add(new AsientoDetalle
                    {
                        IdAsiento = asiento.IdAsiento,
                        CuentaContable = fd.IdProductoNavigation.Nombre,
                        Debe = (fd.Precio ?? 0m) * (fd.Cantidad ?? 0),
                        Haber = 0m
                    });
                }

                // Detalle IVA
                var ivaTotal = (factura.Iva5 ?? 0m) + (factura.Iva10 ?? 0m);
                if (ivaTotal > 0)
                {
                    detalles.Add(new AsientoDetalle
                    {
                        IdAsiento = asiento.IdAsiento,
                        CuentaContable = "IVA Crédito Fiscal",
                        Debe = ivaTotal,
                        Haber = 0m
                    });
                }

                // Detalle Proveedores (haber)
                detalles.Add(new AsientoDetalle
                {
                    IdAsiento = asiento.IdAsiento,
                    CuentaContable = "Proveedores",
                    Debe = 0m,
                    Haber = factura.MontoTotal
                });

                _context.AsientoDetalles.AddRange(detalles);
                await _context.SaveChangesAsync();
            }
        }

        private async Task GenerarAsientosDeNotasCreditoAsync()
        {
            var notas = await _context.NotasCreditos
                .Include(n => n.IdProveedorNavigation)
                .Include(n => n.NotaCreditoDetalles)
                    .ThenInclude(ncd => ncd.IdProductoNavigation)
                .Where(n => !_context.Asientos.Any(a => a.IdNota == n.IdNota))
                .ToListAsync();

            foreach (var nota in notas)
            {
                // Crear asiento principal
                var asiento = new Asiento
                {
                    IdProveedor = nota.IdProveedor,
                    IdFactura = null,
                    IdNota = nota.IdNota,
                    Fecha = nota.Fecha ?? DateOnly.FromDateTime(DateTime.Now),
                    MontoTotal = nota.MontoTotal
                };
                _context.Asientos.Add(asiento);
                await _context.SaveChangesAsync();

                var detalles = new List<AsientoDetalle>();

                // Crear detalle por cada línea de nota crédito
                foreach (var ncd in nota.NotaCreditoDetalles)
                {
                    detalles.Add(new AsientoDetalle
                    {
                        IdAsiento = asiento.IdAsiento,
                        CuentaContable = ncd.IdProductoNavigation.Nombre,
                        Debe = 0m,
                        Haber = (ncd.Precio ?? 0m) * (ncd.Cantidad ?? 0)
                    });
                }

                // Detalle IVA nota crédito
                var ivaTotal = (nota.Iva5 ?? 0m) + (nota.Iva10 ?? 0m);
                if (ivaTotal > 0)
                {
                    detalles.Add(new AsientoDetalle
                    {
                        IdAsiento = asiento.IdAsiento,
                        CuentaContable = "IVA Crédito Fiscal",
                        Debe = 0m,
                        Haber = ivaTotal
                    });
                }

                // Detalle Proveedores (debe)
                detalles.Add(new AsientoDetalle
                {
                    IdAsiento = asiento.IdAsiento,
                    CuentaContable = "Proveedores",
                    Debe = nota.MontoTotal,
                    Haber = 0m
                });

                _context.AsientoDetalles.AddRange(detalles);
                await _context.SaveChangesAsync();
            }
        }
    }

    // DTOs para respuesta
    public class AsientoDto
    {
        public long IdAsiento { get; set; }
        public DateOnly? Fecha { get; set; }
        public decimal? MontoTotal { get; set; }
        public string? Proveedor { get; set; }
        public long? IdFactura { get; set; }
        public long? IdNota { get; set; }
        public List<AsientoDetalleDto> Detalles { get; set; } = new();
    }

    public class AsientoDetalleDto
    {
        public string CuentaContable { get; set; } = string.Empty;
        public decimal? Debe { get; set; }
        public decimal? Haber { get; set; }
    }
}
