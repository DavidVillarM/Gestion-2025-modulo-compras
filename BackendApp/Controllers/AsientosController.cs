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
            // 1) Generar automáticamente los asientos faltantes
            await GenerarAsientosDeFacturasAsync();
            await GenerarAsientosDeNotasCreditoAsync();

            // 2) Recuperar todos los asientos con sus detalles y datos de factura/proveedor
            var asientos = await _context.Asientos
                .Include(a => a.IdProveedorNavigation)
                .Include(a => a.AsientoDetalles)
                .Include(a => a.IdFacturaNavigation)
                .ToListAsync();

            // Mapear a DTO para exponer solo la info necesaria
            var result = asientos.Select(a => new AsientoDto
            {
                IdAsiento = a.IdAsiento,
                Fecha = a.Fecha,
                MontoTotal = a.MontoTotal,
                Proveedor = a.IdProveedorNavigation.Nombre,
                IdFactura = a.IdFactura,
                FacturaNro = a.IdFacturaNavigation.NroFactura,
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
            // Obtener facturas sin asiento
            var facturas = await _context.Facturas
                .Include(f => f.IdProveedorNavigation)
                .Include(f => f.FacturaDetalles)
                .Where(f => !_context.Asientos.Any(a => a.IdFactura == f.IdFactura))
                .ToListAsync();

            foreach (var factura in facturas)
            {
                var subtotal = factura.Subtotal ?? 0m;
                var iva5 = factura.Iva5 ?? 0m;
                var iva10 = factura.Iva10 ?? 0m;
                var totalIva = iva5 + iva10;
                var total = factura.MontoTotal ?? (subtotal + totalIva);

                var asiento = new Asiento
                {
                    IdProveedor = factura.IdProveedor,
                    IdFactura = factura.IdFactura,
                    IdNota = null,
                    Fecha = factura.Fecha ?? DateOnly.FromDateTime(DateTime.Now),
                    MontoTotal = total
                };
                _context.Asientos.Add(asiento);
                await _context.SaveChangesAsync();

                _context.AsientoDetalles.AddRange(
                    new AsientoDetalle { IdAsiento = asiento.IdAsiento, CuentaContable = "Compras", Debe = subtotal, Haber = 0m },
                    new AsientoDetalle { IdAsiento = asiento.IdAsiento, CuentaContable = "IVA Crédito Fiscal", Debe = totalIva, Haber = 0m },
                    new AsientoDetalle { IdAsiento = asiento.IdAsiento, CuentaContable = "Proveedores", Debe = 0m, Haber = total }
                );

                await _context.SaveChangesAsync();
            }
        }

        private async Task GenerarAsientosDeNotasCreditoAsync()
        {
            var notas = await _context.NotasCreditos
                .Include(n => n.IdProveedorNavigation)
                .Include(n => n.NotaCreditoDetalles)
                .Where(n => !_context.Asientos.Any(a => a.IdNota == n.IdNota))
                .ToListAsync();

            foreach (var nota in notas)
            {
                var subtotal = nota.Subtotal ?? 0m;
                var iva5 = nota.Iva5 ?? 0m;
                var iva10 = nota.Iva10 ?? 0m;
                var totalIva = iva5 + iva10;
                var total = nota.MontoTotal ?? (subtotal + totalIva);

                var asiento = new Asiento
                {
                    IdProveedor = nota.IdProveedor,
                    IdFactura = null,
                    IdNota = nota.IdNota,
                    Fecha = nota.Fecha ?? DateOnly.FromDateTime(DateTime.Now),
                    MontoTotal = total
                };
                _context.Asientos.Add(asiento);
                await _context.SaveChangesAsync();

                _context.AsientoDetalles.AddRange(
                    new AsientoDetalle { IdAsiento = asiento.IdAsiento, CuentaContable = "Proveedores", Debe = total, Haber = 0m },
                    new AsientoDetalle { IdAsiento = asiento.IdAsiento, CuentaContable = "Compras", Debe = 0m, Haber = subtotal },
                    new AsientoDetalle { IdAsiento = asiento.IdAsiento, CuentaContable = "IVA Crédito Fiscal", Debe = 0m, Haber = totalIva }
                );

                await _context.SaveChangesAsync();
            }
        }
    }

    // DTOs para la respuesta
    public class AsientoDto
    {
        public long IdAsiento { get; set; }
        public DateOnly? Fecha { get; set; }
        public decimal? MontoTotal { get; set; }
        public string? Proveedor { get; set; }

        // <-- Agrego IdFactura
        public long? IdFactura { get; set; }

        public string? FacturaNro { get; set; }
        public List<AsientoDetalleDto> Detalles { get; set; } = new();
    }

    public class AsientoDetalleDto
    {
        public string CuentaContable { get; set; } = string.Empty;
        public decimal? Debe { get; set; }
        public decimal? Haber { get; set; }
    }
}
