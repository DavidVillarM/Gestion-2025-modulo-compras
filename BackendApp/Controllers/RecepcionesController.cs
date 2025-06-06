using BackendApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackendApp.Models;
using System.Linq;
using System.Threading.Tasks;

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
        var ordenes = await (
            from orden in _context.Ordenes
            join pedido in _context.Pedidos.Include(p => p.PedidoDetalles).ThenInclude(d => d.IdProductoNavigation)
                on orden.IdOrden equals pedido.IdOrden
            where orden.Estado != "Rechazada"
            select new
            {
                orden.IdOrden,
                Estado = orden.Estado,
                FechaPedido = pedido.FechaPedido.HasValue ? pedido.FechaPedido.Value.ToString("yyyy-MM-dd") : null,
                Productos = pedido.PedidoDetalles.Select(d => new
                {
                    Id = d.IdProducto,
                    Nombre = d.IdProductoNavigation.Nombre,
                    CantidadSolicitada = d.Cantidad
                })
            }
        ).ToListAsync();

        return Ok(ordenes);
    }

    [HttpPost("registrar")]
    public async Task<IActionResult> RegistrarRecepcion([FromBody] RecepcionDTO dto)
    {
        var pedido = await _context.Pedidos
            .Include(p => p.PedidoDetalles)
            .FirstOrDefaultAsync(p => p.IdOrden == dto.OrdenId);

        if (pedido == null)
            return NotFound("Pedido no encontrado");

        var orden = await _context.Ordenes.FirstOrDefaultAsync(o => o.IdOrden == dto.OrdenId);
        if (orden == null)
            return NotFound("Orden no encontrada");

        NotasCredito? notaCredito = null;
        bool huboDevolucion = false;
        bool todoRecibido = true;

        foreach (var recibido in dto.Productos)
        {
            var detalle = pedido.PedidoDetalles.FirstOrDefault(d => d.IdProducto == recibido.ProductoId);
            if (detalle == null) continue;

            if (recibido.CantidadRecibida > detalle.Cantidad)
                return BadRequest($"Cantidad mayor a la solicitada para el producto ID {recibido.ProductoId}");

            var producto = await _context.Productos.FirstOrDefaultAsync(p => p.IdProducto == recibido.ProductoId);
            if (producto != null && producto.CantidadTotal.HasValue)
            {
                producto.CantidadTotal += recibido.CantidadRecibida;
            }

            if (detalle.Cantidad.HasValue && recibido.CantidadRecibida < detalle.Cantidad.Value)
            {
                todoRecibido = false;

                if (notaCredito == null)
                {
                    notaCredito = new NotasCredito
                    {
                
                        Fecha = DateOnly.FromDateTime(DateTime.Today),
                        Timbrado = dto.Timbrado,
                        Estado = "Generada"
                    };
                    _context.NotasCreditos.Add(notaCredito);
                    await _context.SaveChangesAsync();
                }

                _context.NotaCreditoDetalles.Add(new NotaCreditoDetalle
                {
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
            IdPedido = pedido.IdPedido,
            Timbrado = dto.Timbrado,
            Fecha = DateOnly.FromDateTime(DateTime.Today),
        });

        orden.Estado = todoRecibido ? "Completa" : "Incompleta";

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

        await _context.SaveChangesAsync();
        return Ok("Orden rechazada correctamente");
    }

    public class RecepcionDTO
    {
        public long OrdenId { get; set; }
        public string NumeroFactura { get; set; }
        public string Timbrado { get; set; }
        public List<ProductoRecibidoDTO> Productos { get; set; }
    }

    public class ProductoRecibidoDTO
    {
        public long ProductoId { get; set; }
        public int CantidadRecibida { get; set; }
    }

    public class RechazoRecepcionDTO
    {
        public long OrdenId { get; set; }
    }
}
