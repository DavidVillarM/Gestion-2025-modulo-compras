using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendApp.Modelss
{
    [Table("facturas")]
    public class Factura
    {
        [Key]
        [Column("id_factura")]
        public int Id_Factura { get; set; }

        // FK a pedido (puede ser nullable si no siempre existe)
        [Column("id_pedido")]
        public int? Id_Pedido { get; set; }

        // FK a proveedor (no nullable)
        [Column("id_proveedor")]
        public int Id_Proveedor { get; set; }

        [Column("fecha")]
        public DateTime Fecha { get; set; }

        [Column("ruc")]
        public string Ruc { get; set; }

        [Column("nombre_proveedor")]
        public string Nombre_Proveedor { get; set; }

        [Column("timbrado")]
        public string Timbrado { get; set; }

        [Column("subtotal")]
        public decimal Subtotal { get; set; }

        [Column("iva5")]
        public decimal Iva5 { get; set; }

        [Column("iva10")]
        public decimal Iva10 { get; set; }

        [Column("monto_total")]
        public decimal Monto_Total { get; set; }

        [Column("estado")]
        public string Estado { get; set; }

        // Relación 1:N con detalles
        public List<FacturaDetalle> Detalles { get; set; } = new List<FacturaDetalle>();
    }
}
