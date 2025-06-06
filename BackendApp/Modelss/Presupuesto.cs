using System;
using System.Collections.Generic;

namespace BackendApp.Modelss
{
    public class Presupuesto
    {
        public int IdPresupuesto { get; set; }    // id_presupuesto
        public int IdOrden { get; set; }          // id_orden
        public int IdProveedor { get; set; }      // id_proveedor
        public DateTime FechaEntrega { get; set; }// fecha_entrega
        public decimal Subtotal { get; set; }     // subtotal
        public decimal Iva5 { get; set; }         // iva5
        public decimal Iva10 { get; set; }        // iva10
        public decimal Total { get; set; }        // total

        // Navegaciones
        public Orden Orden { get; set; }
        public Proveedor Proveedor { get; set; }

        public ICollection<PresupuestoDetalle> Detalles { get; set; }
            = new List<PresupuestoDetalle>();
    }
}
