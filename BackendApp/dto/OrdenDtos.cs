namespace ModuloCompras.DTOs
{
    // ─── DTOs para Orden (solicitud de compra) ───

    public class OrdenCreateDto
    {
        // Nota: al crear, el campo Estado se ignora (el controlador siempre lo defiende como "INCOMPLETA")
        // pero lo dejamos aquí por si en el futuro decides enviar un estado distinto.
        public string Estado { get; set; }
        public List<OrdenDetalleCreateDto> Detalles { get; set; } = new List<OrdenDetalleCreateDto>();
    }

    public class OrdenReadDto
    {
        public int IdOrden { get; set; }
        public DateTime Fecha { get; set; }
        public string Estado { get; set; }
        public List<OrdenDetalleReadDto> Detalles { get; set; } = new List<OrdenDetalleReadDto>();
    }

    public class OrdenDetalleCreateDto
    {
        public int IdProducto { get; set; }
        public int Cantidad { get; set; }
    }

    public class OrdenDetalleReadDto
    {
        public int IdOrdenDetalle { get; set; }
        public int IdProducto { get; set; }
        public int Cantidad { get; set; }
        public string NombreProducto { get; set; }
        public int IdCategoria { get; set; }
        public string CategoriaNombre { get; set; }
    }
    public class OrdenUpdateDto
    {
        public string Estado { get; set; }
    }
    public class ProveedorSimpleDto
    {
        public int IdProveedor { get; set; }
        public string Nombre { get; set; }
        public string Ruc { get; set; }
    }
}
