using System;
using System.Collections.Generic;

namespace BackendApp.Models;

public partial class BajasProducto
{
    public long IdBajaProducto { get; set; }

    public long? IdProducto { get; set; }

    public int? Cantidad { get; set; }

    public string? Motivo { get; set; }

    public virtual Producto? IdProductoNavigation { get; set; }
}
