using System;
using System.Collections.Generic;

namespace BackendApp.Modelss
{
    public class Orden
    {
        public int IdOrden { get; set; }           // id_orden
        public string Estado { get; set; }        // estado
        public DateTime Fecha { get; set; }       // fecha

        // Navegación a los detalles de orden
        public ICollection<OrdenDetalle> OrdenDetalles { get; set; }
            = new List<OrdenDetalle>();

        // Navegación a los presupuestos generados para esta orden
        public ICollection<Presupuesto> Presupuestos { get; set; }
            = new List<Presupuesto>();

        // Navegación a los pedidos finales generados a partir de esta orden
        public ICollection<Pedido> Pedidos { get; set; }
            = new List<Pedido>();
    }
}
