namespace ModuloCompras.DTOs
{
    // ─── DTOs para Proveedor ───

    public class ProveedorCreateDto
    {
        public string Ruc { get; set; }
        public string Nombre { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }
        public string NombreContacto { get; set; }
        public List<int> CategoriaIds { get; set; } = new List<int>();
    }

    public class ProveedorReadDto
    {
        public int IdProveedor { get; set; }
        public string Ruc { get; set; }
        public string Nombre { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }
        public string NombreContacto { get; set; }
        public List<CategoriaAsignadaDto> Categorias { get; set; } = new List<CategoriaAsignadaDto>();
    }   

}
