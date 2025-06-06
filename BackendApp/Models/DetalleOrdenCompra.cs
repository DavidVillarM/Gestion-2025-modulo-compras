namespace BackendApp.Models
{
    public class DetalleOrdenCompra
    {
        public int OrdenCompraId { get; set; }
        public OrdenCompra OrdenCompra { get; set; }

        public int ProductoId { get; set; }
        public Producto Producto { get; set; }

        public int CantidadSolicitada { get; set; }
        public int CantidadRecibida { get; set; }
        public decimal PrecioUnitario { get; set; }
    }
}
