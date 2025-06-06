namespace BackendApp.Dtos
{
    public class FacturaRequestDto
    {
        public long OrdenId { get; set; }
        public long ProveedorId { get; set; }
        public DateTime FechaEmision { get; set; }
        public List<FacturaItemDto> Items { get; set; }
    }

    public class FacturaItemDto
    {
        public long ProductoId { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
    }

}
