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
            join pedido in _context.Pedidos
                .Include(p => p.PedidoDetalles)
                    .ThenInclude(d => d.IdProductoNavigation)
                .Include(p => p.IdProveedorNavigation) // Incluye proveedor
                on orden.IdOrden equals pedido.IdOrden
            where orden.Estado != "RECHAZADA"
            select new
            {
                orden.IdOrden,
                Estado = orden.Estado,
                FechaPedido = pedido.FechaPedido.HasValue ? pedido.FechaPedido.Value.ToString("yyyy-MM-dd") : null,
                Proveedor = new
                {
                    pedido.IdProveedorNavigation.Nombre,
                    pedido.IdProveedorNavigation.Ruc
                },
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


    [HttpGet]
    public async Task<IActionResult> GetRecepciones()
    {
        var recepciones = await _context.Recepcions
            .Include(r => r.IdPedidoNavigation)
                .ThenInclude(p => p.IdProveedorNavigation)
            .OrderByDescending(r => r.FechaRecepcion)
            .Select(r => new
            {
                r.Id,
                Fecha = r.FechaRecepcion.ToString("yyyy-MM-dd"),
                r.Estado,
                r.NumeroFactura,
                r.Timbrado,
                Proveedor = new
                {
                    Nombre = r.IdPedidoNavigation.IdProveedorNavigation.Nombre,
                    Ruc = r.IdPedidoNavigation.IdProveedorNavigation.Ruc
                }
            })
            .ToListAsync();

        return Ok(recepciones);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetRecepcionById(long id)
    {
        var recepcion = await _context.Recepcions
            .Include(r => r.IdPedidoNavigation)
                .ThenInclude(p => p.IdProveedorNavigation)
            .Include(r => r.RecepcionDetalles)
                .ThenInclude(d => d.IdProductoNavigation)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (recepcion == null)
            return NotFound("Recepción no encontrada");

        return Ok(new
        {
            recepcion.Id,
            Fecha = recepcion.FechaRecepcion.ToString("yyyy-MM-dd"),
            recepcion.Estado,
            recepcion.Timbrado,
            recepcion.NumeroFactura,
            Proveedor = new
            {
                recepcion.IdPedidoNavigation.IdProveedorNavigation.Nombre,
                recepcion.IdPedidoNavigation.IdProveedorNavigation.Ruc
            },
            Productos = recepcion.RecepcionDetalles.Select(d => new
            {
                d.IdProducto,
                d.IdProductoNavigation.Nombre,
                d.CantidadRecibida
            })
        });
    }

    [HttpGet("filtrar")]
    public async Task<IActionResult> FiltrarRecepciones(
    [FromQuery] string? proveedor,
    [FromQuery] string? estado,
    [FromQuery] DateTime? fechaDesde,
    [FromQuery] DateTime? fechaHasta)
    {
        var query = _context.Recepcions
            .Include(r => r.IdPedidoNavigation)
                .ThenInclude(p => p.IdProveedorNavigation)
            .AsQueryable();

        if (!string.IsNullOrEmpty(proveedor))
        {
            query = query.Where(r =>
                r.IdPedidoNavigation.IdProveedorNavigation.Nombre.ToLower().Contains(proveedor.ToLower()) ||
                r.IdPedidoNavigation.IdProveedorNavigation.Ruc.ToLower().Contains(proveedor.ToLower()));
        }

        if (!string.IsNullOrEmpty(estado))
        {
            query = query.Where(r => r.Estado.ToLower() == estado.ToLower());
        }

        if (fechaDesde.HasValue)
        {
            query = query.Where(r => r.FechaRecepcion >= fechaDesde.Value);
        }

        if (fechaHasta.HasValue)
        {
            query = query.Where(r => r.FechaRecepcion <= fechaHasta.Value);
        }

        var resultados = await query
            .OrderByDescending(r => r.FechaRecepcion)
            .Select(r => new
            {
                r.Id,
                Fecha = r.FechaRecepcion.ToString("yyyy-MM-dd"),
                r.Estado,
                r.NumeroFactura,
                r.Timbrado,
                Proveedor = new
                {
                    r.IdPedidoNavigation.IdProveedorNavigation.Nombre,
                    r.IdPedidoNavigation.IdProveedorNavigation.Ruc
                }
            })
            .ToListAsync();

        return Ok(resultados);
    }



    [HttpPost("registrar")]
    public async Task<IActionResult> RegistrarRecepcion([FromBody] RecepcionDTO dto)
    {
        var pedido = await _context.Pedidos
            .Include(p => p.PedidoDetalles)
            .Include(p => p.IdProveedorNavigation)
            .FirstOrDefaultAsync(p => p.IdOrden == dto.OrdenId);

        if (pedido == null)
            return NotFound("Pedido no encontrado");

        var orden = await _context.Ordenes.FirstOrDefaultAsync(o => o.IdOrden == dto.OrdenId);
        if (orden == null)
            return NotFound("Orden no encontrada");

        bool todoRecibido = true;

        // Registrar Recepción
        var nuevaRecepcion = new Recepcion
        {
            IdOrden = orden.IdOrden,
            IdPedido = pedido.IdPedido,
            Estado = "PENDIENTE",
            FechaRecepcion = DateTime.Now,
            Timbrado = dto.Timbrado,
            NumeroFactura = dto.NumeroFactura
        };

        _context.Set<Recepcion>().Add(nuevaRecepcion);
        await _context.SaveChangesAsync(); // Necesario para obtener el Id

        foreach (var recibido in dto.Productos)
        {
            var detalle = pedido.PedidoDetalles.FirstOrDefault(d => d.IdProducto == recibido.ProductoId);
            if (detalle == null) continue;

            if (recibido.CantidadRecibida > detalle.Cantidad)
                return BadRequest($"Cantidad mayor a la solicitada para el producto ID {recibido.ProductoId}");

            // Agregar al detalle de recepción
            _context.Set<RecepcionDetalle>().Add(new RecepcionDetalle
            {
                IdRecepcion = nuevaRecepcion.Id,
                IdProducto = recibido.ProductoId,
                CantidadRecibida = recibido.CantidadRecibida
            });

            // Aumentar stock
            var producto = await _context.Productos.FirstOrDefaultAsync(p => p.IdProducto == recibido.ProductoId);
            if (producto != null)
            {
                producto.CantidadTotal = (producto.CantidadTotal ?? 0) + recibido.CantidadRecibida;
            }

            if (detalle.Cantidad.HasValue && recibido.CantidadRecibida < detalle.Cantidad.Value)
            {
                todoRecibido = false;
            }
        }

        // Registrar factura
        _context.Facturas.Add(new Factura
        {
            IdPedido = pedido.IdPedido,
            IdProveedor = pedido.IdProveedor,
            Timbrado = dto.Timbrado,
            Fecha = DateOnly.FromDateTime(DateTime.Today),
            Estado = "RECIBIDA",
            NombreProveedor = pedido.IdProveedorNavigation.Nombre,
            Ruc = pedido.IdProveedorNavigation.Ruc
        });

        // Actualizar estado de la orden y recepción
        orden.Estado = todoRecibido ? "COMPLETA" : "INCOMPLETA";
        nuevaRecepcion.Estado = orden.Estado;

        await _context.SaveChangesAsync();

        return Ok("Recepción registrada correctamente");
    }


    [HttpGet("detalle-recepcion/{ordenId}")]
    public async Task<IActionResult> GetDetalleRecepcion(long ordenId)
    {
        var pedido = await _context.Pedidos
            .Include(p => p.IdProveedorNavigation)
            .Include(p => p.PedidoDetalles)
                .ThenInclude(d => d.IdProductoNavigation)
            .Include(p => p.Facturas)
            .FirstOrDefaultAsync(p => p.IdOrden == ordenId);

        if (pedido == null) return NotFound("Pedido no encontrado");

        return Ok(new
        {
            Proveedor = new
            {
                pedido.IdProveedorNavigation.Nombre,
                pedido.IdProveedorNavigation.Ruc
            },
            pedido.FechaPedido,
            Productos = pedido.PedidoDetalles.Select(d => new
            {
                d.IdProducto,
                d.IdProductoNavigation.Nombre,
                CantidadSolicitada = d.Cantidad
            }),
            Factura = pedido.Facturas.Select(f => new
            {
                f.Timbrado,
                f.Fecha,
                f.MontoTotal
            }).FirstOrDefault()
        });
    }

    [HttpGet("buscar")]
    public async Task<IActionResult> BuscarRecepciones(
    [FromQuery] string? proveedor,
    [FromQuery] string? estado,
    [FromQuery] DateTime? fechaDesde,
    [FromQuery] DateTime? fechaHasta)
    {
        var query = _context.Recepcions
            .Include(r => r.IdPedidoNavigation)
                .ThenInclude(p => p.IdProveedorNavigation)
            .Include(r => r.RecepcionDetalles)
                .ThenInclude(d => d.IdProductoNavigation)
            .AsQueryable();

        if (!string.IsNullOrEmpty(proveedor))
        {
            query = query.Where(r =>
                r.IdPedidoNavigation.IdProveedorNavigation.Nombre.ToLower().Contains(proveedor.ToLower()) ||
                r.IdPedidoNavigation.IdProveedorNavigation.Ruc.ToLower().Contains(proveedor.ToLower()));
        }

        if (!string.IsNullOrEmpty(estado))
        {
            query = query.Where(r => r.Estado.ToLower() == estado.ToLower());
        }

        if (fechaDesde.HasValue)
        {
            query = query.Where(r => r.FechaRecepcion >= fechaDesde.Value);
        }

        if (fechaHasta.HasValue)
        {
            query = query.Where(r => r.FechaRecepcion <= fechaHasta.Value);
        }

        var recepciones = await query
            .OrderByDescending(r => r.FechaRecepcion)
            .Select(r => new
            {
                r.Id,
                Fecha = r.FechaRecepcion.ToString("yyyy-MM-dd"),
                r.Estado,
                r.Timbrado,
                r.NumeroFactura,
                Proveedor = new
                {
                    r.IdPedidoNavigation.IdProveedorNavigation.Nombre,
                    r.IdPedidoNavigation.IdProveedorNavigation.Ruc
                },
                Productos = r.RecepcionDetalles.Select(d => new
                {
                    IdProducto = d.IdProducto,
                    Nombre = d.IdProductoNavigation.Nombre,
                    CantidadRecibida = d.CantidadRecibida
                })
            })
            .ToListAsync();

        return Ok(recepciones);
    }

    [HttpPut("{id}/confirmar")]
    public async Task<IActionResult> ConfirmarRecepcion(long id, [FromBody] ConfirmacionRecepcionDto dto)
    {
        var recepcion = await _context.Recepcions
            .Include(r => r.RecepcionDetalles)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (recepcion == null)
            return NotFound("Recepción no encontrada");

        var pedido = await _context.Pedidos
            .Include(p => p.PedidoDetalles)
            .FirstOrDefaultAsync(p => p.IdPedido == recepcion.IdPedido);

        if (pedido == null)
            return NotFound("Pedido relacionado no encontrado");

        foreach (var detalle in recepcion.RecepcionDetalles)
        {
            var nuevo = dto.Productos.FirstOrDefault(p => p.ProductoId == detalle.IdProducto);
            if (nuevo != null)
            {
                detalle.CantidadRecibida = nuevo.CantidadFinal;

                var producto = await _context.Productos.FirstOrDefaultAsync(p => p.IdProducto == detalle.IdProducto);
                if (producto != null)
                {
                    producto.CantidadTotal = (producto.CantidadTotal ?? 0) + nuevo.CantidadFinal;
                }
            }
        }

        bool completo = true;
        foreach (var detalle in pedido.PedidoDetalles)
        {
            var recibido = recepcion.RecepcionDetalles.FirstOrDefault(r => r.IdProducto == detalle.IdProducto);
            if (recibido == null || recibido.CantidadRecibida < (detalle.Cantidad ?? 0))
            {
                completo = false;
                break;
            }
        }

        recepcion.Estado = completo ? "COMPLETA" : "INCOMPLETA";

        await _context.SaveChangesAsync();
        return Ok("Recepción confirmada");
    }


    [HttpPost("{id}/rechazar")]
    public async Task<IActionResult> RechazarRecepcion(long id, [FromBody] RechazoRecepcionDTO dto)
    {
        var recepcion = await _context.Recepcions
            .Include(r => r.RecepcionDetalles)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (recepcion == null)
            return NotFound("Recepción no encontrada");

        recepcion.Estado = "RECHAZADA";

        foreach (var item in dto.Productos)
        {
            // Agregar nota de devolución
            var nota = new NotaDeDevolucion
            {
                PedidoId = (int)recepcion.IdPedido, // Asegurate de que esto esté bien mapeado
                ProductoId = (int)item.ProductoId,
                Motivo = item.Motivo,
                Fecha = DateTime.Now
            };
            _context.NotaDeDevolucions.Add(nota);

            // Opcional: actualizar cantidades si querés marcar rechazado en RecepcionDetalle
            var detalle = recepcion.RecepcionDetalles.FirstOrDefault(d => d.IdProducto == item.ProductoId);
            if (detalle != null)
            {
                detalle.CantidadRecibida = 0;
            }
        }

        await _context.SaveChangesAsync();
        return Ok("Recepción rechazada y nota de devolución generada");
    }

    public class ConfirmacionRecepcionDto
    {
        public List<ProductoConfirmadoDto> Productos { get; set; }
    }

    public class ProductoConfirmadoDto
    {
        public int ProductoId { get; set; }
        public int CantidadFinal { get; set; }
    }

    public class RechazoRecepcionDTO
    {
        public List<ItemRechazadoDTO> Productos { get; set; } = new();
    }

    public class ItemRechazadoDTO
    {
        public long ProductoId { get; set; }
        public int Cantidad { get; set; }
        public string Motivo { get; set; }
    }

    public class MotivoRechazoDTO
    {
        public int ProductoId { get; set; }
        public string Motivo { get; set; }
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
}