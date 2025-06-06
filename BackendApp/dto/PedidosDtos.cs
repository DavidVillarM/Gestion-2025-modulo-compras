namespace BackendApp.DTOs
{
    // ─── DTOs para Pedido (orden de compra final) ───

    public class PedidoCreateDto
    {
        public int IdOrden { get; set; }
        public int IdProveedor { get; set; }
        public decimal MontoTotal { get; set; }
        public DateTime? FechaPedido { get; set; }
        public DateTime? FechaEntrega { get; set; }
        public string Estado { get; set; }
        public List<PedidoDetalleCreateDto> Detalles { get; set; } = new List<PedidoDetalleCreateDto>();
    }

    public class PedidoReadDto
    {
        public int IdPedido { get; set; }
        public int IdOrden { get; set; }
        public int IdProveedor { get; set; }
        public string NombreProveedor { get; set; }
        public decimal MontoTotal { get; set; }
        public DateTime? FechaPedido { get; set; }
        public DateTime? FechaEntrega { get; set; }
        public string Estado { get; set; }
        public List<PedidoDetalleReadDto> Detalles { get; set; } = new List<PedidoDetalleReadDto>();
    }

    public class PedidoDetalleCreateDto
    {
        public int IdProducto { get; set; }
        public int Cantidad { get; set; }
        public decimal Cotizacion { get; set; }
        public decimal Iva { get; set; }
    }

    public class PedidoDetalleReadDto
    {
        public int IdPedidoDetalle { get; set; }
        public int IdProducto { get; set; }
        public int Cantidad { get; set; }
        public decimal Cotizacion { get; set; }
        public decimal Iva { get; set; }
        public string NombreProducto { get; set; }
    }
}
