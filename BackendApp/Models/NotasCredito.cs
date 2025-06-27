using System;
using System.Collections.Generic;

namespace BackendApp.Models;

public partial class NotasCredito
{
    public long IdNota { get; set; }

    public long IdFactura { get; set; }

    public long IdProveedor { get; set; }

    public DateOnly? Fecha { get; set; }

    public string? Ruc { get; set; }

    public string? NombreProveedor { get; set; }

    public string? Timbrado { get; set; }

    public decimal? MontoTotal { get; set; }

    public decimal? Subtotal { get; set; }

    public decimal? Iva5 { get; set; }

    public decimal? Iva10 { get; set; }

    public string? Estado { get; set; }

    public virtual ICollection<Asiento> Asientos { get; set; } = new List<Asiento>();

    public virtual Factura IdFacturaNavigation { get; set; } = null!;

    public virtual Proveedore IdProveedorNavigation { get; set; } = null!;

    public virtual ICollection<NotaCreditoDetalle> NotaCreditoDetalles { get; set; } = new List<NotaCreditoDetalle>();
}
