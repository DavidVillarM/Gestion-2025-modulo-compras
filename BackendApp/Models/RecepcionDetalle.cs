using System;
using System.Collections.Generic;

namespace BackendApp.Models;

public partial class RecepcionDetalle
{
    public long Id { get; set; }

    public long IdRecepcion { get; set; }

    public long IdProducto { get; set; }

    public int CantidadRecibida { get; set; }

    public virtual Producto IdProductoNavigation { get; set; } = null!;

    public virtual Recepcion IdRecepcionNavigation { get; set; } = null!;
}
