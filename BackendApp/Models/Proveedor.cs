// Models/Proveedor.cs
using System.Collections.Generic;

namespace ModuloCompras.Models
{
    public class Proveedor
    {
        public int IdProveedor { get; set; }
        public string Ruc { get; set; }
        public string Nombre { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }
        public string NombreContacto { get; set; }

        // Navegaciones
        public ICollection<Presupuesto> Presupuestos { get; set; } = new List<Presupuesto>();
        public ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
        public ICollection<ProductoProveedor> ProductoProveedores { get; set; } = new List<ProductoProveedor>();
        public ICollection<CategoriaProveedor> CategoriaProveedores { get; set; } = new List<CategoriaProveedor>();
    }
}
