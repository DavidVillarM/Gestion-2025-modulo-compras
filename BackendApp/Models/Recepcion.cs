using System;
using System.Collections.Generic;

namespace BackendApp.Models;

public partial class Recepcion
{
    public long Id { get; set; }

    public long IdOrden { get; set; }

    public long IdPedido { get; set; }

    public string Estado { get; set; } = null!;

    public DateTime FechaRecepcion { get; set; }

    public string Timbrado { get; set; } = null!;

    public string NumeroFactura { get; set; } = null!;

    public virtual Ordene IdOrdenNavigation { get; set; } = null!;

    public virtual Pedido IdPedidoNavigation { get; set; } = null!;

    public virtual ICollection<RecepcionDetalle> RecepcionDetalles { get; set; } = new List<RecepcionDetalle>();
}
