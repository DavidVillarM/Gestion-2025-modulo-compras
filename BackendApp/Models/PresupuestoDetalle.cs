namespace ModuloCompras.Models
{
    public class PresupuestoDetalle
    {
        public int IdPresupuestoDetalle { get; set; } // id_presupuesto_detalle
        public int IdPresupuesto { get; set; }       // id_presupuesto
        public int IdProducto { get; set; }          // id_producto
        public int Cantidad { get; set; }            // cantidad
        public decimal Precio { get; set; }          // precio
        public decimal Iva5 { get; set; }            // iva5
        public decimal Iva10 { get; set; }           // iva10

        // Navegaciones
        public Presupuesto Presupuesto { get; set; }
        public Producto Producto { get; set; }
    }
}
