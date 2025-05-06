using System;
using System.Collections.Generic;

namespace BackendApp.Models;

public partial class Usuario
{
    public long IdUser { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public virtual ICollection<Personal> Personals { get; set; } = new List<Personal>();
}
