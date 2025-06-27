using BackendApp.Dtos;
using BackendApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
//[Authorize]
[ApiController]
public class NotaCreditoController : ControllerBase
{
    private readonly PostgresContext _context;

    public NotaCreditoController(PostgresContext context)
    {
        _context = context;
    }

    // GET: api/<NotasreditoController>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<NotasCredito>>> GetNotas()
    {
        return await _context.NotasCreditos.ToListAsync();
    }

    // GET: api/NotaCredito/listado-simple
    [HttpGet("listado-simple")]
    public async Task<ActionResult<IEnumerable<object>>> GetNotasCreditoListadoSimple()
    {
        var notas = await _context.NotasCreditos
            .Include(nc => nc.IdProveedorNavigation)
            .Select(nc => new
            {
                nc.IdFactura,
                NombreProveedor = nc.IdProveedorNavigation != null ? nc.IdProveedorNavigation.Nombre : null,
                nc.Fecha,
                nc.Ruc,
                nc.MontoTotal
            })
            .ToListAsync();

        return Ok(notas);
    }


    // GET: api/NotaCredito/facturas-pendientes
    [HttpGet("facturas-pendientes")]
    public async Task<ActionResult<IEnumerable<Factura>>> GetFacturasPendientesOIncompletas()
    {
        var facturas = await _context.Facturas
            .Where(f => f.Estado == "PENDIENTE" || f.Estado == "INCOMPLETA")
            .Select(f => new
            {
                f.IdFactura,
                f.IdPedido,
                NombreProveedor = f.IdProveedorNavigation.Nombre,
                f.Fecha,
                f.Ruc,
                f.Timbrado,
                f.MontoTotal,
                f.Subtotal,
                f.Iva5,
                f.Iva10,
                f.Estado
            })
            .ToListAsync();

        return Ok(facturas);
    }

    [HttpGet("factura-detalles/{idFactura}")]
    public async Task<ActionResult<IEnumerable<object>>> GetFacturaDetallesPorFactura(long idFactura)
    {
        var detalles = await _context.FacturaDetalles
            .Where(fd => fd.IdFactura == idFactura)
            .Select(fd => new
            {
                fd.IdFacturaDetalle,
                fd.IdFactura,
                fd.IdProducto,
                NombreProducto = fd.IdProductoNavigation.Nombre,
                fd.Precio,
                fd.Cantidad,
                fd.Iva5,
                fd.Iva10
            })
            .ToListAsync();

        if (detalles == null || detalles.Count == 0)
        {
            return NotFound();
        }

        return Ok(detalles);
    }
    // GET api/<NotasreditoController>/5
    [HttpGet("{id}")]
    public async Task<ActionResult<NotasCredito>> GetNota(long id)
    {
        var nota = await _context.NotasCreditos.FindAsync(id);
        if (nota == null)
        {
            return NotFound();
        }
        return nota;
    }


    // POST: api/NotaCredito/{idNota}/detalles
    [HttpPost("{idNota}/detalles")]
    public async Task<ActionResult> PostDetallesNotaCredito(long idNota, [FromBody] List<NotaDetalleDto> detalles)
    {
        // Validar existencia de la cabecera
        var nota = await _context.NotasCreditos.FindAsync(idNota);
        if (nota == null)
        {
            return NotFound($"No existe la Nota de Crédito");
        }

        // Transformar cada NotaDetalleDto a NotaCreditoDetalle
        var detallesEntities = detalles.Select(detalle => new NotaCreditoDetalle
        {
            IdNota = idNota,
            IdProducto = detalle.IdProducto,
            Precio = detalle.Precio,
            Cantidad = detalle.Cantidad,
            Iva5 = detalle.Iva5,
            Iva10 = detalle.Iva10
        }).ToList();

        await _context.NotaCreditoDetalles.AddRangeAsync(detallesEntities);
        await _context.SaveChangesAsync();

        var factura = await _context.Facturas.FindAsync(nota.IdFactura);
        if (factura != null)
        {
            factura.Estado = "COMPLETO";
            await _context.SaveChangesAsync();
        }

        // Calcular totales para la cabecera de la nota de crédito
        decimal montoTotal = detallesEntities.Sum(d => d.Precio ?? 0);
        decimal iva5 = detallesEntities.Sum(d => d.Iva5 ?? 0);
        decimal iva10 = detallesEntities.Sum(d => d.Iva10 ?? 0);
        string? ruc = factura?.Ruc;

        // Actualizar cabecera de la nota de crédito
        await ActualizarCabeceraNotaCreditoAsync(idNota, montoTotal, iva5, iva10, ruc ?? string.Empty);


        return Ok(detallesEntities);
    }


    // POST: api/NotaCredito/crear-cabecera/{idFactura}
    [HttpPost("crear-cabecera/{idFactura}")]
    public async Task<ActionResult<NotasCredito>> CrearCabeceraDesdeFactura(long idFactura)
    {
        // Buscar la factura
        var factura = await _context.Facturas.FindAsync(idFactura);
        if (factura == null)
        {
            return NotFound($"No existe una Factura con IdFactura={idFactura}");
        }

        // Crear la cabecera de Nota de Crédito con los datos requeridos
        var cabecera = new NotasCredito
        {
            IdFactura = factura.IdFactura,
            IdProveedor = factura.IdProveedor,
            NombreProveedor = factura.NombreProveedor,
            Timbrado = factura.Timbrado,
            Estado = "Pendiente"
        };

        await _context.NotasCreditos.AddAsync(cabecera);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetNota), new { id = cabecera.IdNota }, cabecera);
    }


    // PUT api/<NotasreditoController>/5
    [HttpPut("{id}")]
    public async Task<ActionResult<NotasCredito>> Put(int id, [FromBody] NotasCredito value)
    {
        var edit = await _context.NotasCreditos.FindAsync(id);
        if (edit == null)
        {
            return NotFound();
        }
        edit.IdFactura = value.IdFactura;
        edit.IdProveedor = value.IdProveedor;
        edit.Fecha = value.Fecha;
        edit.Ruc = value.Ruc;
        edit.Timbrado = value.Timbrado;
        edit.NombreProveedor = value.NombreProveedor;
        edit.MontoTotal = value.MontoTotal;
        edit.Iva5 = value.Iva5;
        edit.Iva10 = value.Iva10;
        edit.Estado = value.Estado;

        await _context.SaveChangesAsync();
        return Ok(edit);
    }

    // DELETE api/<NotasreditoController>/5
    [HttpDelete("{id}")]
    public async Task<ActionResult<NotasCredito>> DeleteNota(long id)
    {


        var notaDelete = await _context.NotasCreditos.FindAsync(id);
        if (notaDelete == null)
        {
            return NotFound();
        }

        var detalle = await _context.NotaCreditoDetalles.Where(det => det.IdNota == id).ToListAsync();

        //eliminar todos los detalles y la cabecera
        _context.NotaCreditoDetalles.RemoveRange(detalle);
        _context.NotasCreditos.Remove(notaDelete);
        await _context.SaveChangesAsync();
        return Ok(notaDelete);

    }
    //se usa para actualizar la cabecera al guardar los detalles de la nota de crédito
    private async Task<bool> ActualizarCabeceraNotaCreditoAsync(long idNotaCredito, decimal montoTotal, decimal iva5, decimal iva10, string ruc)
    {
        var nota = await _context.NotasCreditos.FindAsync(idNotaCredito);
        if (nota == null)
            return false;

        nota.MontoTotal = montoTotal;
        nota.Iva5 = iva5;
        nota.Iva10 = iva10;
        nota.Ruc = ruc;
        nota.Fecha = DateOnly.FromDateTime(DateTime.Now);
        nota.Estado = "COMPLETO";

        await _context.SaveChangesAsync();
        return true;
    }
}