using System;
using System.Collections.Generic;

namespace BackendApp.Models;

public partial class Precio
{
    public long IdPrecio { get; set; }

    public long? IdProducto { get; set; }

    public decimal? Precio1 { get; set; }

    public DateOnly? FechaRegistro { get; set; }

    public string? Estado { get; set; }

    public virtual Producto? IdProductoNavigation { get; set; }
}
