using System;
using System.Collections.Generic;

namespace BackendApp.Models;

public partial class Personal
{
    public long IdPersonal { get; set; }

    public long? IdUser { get; set; }

    public long? IdRol { get; set; }

    public string? Nombre { get; set; }

    public string? Apellido { get; set; }

    public string? Ci { get; set; }

    public DateOnly? FechaIngreso { get; set; }

    public DateOnly? FechaSalida { get; set; }

    public string? Estado { get; set; }

    public virtual ICollection<AjustesStock> AjustesStocks { get; set; } = new List<AjustesStock>();

    public virtual Role? IdRolNavigation { get; set; }

    public virtual Usuario? IdUserNavigation { get; set; }

    public virtual ICollection<ServiciosRealizado> ServiciosRealizados { get; set; } = new List<ServiciosRealizado>();

    public virtual ICollection<Stock> Stocks { get; set; } = new List<Stock>();
}
