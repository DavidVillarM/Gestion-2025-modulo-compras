// Models/CategoriaProveedor.cs
namespace BackendApp.Modelss
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
