using System;
using System.Collections.Generic;

namespace BackendApp.Models;

public partial class CategoriaProveedor
{
    public long IdCategoriaProveedor { get; set; }

    public long? IdCategoria { get; set; }

    public long? IdProveedor { get; set; }

    public string? Estado { get; set; }

    public virtual Categorium? IdCategoriaNavigation { get; set; }

    public virtual Proveedore? IdProveedorNavigation { get; set; }
}
