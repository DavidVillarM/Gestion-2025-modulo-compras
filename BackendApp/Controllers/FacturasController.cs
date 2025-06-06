using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackendApp.Models;
using BackendApp.Dtos;

[ApiController]
[Route("api/[controller]")]
public class FacturasController : ControllerBase
{
    private readonly PostgresContext _context;

    public FacturasController(PostgresContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetFacturas()
    {
        var facturas = await _context.Facturas
            .Include(f => f.FacturaDetalles)
            .ToListAsync();

        return Ok(facturas);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetFactura(long id)
    {
        var factura = await _context.Facturas
            .Include(f => f.FacturaDetalles)
            .FirstOrDefaultAsync(f => f.IdFactura == id);

        if (factura == null) return NotFound();
        return Ok(factura);
    }

    [HttpPost]
    public async Task<IActionResult> CrearFactura([FromBody] FacturaRequestDto dto)
    {
        var pedido = await _context.Pedidos
            .Include(p => p.PedidoDetalles)
            .FirstOrDefaultAsync(p => p.IdPedido == dto.OrdenId);

        if (pedido == null)
            return BadRequest("Pedido no encontrado");

        // Validar que los productos existan en el pedido y cantidades válidas
        foreach (var item in dto.Items)
        {
            var detalle = pedido.PedidoDetalles.FirstOrDefault(d => d.IdProducto == item.ProductoId);
            if (detalle == null || item.Cantidad > detalle.Cantidad)
                return BadRequest("Producto no válido o cantidad excedida");
        }

        var factura = new Factura
        {
            IdPedido = dto.OrdenId,
            IdProveedor = dto.ProveedorId,
            Fecha = DateOnly.FromDateTime(dto.FechaEmision),
            MontoTotal = dto.Items.Sum(i => i.Cantidad * i.PrecioUnitario),
            Subtotal = dto.Items.Sum(i => i.Cantidad * i.PrecioUnitario), // Ajustar si hay IVA
            Estado = "Registrada",
            FacturaDetalles = dto.Items.Select(i => new FacturaDetalle
            {
                IdProducto = i.ProductoId,
                Cantidad = i.Cantidad,
                Precio = i.PrecioUnitario
            }).ToList()
        };

        _context.Facturas.Add(factura);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetFactura), new { id = factura.IdFactura }, factura);
    }
}
