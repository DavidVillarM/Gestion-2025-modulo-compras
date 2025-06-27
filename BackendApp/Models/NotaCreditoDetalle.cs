using System;
using System.Collections.Generic;

namespace BackendApp.Models;

public partial class NotaCreditoDetalle
{
    public long IdNotaDetalle { get; set; }

    public long IdNota { get; set; }

    public long IdProducto { get; set; }

    public decimal? Precio { get; set; }

    public int? Cantidad { get; set; }

    public decimal? Iva5 { get; set; }

    public decimal? Iva10 { get; set; }

    public string? Motivo { get; set; }

    public virtual NotasCredito IdNotaNavigation { get; set; } = null!;

    public virtual Producto IdProductoNavigation { get; set; } = null!;
}
