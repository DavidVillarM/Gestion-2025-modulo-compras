using System;
using System.Collections.Generic;

namespace BackendApp.Models;

public partial class DetalleAjusteStock
{
    public long IdDetalle { get; set; }

    public long? IdAjuste { get; set; }

    public long? IdProducto { get; set; }

    public int? Cantidad { get; set; }

    public virtual AjustesStock? IdAjusteNavigation { get; set; }

    public virtual Producto? IdProductoNavigation { get; set; }
}
