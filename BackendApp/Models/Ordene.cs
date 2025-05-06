using System;
using System.Collections.Generic;

namespace BackendApp.Models;

public partial class Ordene
{
    public long IdOrden { get; set; }

    public string? Estado { get; set; }

    public virtual ICollection<OrdenDetalle> OrdenDetalles { get; set; } = new List<OrdenDetalle>();

    public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();

    public virtual ICollection<Presupuesto> Presupuestos { get; set; } = new List<Presupuesto>();
}
