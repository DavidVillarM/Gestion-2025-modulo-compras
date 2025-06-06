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
    [HttpPut("{id}")]
    public async Task<IActionResult> ActualizarFactura(long id, [FromBody] FacturaUpdateDto dto)
    {
        var factura = await _context.Facturas
            .Include(f => f.FacturaDetalles)
            .FirstOrDefaultAsync(f => f.IdFactura == id);

        if (factura == null)
            return NotFound("Factura no encontrada");

        // Validación de productos
        foreach (var item in dto.FacturaDetalles)
        {
            var existe = await _context.Productos.AnyAsync(p => p.IdProducto == item.IdProducto);
            if (!existe)
                return BadRequest($"El producto con ID {item.IdProducto} no existe.");
        }

        // Actualizar campos principales
        factura.IdProveedor = dto.IdProveedor;
        factura.IdPedido = dto.IdPedido;
        factura.Fecha = dto.Fecha;
        factura.Ruc = dto.Ruc;
        factura.NombreProveedor = dto.NombreProveedor;
        factura.Timbrado = dto.Timbrado;
        factura.MontoTotal = dto.MontoTotal;
        factura.Subtotal = dto.Subtotal;
        factura.Iva5 = dto.Iva5;
        factura.Iva10 = dto.Iva10;
        factura.Estado = dto.Estado;

        // Reemplazar detalles
        _context.FacturaDetalles.RemoveRange(factura.FacturaDetalles);
        factura.FacturaDetalles = dto.FacturaDetalles.Select(fd => new FacturaDetalle
        {
            IdProducto = fd.IdProducto,
            Cantidad = fd.Cantidad,
            Precio = fd.Precio,
            Iva5 = fd.Iva5,
            Iva10 = fd.Iva10
        }).ToList();

        await _context.SaveChangesAsync();
        return Ok(factura);
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> EliminarFactura(long id)
    {
        var factura = await _context.Facturas
            .Include(f => f.FacturaDetalles)
            .FirstOrDefaultAsync(f => f.IdFactura == id);

        if (factura == null)
            return NotFound("Factura no encontrada");

        // Eliminar detalles primero
        _context.FacturaDetalles.RemoveRange(factura.FacturaDetalles);
        _context.Facturas.Remove(factura);

        await _context.SaveChangesAsync();
        return NoContent();
    }



}
