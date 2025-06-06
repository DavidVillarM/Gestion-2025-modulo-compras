using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendApp.Modelss
{
    [Table("factura_detalle")]
    public class FacturaDetalle
    {
        [Key]
        [Column("id_factura_detalle")]
        public int Id_Factura_Detalle { get; set; }

        [Column("id_factura")]
        public int Id_Factura { get; set; }

        [ForeignKey(nameof(Id_Factura))]
        public Factura Factura { get; set; }

        [Column("id_producto")]
        public int Id_Producto { get; set; }

        [Column("precio")]
        public decimal Precio { get; set; }

        [Column("cantidad")]
        public int Cantidad { get; set; }

        [Column("iva5")]
        public decimal Iva5 { get; set; }

        [Column("iva10")]
        public decimal Iva10 { get; set; }
    }
}
