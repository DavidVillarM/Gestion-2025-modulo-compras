using System;
using System.Collections.Generic;

namespace BackendApp.Models;

public partial class PedidoDetalle
{
    public long IdPedidoDetalle { get; set; }

    public long IdPedido { get; set; }

    public long IdProducto { get; set; }

    public decimal? Cotizacion { get; set; }

    public int? Cantidad { get; set; }

    public decimal? Iva { get; set; }

    public virtual Pedido IdPedidoNavigation { get; set; } = null!;

    public virtual Producto IdProductoNavigation { get; set; } = null!;
}
