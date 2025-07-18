using System;
using System.Collections.Generic;

namespace BackendApp.Models;

public partial class Asiento
{
    public long IdAsiento { get; set; }

    public long IdProveedor { get; set; }

    public long? IdNota { get; set; }

    public decimal? MontoTotal { get; set; }

    public DateOnly? Fecha { get; set; }

    public long? IdFactura { get; set; }

    public virtual ICollection<AsientoDetalle> AsientoDetalles { get; set; } = new List<AsientoDetalle>();

    public virtual Factura? IdFacturaNavigation { get; set; }

    public virtual NotasCredito? IdNotaNavigation { get; set; }

    public virtual Proveedore IdProveedorNavigation { get; set; } = null!;
}
