namespace BackendApp.Dtos
{
    public class NotaCreditoCabecera
    {
        public long IdNota { get; set; }

        public long IdFactura { get; set; }

        public long IdProveedor { get; set; }

        public DateOnly? Fecha { get; set; }

        public string? Ruc { get; set; }

        public string? NombreProveedor { get; set; }

        public string? Timbrado { get; set; }

        public decimal? MontoTotal { get; set; }

        public decimal? Subtotal { get; set; }

        public decimal? Iva5 { get; set; }

        public decimal? Iva10 { get; set; }

        public string? Estado { get; set; }
    }
}
