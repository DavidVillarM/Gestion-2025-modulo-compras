// Models/Producto.cs
using System.Collections.Generic;

namespace BackendApp.Modelss
{
    public class Producto
    {
        public int IdProducto { get; set; }
        public int IdCategoria { get; set; }
        public string Nombre { get; set; }
        public string Marca { get; set; }
        public int CantidadTotal { get; set; }
        public int CantidadMinima { get; set; }

        // Navegaciones
        public Categoria Categoria { get; set; }

        public ICollection<ProductoProveedor> ProductoProveedores { get; set; }
            = new List<ProductoProveedor>();

        public ICollection<OrdenDetalle> OrdenDetalles { get; set; }
            = new List<OrdenDetalle>();

        public ICollection<PresupuestoDetalle> PresupuestoDetalles { get; set; }
            = new List<PresupuestoDetalle>();

        public ICollection<PedidoDetalle> PedidoDetalles { get; set; }
            = new List<PedidoDetalle>();

        public ICollection<Precio> Precios { get; set; }
            = new List<Precio>();
    }
}
