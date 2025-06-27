using System;
using System.Collections.Generic;

namespace BackendApp.Models;

public partial class Ordene
{
    public long IdOrden { get; set; }

    public string? Estado { get; set; }

    public DateOnly? Fecha { get; set; }

    public virtual ICollection<Asiento> Asientos { get; set; } = new List<Asiento>();

    public virtual ICollection<OrdenDetalle> OrdenDetalles { get; set; } = new List<OrdenDetalle>();

    public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();

    public virtual ICollection<Recepcion> Recepcions { get; set; } = new List<Recepcion>();
}
