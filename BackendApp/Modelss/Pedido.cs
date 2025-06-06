using System;
using System.Collections.Generic;

namespace BackendApp.Modelss
{
    public class Pedido
    {
        public int IdPedido { get; set; }             // id_pedido
        public int IdOrden { get; set; }              // id_orden
        public int IdProveedor { get; set; }          // id_proveedor
        public decimal MontoTotal { get; set; }       // monto_total
        public DateTime? FechaPedido { get; set; }    // fecha_pedido
        public DateTime? FechaEntrega { get; set; }   // fecha_entrega
        public string Estado { get; set; }            // estado

        // Navegaciones
        public Orden Orden { get; set; }
        public Proveedor Proveedor { get; set; }

        public ICollection<PedidoDetalle> Detalles { get; set; }
            = new List<PedidoDetalle>();
    }
}
