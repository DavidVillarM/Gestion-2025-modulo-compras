using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackendApp.Models;
using System.Linq;
using System.Threading.Tasks;

namespace BackendApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecepcionesController : ControllerBase
    {
        private readonly PostgresContext _context;

        public RecepcionesController(PostgresContext context)
        {
            _context = context;
        }

        [HttpGet("ordenes-pendientes")]
        public async Task<IActionResult> GetOrdenesPendientes()
        {
            var ordenes = await _context.Ordenes
                .Include(o => o.OrdenDetalles)
                    .ThenInclude(d => d.IdProductoNavigation)
                .Where(o => o.Estado != "Rechazada")
                .ToListAsync();

            return Ok(ordenes);
        }

        [HttpPost("registrar")]
        public async Task<IActionResult> RegistrarRecepcion([FromBody] RecepcionDTO dto)
        {
            var orden = await _context.Ordenes
                .Include(o => o.OrdenDetalles)
                .FirstOrDefaultAsync(o => o.IdOrden == dto.OrdenId);

            if (orden == null)
                return NotFound("Orden no encontrada");

            // Crear cabecera de Nota de Crédito (si aplica devoluciones)
            NotasCredito? notaCredito = null;
            bool huboDevolucion = false;

            foreach (var recibido in dto.Productos)
            {
                var detalle = orden.OrdenDetalles.FirstOrDefault(d => d.IdProducto == recibido.ProductoId);
                if (detalle == null) continue;

                if (recibido.CantidadRecibida > detalle.Cantidad)
                    return BadRequest($"Cantidad mayor a la solicitada para el producto ID {recibido.ProductoId}");

                var producto = await _context.Productos.FirstOrDefaultAsync(p => p.IdProducto == recibido.ProductoId);
                if (producto != null && producto.CantidadTotal.HasValue)
                {
                    producto.CantidadTotal += recibido.CantidadRecibida;
                }

                // Si hay devolución parcial (menos de lo pedido)
                if (detalle.Cantidad.HasValue && recibido.CantidadRecibida < detalle.Cantidad.Value)
                {
                    if (notaCredito == null)
                    {
                        notaCredito = new NotasCredito
                        {
                            IdPedido = orden.IdOrden,
                            Fecha = DateOnly.FromDateTime(DateTime.Today),
                            Ruc = dto.Ruc,
                            NombreProveedor = dto.NombreProveedor,
                            Timbrado = dto.Timbrado,
                            Estado = "Generada"
                        };
                        _context.NotasCreditos.Add(notaCredito);
                        await _context.SaveChangesAsync(); // Obtener IdFactura
                    }

                    _context.NotaCreditoDetalles.Add(new NotaCreditoDetalle
                    {
                        IdFacturaDetalle = notaCredito.IdFactura,
                        IdProducto = recibido.ProductoId,
                        Cantidad = detalle.Cantidad.Value - recibido.CantidadRecibida,
                        Precio = detalle.Cotizacion ?? 0,
                        Iva10 = detalle.Iva ?? 0
                    });

                    huboDevolucion = true;
                }
            }

            _context.Facturas.Add(new Factura
            {
                IdPedido = orden.IdOrden,
                Timbrado = dto.Timbrado,
                Fecha = DateOnly.FromDateTime(DateTime.Today),
                Ruc = dto.Ruc,
                NombreProveedor = dto.NombreProveedor
            });

            await _context.SaveChangesAsync();

            string mensaje = "Recepción registrada correctamente";
            if (huboDevolucion)
                mensaje += " con devolución registrada";

            return Ok(mensaje);
        }

        [HttpPost("rechazar")]
        public async Task<IActionResult> RechazarRecepcion([FromBody] RechazoRecepcionDTO dto)
        {
            var orden = await _context.Ordenes.FirstOrDefaultAsync(o => o.IdOrden == dto.OrdenId);

            if (orden == null)
                return NotFound("Orden no encontrada");

            orden.Estado = "Rechazada";
            //orden.MotivoRechazo = dto.Motivo;

            await _context.SaveChangesAsync();
            return Ok("Orden rechazada correctamente");
        }
    }

    public class RecepcionDTO
    {
        public long OrdenId { get; set; }
        public string NumeroFactura { get; set; }
        public string Timbrado { get; set; }
        public string Ruc { get; set; }
        public string NombreProveedor { get; set; }
        public List<ProductoRecibidoDTO> Productos { get; set; }
    }

    public class ProductoRecibidoDTO
    {
        public long ProductoId { get; set; }
        public int CantidadRecibida { get; set; }
        public string MotivoDevolucion { get; set; }
    }

    public class RechazoRecepcionDTO
    {
        public long OrdenId { get; set; }
        public string Motivo { get; set; }
    }
}
