using System;
using System.Collections.Generic;

namespace BackendApp.Models;

public partial class Migration
{
    public string Version { get; set; } = null!;

    public string? Name { get; set; }

    public DateTime AppliedAt { get; set; }
}
