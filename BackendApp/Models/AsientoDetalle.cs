using System;
using System.Collections.Generic;

namespace BackendApp.Models;

public partial class AsientoDetalle
{
    public long IdDetalle { get; set; }

    public long IdAsiento { get; set; }

    public string? CuentaContable { get; set; }

    public decimal? Debe { get; set; }

    public decimal? Haber { get; set; }

    public virtual Asiento IdAsientoNavigation { get; set; } = null!;
}
