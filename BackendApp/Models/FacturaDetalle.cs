﻿using System;
using System.Collections.Generic;

namespace BackendApp.Models;

public partial class FacturaDetalle
{
    public long IdFacturaDetalle { get; set; }

    public long? IdProducto { get; set; }

    public decimal? Precio { get; set; }

    public int? Cantidad { get; set; }

    public decimal? Iva5 { get; set; }

    public decimal? Iva10 { get; set; }

    public virtual Producto? IdProductoNavigation { get; set; }
}
