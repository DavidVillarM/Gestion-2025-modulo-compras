// Models/CategoriaProveedor.cs
namespace ModuloCompras.Models
{
    public class CategoriaProveedor
    {
        public int IdCategoriaProveedor { get; set; }
        public int IdCategoria { get; set; }
        public int IdProveedor { get; set; }
        public string Estado { get; set; }

        // Navegaciones
        public Categoria Categoria { get; set; }
        public Proveedor Proveedor { get; set; }
    }
}
