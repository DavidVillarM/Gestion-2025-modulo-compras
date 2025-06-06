using System;
using System.Collections.Generic;

namespace BackendApp.Models;

public partial class PresupuestoDetalle
{
    public long IdPresupuestoDetalle { get; set; }

    public long IdPresupuesto { get; set; }

    public long IdProducto { get; set; }

    public int? Cantidad { get; set; }

    public decimal? Precio { get; set; }

    public decimal? Iva5 { get; set; }

    public decimal? Iva10 { get; set; }

    public virtual Presupuesto IdPresupuestoNavigation { get; set; } = null!;

    public virtual Producto IdProductoNavigation { get; set; } = null!;
}
