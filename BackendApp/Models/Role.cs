using System;
using System.Collections.Generic;

namespace BackendApp.Models;

public partial class Role
{
    public long IdRol { get; set; }

    public string? Descripcion { get; set; }

    public virtual ICollection<Personal> Personals { get; set; } = new List<Personal>();
}
