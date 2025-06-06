using System;
using System.Collections.Generic;

namespace BackendApp.Models;

public partial class Proveedore
{
    public long IdProveedor { get; set; }

    public string? Ruc { get; set; }

    public string? Nombre { get; set; }

    public string? Telefono { get; set; }

    public string? Correo { get; set; }

    public string? NombreContacto { get; set; }

    public virtual ICollection<CategoriaProveedor> CategoriaProveedors { get; set; } = new List<CategoriaProveedor>();

    public virtual ICollection<Factura> Facturas { get; set; } = new List<Factura>();

    public virtual ICollection<NotasCredito> NotasCreditos { get; set; } = new List<NotasCredito>();

    public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();

    public virtual ICollection<Presupuesto> Presupuestos { get; set; } = new List<Presupuesto>();

    public virtual ICollection<PresupuestosProveedore> PresupuestosProveedores { get; set; } = new List<PresupuestosProveedore>();

    public virtual ICollection<ProductoProveedor> ProductoProveedors { get; set; } = new List<ProductoProveedor>();
}
