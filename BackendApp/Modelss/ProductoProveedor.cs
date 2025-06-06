// Models/ProductoProveedor.cs
using System;
namespace BackendApp.Modelss
{
    public class ProductoProveedor
    {
        public int IdProductoProveedor { get; set; }
        public int IdProducto { get; set; }
        public int IdProveedor { get; set; }
        public DateTime FechaCompra { get; set; }
        public int Cantidad { get; set; }

        // Navegaciones
        public Producto Producto { get; set; }
        public Proveedor Proveedor { get; set; }
    }
}
