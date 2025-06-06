namespace BackendApp.Modelss
{
    public class OrdenDetalle
    {
        public int IdOrdenDetalle { get; set; }   // id_orden_detalle
        public int IdOrden { get; set; }         // id_orden
        public int IdProducto { get; set; }      // id_producto
        public int Cantidad { get; set; }        // cantidad

        // Navegaciones
        public Orden Orden { get; set; }
        public Producto Producto { get; set; }
    }
}
