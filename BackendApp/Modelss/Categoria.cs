using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace BackendApp.Modelss
{
    public class Categoria
    {
        public int IdCategoria { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }

        public ICollection<CategoriaProveedor> CategoriaProveedores { get; set; } = new List<CategoriaProveedor>();
        public ICollection<Producto> Productos { get; set; } = new List<Producto>();
    }
}