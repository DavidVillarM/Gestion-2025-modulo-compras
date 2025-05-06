using System;
using System.Collections.Generic;

namespace BackendApp.Models;

public partial class OrdenDetalle
{
    public long IdOrdenDetalle { get; set; }

    public long? IdProducto { get; set; }

    public long? IdOrden { get; set; }

    public decimal? Cotizacion { get; set; }

    public int? Cantidad { get; set; }

    public decimal? Iva { get; set; }

    public virtual Ordene? IdOrdenNavigation { get; set; }

    public virtual Producto? IdProductoNavigation { get; set; }
}
