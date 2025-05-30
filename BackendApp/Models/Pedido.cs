using System;
using System.Collections.Generic;

namespace BackendApp.Models;

public partial class Pedido
{
    public long IdPedido { get; set; }

    public long IdOrden { get; set; }

    public decimal? MontoTotal { get; set; }

    public DateOnly? FechaEntrega { get; set; }

    public DateOnly? FechaPedido { get; set; }

    public string? Estado { get; set; }

    public virtual Ordene IdOrdenNavigation { get; set; } = null!;

    public virtual ICollection<PedidoDetalle> PedidoDetalles { get; set; } = new List<PedidoDetalle>();
}
