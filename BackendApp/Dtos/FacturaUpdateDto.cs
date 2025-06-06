using System.ComponentModel.DataAnnotations;

namespace BackendApp.Dtos
{
    public class FacturaUpdateDto
    {
        [Required]
        public long IdProveedor { get; set; }

        [Required]
        public long IdPedido { get; set; }

        [Required]
        public DateOnly Fecha { get; set; }

        [StringLength(20)]
        public string? Ruc { get; set; }

        [Required]
        [StringLength(100)]
        public string? NombreProveedor { get; set; }

        [StringLength(20)]
        public string? Timbrado { get; set; }

        [Range(0, double.MaxValue)]
        public decimal MontoTotal { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Subtotal { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Iva5 { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Iva10 { get; set; }

        [Required]
        public string Estado { get; set; } = "Pendiente";

        [MinLength(1, ErrorMessage = "Debe haber al menos un producto en la factura.")]
        public List<FacturaItemUpdateDto> FacturaDetalles { get; set; } = new();
    }

    public class FacturaItemUpdateDto
    {
        [Required]
        public long IdProducto { get; set; }

        [Range(1, int.MaxValue)]
        public int Cantidad { get; set; }

        [Range(1, double.MaxValue)]
        public decimal Precio { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Iva5 { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Iva10 { get; set; }
    }
}
