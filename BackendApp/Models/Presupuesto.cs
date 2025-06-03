using System;
using System.Collections.Generic;

namespace BackendApp.Models;

public partial class Presupuesto
{
    public long IdPresupuesto { get; set; }

    public long IdOrden { get; set; }

    public long IdProveedor { get; set; }

    public DateOnly? FechaEntrega { get; set; }

    public decimal? Subtotal { get; set; }

    public decimal? Iva5 { get; set; }

    public decimal? Iva10 { get; set; }

    public decimal? Total { get; set; }

    public virtual Proveedore IdProveedorNavigation { get; set; } = null!;

    public virtual ICollection<PresupuestoDetalle> PresupuestoDetalles { get; set; } = new List<PresupuestoDetalle>();
}
