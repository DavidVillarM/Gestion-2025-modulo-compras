using System;
using System.Collections.Generic;

namespace BackendApp.Models;

public partial class Producto
{
    public long IdProducto { get; set; }

    public long? IdCategoria { get; set; }

    public long? IdStock { get; set; }

    public string? Nombre { get; set; }

    public string? Marca { get; set; }

    public int? CantidadTotal { get; set; }

    public int? CantidadMinima { get; set; }

    public virtual ICollection<BajasProducto> BajasProductos { get; set; } = new List<BajasProducto>();

    public virtual ICollection<DetalleAjusteStock> DetalleAjusteStocks { get; set; } = new List<DetalleAjusteStock>();

    public virtual ICollection<FacturaDetalle> FacturaDetalles { get; set; } = new List<FacturaDetalle>();

    public virtual Categorium? IdCategoriaNavigation { get; set; }

    public virtual Stock? IdStockNavigation { get; set; }

    public virtual ICollection<NotaCreditoDetalle> NotaCreditoDetalles { get; set; } = new List<NotaCreditoDetalle>();

    public virtual ICollection<OrdenDetalle> OrdenDetalles { get; set; } = new List<OrdenDetalle>();

    public virtual ICollection<PedidoDetalle> PedidoDetalles { get; set; } = new List<PedidoDetalle>();

    public virtual ICollection<Precio> Precios { get; set; } = new List<Precio>();

    public virtual ICollection<PresupuestoDetalle> PresupuestoDetalles { get; set; } = new List<PresupuestoDetalle>();

    public virtual ICollection<ProductoProveedor> ProductoProveedors { get; set; } = new List<ProductoProveedor>();

    public virtual ICollection<ServiciosProductosUtilizado> ServiciosProductosUtilizados { get; set; } = new List<ServiciosProductosUtilizado>();
}
