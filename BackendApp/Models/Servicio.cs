using System;
using System.Collections.Generic;

namespace BackendApp.Models;

public partial class Servicio
{
    public long IdServicio { get; set; }

    public string? Nombre { get; set; }

    public string? Descripcion { get; set; }

    public decimal? Costo { get; set; }

    public virtual ICollection<ServiciosRealizado> ServiciosRealizados { get; set; } = new List<ServiciosRealizado>();
}
