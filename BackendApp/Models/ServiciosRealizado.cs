using System;
using System.Collections.Generic;

namespace BackendApp.Models;

public partial class ServiciosRealizado
{
    public long IdServicioRealizado { get; set; }

    public long? IdPersonal { get; set; }

    public long? IdServicio { get; set; }

    public DateOnly? Fecha { get; set; }

    public string? Estado { get; set; }

    public virtual Personal? IdPersonalNavigation { get; set; }

    public virtual Servicio? IdServicioNavigation { get; set; }

    public virtual ICollection<ServiciosProductosUtilizado> ServiciosProductosUtilizados { get; set; } = new List<ServiciosProductosUtilizado>();
}
