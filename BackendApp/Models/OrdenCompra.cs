using System;
using System.Collections.Generic;

namespace BackendApp.Models
{
    public class OrdenCompra
    {
        public int Id { get; set; }
        public int ProveedorId { get; set; }
        public Proveedor Proveedor { get; set; }
        public DateTime Fecha { get; set; }

        public List<DetalleOrdenCompra> Detalles { get; set; }
    }
}
