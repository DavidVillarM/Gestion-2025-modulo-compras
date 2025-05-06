using System;
using System.Collections.Generic;

namespace BackendApp.Models;

public partial class ServiciosProductosUtilizado
{
    public long IdServicioProducto { get; set; }

    public long? IdServicioRealizado { get; set; }

    public long? IdProducto { get; set; }

    public int? Cantidad { get; set; }

    public virtual Producto? IdProductoNavigation { get; set; }

    public virtual ServiciosRealizado? IdServicioRealizadoNavigation { get; set; }
}
