using System;
using System.Collections.Generic;

namespace BackendApp.Models;

public partial class Embedding
{
    public long Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public string Content { get; set; } = null!;
}
