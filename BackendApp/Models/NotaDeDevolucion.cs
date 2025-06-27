using System;
using System.Collections.Generic;

namespace BackendApp.Models;

public partial class NotaDeDevolucion
{
    public int Id { get; set; }

    public long? PedidoId { get; set; }

    public long? ProductoId { get; set; }

    public string Motivo { get; set; } = null!;

    public DateTime? Fecha { get; set; }

    public virtual Pedido? Pedido { get; set; }

    public virtual Producto? Producto { get; set; }
}
