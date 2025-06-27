using System;
using System.Collections.Generic;

namespace BackendApp.Models;

public partial class Migration1
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Hash { get; set; } = null!;

    public DateTime? ExecutedAt { get; set; }
}
