using System;
using System.Collections.Generic;

namespace BackendApp.Models;

public partial class Stock
{
    public long IdStock { get; set; }

    public long? IdPersonal { get; set; }

    public string? Ubicacion { get; set; }

    public virtual Personal? IdPersonalNavigation { get; set; }

    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
}
