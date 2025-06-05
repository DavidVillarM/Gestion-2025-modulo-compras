using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackendApp.Models;
using BackendApp.Dtos;

[ApiController]
[Route("api/[controller]")]
public class FacturasController : ControllerBase
{
    private readonly AppDbContext _context;

    public FacturasController(AppDbContext context)
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
    public async Task<IActionResult> CrearFactura(FacturaRequestDto dto)
    {
        var orden = await _context.OrdenesCompra
            .Include(o => o.Detalles)
            .FirstOrDefaultAsync(o => o.Id == dto.OrdenId);

        if (orden == null) return BadRequest("Orden de compra no encontrada");

        foreach (var item in dto.Items)
        {
            var detalleOrden = orden.Detalles
                .FirstOrDefault(d => d.ProductoId == item.ProductoId);

            if (detalleOrden == null || item.Cantidad > detalleOrden.CantidadSolicitada)
                return BadRequest("Producto inválido o cantidad excedida en la orden de compra");
        }

        var factura = new Factura
        {
            Fecha = DateOnly.FromDateTime(dto.FechaEmision),
            IdPedido = dto.OrdenId,
            IdProveedor = dto.ProveedorId,
            MontoTotal = dto.Items.Sum(i => i.Cantidad * i.PrecioUnitario),
            Subtotal = dto.Items.Sum(i => i.Cantidad * i.PrecioUnitario),
            FacturaDetalles = dto.Items.Select(i => new FacturaDetalle
            {
                IdProducto = i.ProductoId,
                Cantidad = i.Cantidad,
                Precio = i.PrecioUnitario,
                Iva5 = 0,
                Iva10 = 0
            }).ToList()
        };

        _context.Facturas.Add(factura);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetFactura), new { id = factura.IdFactura }, factura);
    }
}


