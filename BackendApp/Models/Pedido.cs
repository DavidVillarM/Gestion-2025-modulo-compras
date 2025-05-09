using System;
using System.Collections.Generic;

namespace BackendApp.Models;

public partial class Pedido
{
    public long IdPedido { get; set; }

    public long? IdOrden { get; set; }

    public long? IdProveedor { get; set; }

    public decimal? MontoTotal { get; set; }

    public DateOnly? FechaEntrega { get; set; }

    public DateOnly? FechaPedido { get; set; }

    public string? Estado { get; set; }

    public virtual ICollection<Factura> Facturas { get; set; } = new List<Factura>();

    public virtual Ordene? IdOrdenNavigation { get; set; }

    public virtual Proveedore? IdProveedorNavigation { get; set; }

    public virtual ICollection<NotasCredito> NotasCreditos { get; set; } = new List<NotasCredito>();
}
