using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackendApp.Models; // Ajustalo a tu namespace real
using System.Linq;
using System.Threading.Tasks;

namespace BackendApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecepcionesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RecepcionesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("ordenes-pendientes")]
        public async Task<IActionResult> GetOrdenesPendientes()
        {
            var ordenes = await _context.OrdenesCompra
                .Include(o => o.Proveedor)
                .Include(o => o.Detalles)
                    .ThenInclude(d => d.Producto)
                .Where(o => o.Detalles.Any(d => d.CantidadRecibida < d.CantidadSolicitada))
                .ToListAsync();

            return Ok(ordenes);
        }

        [HttpPost("registrar")]
        public async Task<IActionResult> RegistrarRecepcion([FromBody] RecepcionDTO dto)
        {
            var orden = await _context.OrdenesCompra
                .Include(o => o.Detalles)
                .FirstOrDefaultAsync(o => o.Id == dto.OrdenId);

            if (orden == null)
                return NotFound("Orden no encontrada");

            foreach (var recibido in dto.Productos)
            {
                var detalle = orden.Detalles.FirstOrDefault(d => d.ProductoId == recibido.ProductoId);
                if (detalle == null) continue;

                int restante = detalle.CantidadSolicitada - detalle.CantidadRecibida;
                if (recibido.CantidadRecibida > restante)
                    return BadRequest("Cantidad mayor a la solicitada");

                detalle.CantidadRecibida += recibido.CantidadRecibida;

                var stock = await _context.Stock.FirstOrDefaultAsync(s => s.ProductoId == recibido.ProductoId);
                if (stock != null)
                {
                    stock.Cantidad += recibido.CantidadRecibida;
                }

                if (!string.IsNullOrWhiteSpace(recibido.MotivoDevolucion))
                {
                    _context.NotasDeDevolucion.Add(new NotaDeDevolucion
                    {
                        ProductoId = recibido.ProductoId,
                        OrdenId = orden.Id,
                        Motivo = recibido.MotivoDevolucion,
                        Fecha = DateTime.Now
                    });
                }
            }

            _context.Facturas.Add(new Factura
            {
                OrdenCompraId = dto.OrdenId,
                Numero = dto.NumeroFactura,
                Timbrado = dto.Timbrado,
                Fecha = DateTime.Now
            });

            await _context.SaveChangesAsync();
            return Ok("Recepción registrada");
        }
    }

    public class RecepcionDTO
    {
        public int OrdenId { get; set; }
        public string NumeroFactura { get; set; }
        public string Timbrado { get; set; }
        public List<ProductoRecibidoDTO> Productos { get; set; }
    }

    public class ProductoRecibidoDTO
    {
        public int ProductoId { get; set; }
        public int CantidadRecibida { get; set; }
        public string MotivoDevolucion { get; set; }
    }
}
