using System;
using System.Collections.Generic;

namespace BackendApp.Models;

public partial class AjustesStock
{
    public long IdAjuste { get; set; }

    public DateOnly? Fecha { get; set; }

    public string? Motivo { get; set; }

    public string? TipoAjuste { get; set; }

    public long? IdPersonal { get; set; }

    public virtual ICollection<DetalleAjusteStock> DetalleAjusteStocks { get; set; } = new List<DetalleAjusteStock>();

    public virtual Personal? IdPersonalNavigation { get; set; }
}
