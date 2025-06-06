using System;
using System.Collections.Generic;

namespace BackendApp.Models;

public partial class Categorium
{
    public long IdCategoria { get; set; }

    public string? Nombre { get; set; }

    public string? Descripcion { get; set; }

    public virtual ICollection<CategoriaProveedor> CategoriaProveedors { get; set; } = new List<CategoriaProveedor>();

    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
}
