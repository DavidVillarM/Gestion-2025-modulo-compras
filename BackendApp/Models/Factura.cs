using System;

namespace BackendApp.Models
{
    public class Factura
    {
        public int Id { get; set; }
        public string Numero { get; set; }
        public string Timbrado { get; set; }
        public DateTime Fecha { get; set; }

        public int OrdenCompraId { get; set; }
        public OrdenCompra OrdenCompra { get; set; }
    }
}
