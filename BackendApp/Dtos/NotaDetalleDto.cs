namespace BackendApp.Dtos
{
    public class NotaDetalleDto
    {
        public long IdNota { get; set; }

        public long IdProducto { get; set; }

        public decimal? Precio { get; set; }

        public int? Cantidad { get; set; }

        public decimal? Iva5 { get; set; }

        public decimal? Iva10 { get; set; }
    }
}
