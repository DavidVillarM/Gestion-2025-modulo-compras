using System;
using System.Collections.Generic;

namespace BackendApp.Models;

public partial class ProductoProveedor
{
    public long IdProductoProveedor { get; set; }

    public long? IdProveedor { get; set; }

    public long? IdProducto { get; set; }

    public DateOnly? FechaCompra { get; set; }

    public int? Cantidad { get; set; }

    public virtual Producto? IdProductoNavigation { get; set; }

    public virtual Proveedore? IdProveedorNavigation { get; set; }
}
