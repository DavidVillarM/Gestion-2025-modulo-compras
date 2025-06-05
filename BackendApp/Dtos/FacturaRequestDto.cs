namespace BackendApp.Dtos
{
    public class FacturaRequestDto
    {
        public int OrdenId { get; set; }
        public int ProveedorId { get; set; }
        public DateTime FechaEmision { get; set; }
        public List<FacturaItemDto> Items { get; set; }
    }

    public class FacturaItemDto
    {
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
    }

}
