namespace ModuloCompras.Models
{
    public class PedidoDetalle
    {
        public int IdPedidoDetalle { get; set; }   // id_pedido_detalle
        public int IdPedido { get; set; }          // id_pedido
        public int IdProducto { get; set; }        // id_producto
        public decimal Cotizacion { get; set; }    // cotizacion
        public int Cantidad { get; set; }          // cantidad
        public decimal Iva { get; set; }           // iva

        // Navegaciones
        public Pedido Pedido { get; set; }
        public Producto Producto { get; set; }

        // Ya no hay ID de proveedor ni navegación a Proveedor aquí.
        // El proveedor se asume desde la cabecera Pedido.IdProveedor.
    }
}
