using System;
using System.Collections.Generic;

namespace BackendApp.Models;

public partial class ContadorFactura
{
    public int Id { get; set; }

    public long IdProveedor { get; set; }

    public int UltimoNumero { get; set; }

    public string? Prefijo { get; set; }

    public virtual Proveedore IdProveedorNavigation { get; set; } = null!;
}
