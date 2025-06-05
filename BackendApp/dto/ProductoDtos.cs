namespace ModuloCompras.DTOs
{
    // ─── DTOs para Producto ───

    public class ProductoCreateDto
    {
        public int? IdCategoria { get; set; }
        public string Nombre { get; set; }
        public string Marca { get; set; }
        public int CantidadTotal { get; set; }
        public int CantidadMinima { get; set; }
    }

    public class ProductoReadDto
    {
        public int IdProducto { get; set; }
        public string Nombre { get; set; }
        public string Marca { get; set; }
        public int IdCategoria { get; set; }
        public string NombreCategoria { get; set; }
        public int CantidadTotal { get; set; }
        public int CantidadMinima { get; set; }
    }

    public class ProductoProveedorCreateDto
    {
        public int IdProducto { get; set; }
        public int IdProveedor { get; set; }
        public DateTime FechaCompra { get; set; }
        public int Cantidad { get; set; }
    }

    public class ProductoProveedorReadDto
    {
        public int IdProductoProveedor { get; set; }
        public int IdProducto { get; set; }
        public string NombreProducto { get; set; }
        public int IdProveedor { get; set; }
        public string NombreProveedor { get; set; }
        public DateTime FechaCompra { get; set; }
        public int Cantidad { get; set; }
    }
}
