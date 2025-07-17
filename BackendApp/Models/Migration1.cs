using System;
using System.Collections.Generic;

namespace BackendApp.Models;

public partial class Migration1
{
    public string Version { get; set; } = null!;

    public string? Name { get; set; }

    public DateTime AppliedAt { get; set; }
}
