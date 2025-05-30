using System;
using System.Collections.Generic;

namespace BackendApp.Models;

public partial class PresupuestosProveedore
{
    public long IdPresupuesto { get; set; }

    public long OrdenId { get; set; }

    public long ProveedorId { get; set; }

    public DateOnly FechaEnvio { get; set; }

    public virtual Proveedore Proveedor { get; set; } = null!;
}
