// Data/ApplicationDbContext.cs (versión actualizada)
using Microsoft.EntityFrameworkCore;
using ModuloCompras.Models;

namespace ModuloCompras.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Proveedor> Proveedores { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<CategoriaProveedor> CategoriaProveedores { get; set; }

        public DbSet<Producto> Productos { get; set; }
        public DbSet<ProductoProveedor> ProductoProveedor { get; set; }

        public DbSet<Factura> Facturas { get; set; }
        public DbSet<FacturaDetalle> FacturaDetalles { get; set; }

        public DbSet<Orden> Ordenes { get; set; }
        public DbSet<OrdenDetalle> OrdenDetalles { get; set; }

        public DbSet<Presupuesto> Presupuestos { get; set; }
        public DbSet<PresupuestoDetalle> PresupuestoDetalles { get; set; }

        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<PedidoDetalle> PedidoDetalles { get; set; }

        public DbSet<Precio> Precios { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // PROVEEDOR
            modelBuilder.Entity<Proveedor>(entity =>
            {
                entity.ToTable("proveedores");
                entity.HasKey(e => e.IdProveedor);
                entity.Property(e => e.IdProveedor).HasColumnName("id_proveedor");
                entity.Property(e => e.Ruc).HasColumnName("ruc");
                entity.Property(e => e.Nombre).HasColumnName("nombre");
                entity.Property(e => e.Telefono).HasColumnName("telefono");
                entity.Property(e => e.Correo).HasColumnName("correo");
                entity.Property(e => e.NombreContacto).HasColumnName("nombre_contacto");

                entity.HasMany(e => e.CategoriaProveedores)
                      .WithOne(cp => cp.Proveedor)
                      .HasForeignKey(cp => cp.IdProveedor);

                entity.HasMany(e => e.Presupuestos)
                      .WithOne(p => p.Proveedor)
                      .HasForeignKey(p => p.IdProveedor);

                // Se removió la relación a PedidoDetalles vía Proveedor,
                // porque PedidoDetalle ya no almacena IdProveedor.
                entity.HasMany(e => e.ProductoProveedores)
                      .WithOne(pp => pp.Proveedor)
                      .HasForeignKey(pp => pp.IdProveedor);
            });

            // CATEGORIA
            modelBuilder.Entity<Categoria>(entity =>
            {
                entity.ToTable("categoria");
                entity.HasKey(e => e.IdCategoria);
                entity.Property(e => e.IdCategoria).HasColumnName("id_categoria");
                entity.Property(e => e.Nombre).HasColumnName("nombre");
                entity.Property(e => e.Descripcion).HasColumnName("descripcion");

                entity.HasMany(e => e.CategoriaProveedores)
                      .WithOne(cp => cp.Categoria)
                      .HasForeignKey(cp => cp.IdCategoria);

                entity.HasMany(e => e.Productos)
                      .WithOne(p => p.Categoria)
                      .HasForeignKey(p => p.IdCategoria);
            });

            // CATEGORIA_PROVEEDOR
            modelBuilder.Entity<CategoriaProveedor>(entity =>
            {
                entity.ToTable("categoria_proveedor");
                entity.HasKey(e => e.IdCategoriaProveedor);
                entity.Property(e => e.IdCategoriaProveedor).HasColumnName("id_categoria_proveedor");
                entity.Property(e => e.IdCategoria).HasColumnName("id_categoria");
                entity.Property(e => e.IdProveedor).HasColumnName("id_proveedor");
                entity.Property(e => e.Estado).HasColumnName("estado");
            });

            // PRODUCTO
            modelBuilder.Entity<Producto>(entity =>
            {
                entity.ToTable("productos");
                entity.HasKey(e => e.IdProducto);
                entity.Property(e => e.IdProducto).HasColumnName("id_producto");
                entity.Property(e => e.IdCategoria).HasColumnName("id_categoria");
                entity.Property(e => e.Nombre).HasColumnName("nombre");
                entity.Property(e => e.Marca).HasColumnName("marca");
                entity.Property(e => e.CantidadTotal).HasColumnName("cantidad_total");
                entity.Property(e => e.CantidadMinima).HasColumnName("cantidad_minima");

                entity.HasMany(e => e.ProductoProveedores)
                      .WithOne(pp => pp.Producto)
                      .HasForeignKey(pp => pp.IdProducto);

                entity.HasMany(e => e.OrdenDetalles)
                      .WithOne(d => d.Producto)
                      .HasForeignKey(d => d.IdProducto);

                entity.HasMany(e => e.PresupuestoDetalles)
                      .WithOne(d => d.Producto)
                      .HasForeignKey(d => d.IdProducto);

                entity.HasMany(e => e.PedidoDetalles)
                      .WithOne(d => d.Producto)
                      .HasForeignKey(d => d.IdProducto);

                entity.HasMany(e => e.Precios)
                      .WithOne(p => p.Producto)
                      .HasForeignKey(p => p.IdProducto);
            });

            // PRODUCTO_PROVEEDOR
            modelBuilder.Entity<ProductoProveedor>(entity =>
            {
                entity.ToTable("producto_proveedor");
                entity.HasKey(e => e.IdProductoProveedor);
                entity.Property(e => e.IdProductoProveedor).HasColumnName("id_producto_proveedor");
                entity.Property(e => e.IdProducto).HasColumnName("id_producto");
                entity.Property(e => e.IdProveedor).HasColumnName("id_proveedor");
                entity.Property(e => e.FechaCompra).HasColumnName("fecha_compra");
                entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            });

            // FACTURA
            modelBuilder.Entity<Factura>(entity =>
            {
                entity.ToTable("facturas");
                entity.HasKey(e => e.Id_Factura);
                entity.Property(e => e.Id_Factura).HasColumnName("id_factura");
                entity.Property(e => e.Id_Pedido).HasColumnName("id_pedido");
                entity.Property(e => e.Id_Proveedor).HasColumnName("id_proveedor");
                entity.Property(e => e.Fecha).HasColumnName("fecha");
                entity.Property(e => e.Ruc).HasColumnName("ruc");
                entity.Property(e => e.Nombre_Proveedor).HasColumnName("nombre_proveedor");
                entity.Property(e => e.Timbrado).HasColumnName("timbrado");
                entity.Property(e => e.Subtotal).HasColumnName("subtotal");
                entity.Property(e => e.Iva5).HasColumnName("iva5");
                entity.Property(e => e.Iva10).HasColumnName("iva10");
                entity.Property(e => e.Monto_Total).HasColumnName("monto_total");
                entity.Property(e => e.Estado).HasColumnName("estado");

                entity.HasMany(e => e.Detalles)
                      .WithOne(d => d.Factura)
                      .HasForeignKey(d => d.Id_Factura)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // FACTURA_DETALLE
            modelBuilder.Entity<FacturaDetalle>(entity =>
            {
                entity.ToTable("factura_detalle");
                entity.HasKey(e => e.Id_Factura_Detalle);
                entity.Property(e => e.Id_Factura_Detalle).HasColumnName("id_factura_detalle");
                entity.Property(e => e.Id_Factura).HasColumnName("id_factura");
                entity.Property(e => e.Id_Producto).HasColumnName("id_producto");
                entity.Property(e => e.Precio).HasColumnName("precio");
                entity.Property(e => e.Cantidad).HasColumnName("cantidad");
                entity.Property(e => e.Iva5).HasColumnName("iva5");
                entity.Property(e => e.Iva10).HasColumnName("iva10");
            });

            // ORDEN
            modelBuilder.Entity<Orden>(entity =>
            {
                entity.ToTable("ordenes");
                entity.HasKey(e => e.IdOrden);
                entity.Property(e => e.IdOrden).HasColumnName("id_orden");
                entity.Property(e => e.Estado).HasColumnName("estado");
                entity.Property(e => e.Fecha).HasColumnName("fecha"); // **Se añadió mapeo de Fecha**

                entity.HasMany(e => e.OrdenDetalles)
                      .WithOne(d => d.Orden)
                      .HasForeignKey(d => d.IdOrden);
            });

            modelBuilder.Entity<OrdenDetalle>(entity =>
            {
                entity.ToTable("orden_detalle");
                entity.HasKey(e => e.IdOrdenDetalle);
                entity.Property(e => e.IdOrdenDetalle).HasColumnName("id_orden_detalle");
                entity.Property(e => e.IdOrden).HasColumnName("id_orden");
                entity.Property(e => e.IdProducto).HasColumnName("id_producto");
                entity.Property(e => e.Cantidad).HasColumnName("cantidad");

                entity.HasOne(d => d.Producto)
                      .WithMany(p => p.OrdenDetalles)
                      .HasForeignKey(d => d.IdProducto);
            });

            // PRESUPUESTO
            modelBuilder.Entity<Presupuesto>(entity =>
            {
                entity.ToTable("presupuestos");
                entity.HasKey(e => e.IdPresupuesto);
                entity.Property(e => e.IdPresupuesto).HasColumnName("id_presupuesto");
                entity.Property(e => e.IdOrden).HasColumnName("id_orden");
                entity.Property(e => e.IdProveedor).HasColumnName("id_proveedor");
                entity.Property(e => e.FechaEntrega).HasColumnName("fecha_entrega");
                entity.Property(e => e.Subtotal).HasColumnName("subtotal");
                entity.Property(e => e.Iva5).HasColumnName("iva5");
                entity.Property(e => e.Iva10).HasColumnName("iva10");
                entity.Property(e => e.Total).HasColumnName("total");

                entity.HasOne(e => e.Orden)
                      .WithMany(o => o.Presupuestos)
                      .HasForeignKey(e => e.IdOrden);

                entity.HasOne(e => e.Proveedor)
                      .WithMany(p => p.Presupuestos)
                      .HasForeignKey(e => e.IdProveedor);
            });

            modelBuilder.Entity<PresupuestoDetalle>(entity =>
            {
                entity.ToTable("presupuesto_detalle");
                entity.HasKey(e => e.IdPresupuestoDetalle);
                entity.Property(e => e.IdPresupuestoDetalle).HasColumnName("id_presupuesto_detalle");
                entity.Property(e => e.IdPresupuesto).HasColumnName("id_presupuesto");
                entity.Property(e => e.IdProducto).HasColumnName("id_producto");
                entity.Property(e => e.Cantidad).HasColumnName("cantidad");
                entity.Property(e => e.Precio).HasColumnName("precio");
                entity.Property(e => e.Iva5).HasColumnName("iva5");
                entity.Property(e => e.Iva10).HasColumnName("iva10");

                entity.HasOne(d => d.Presupuesto)
                      .WithMany(p => p.Detalles)
                      .HasForeignKey(d => d.IdPresupuesto);

                entity.HasOne(d => d.Producto)
                      .WithMany(p => p.PresupuestoDetalles)
                      .HasForeignKey(d => d.IdProducto);
            });

            // PEDIDO (versión actualizada)
            modelBuilder.Entity<Pedido>(entity =>
            {
                entity.ToTable("pedidos");
                entity.HasKey(e => e.IdPedido);
                entity.Property(e => e.IdPedido).HasColumnName("id_pedido");
                entity.Property(e => e.IdOrden).HasColumnName("id_orden");
                entity.Property(e => e.IdProveedor).HasColumnName("id_proveedor");
                entity.Property(e => e.MontoTotal).HasColumnName("monto_total");
                entity.Property(e => e.FechaPedido).HasColumnName("fecha_pedido");
                entity.Property(e => e.FechaEntrega).HasColumnName("fecha_entrega");
                entity.Property(e => e.Estado).HasColumnName("estado");

                entity.HasOne(e => e.Orden)
                      .WithMany(o => o.Pedidos)
                      .HasForeignKey(e => e.IdOrden);

                entity.HasOne(e => e.Proveedor)
                      .WithMany(p => p.Pedidos)
                      .HasForeignKey(e => e.IdProveedor);
            });

            modelBuilder.Entity<PedidoDetalle>(entity =>
            {
                entity.ToTable("pedido_detalles");
                entity.HasKey(e => e.IdPedidoDetalle);
                entity.Property(e => e.IdPedidoDetalle).HasColumnName("id_pedido_detalle");
                entity.Property(e => e.IdPedido).HasColumnName("id_pedido");
                entity.Property(e => e.IdProducto).HasColumnName("id_producto");
                // Se removió IdProveedor porque la tabla ya no lo tiene
                entity.Property(e => e.Cotizacion).HasColumnName("cotizacion");
                entity.Property(e => e.Cantidad).HasColumnName("cantidad");
                entity.Property(e => e.Iva).HasColumnName("iva");

                entity.HasOne(d => d.Pedido)
                      .WithMany(p => p.Detalles)
                      .HasForeignKey(d => d.IdPedido);

                entity.HasOne(d => d.Producto)
                      .WithMany(p => p.PedidoDetalles)
                      .HasForeignKey(d => d.IdProducto);

                // NO hay relación directa a Proveedor aquí
            });

            // PRECIO
            modelBuilder.Entity<Precio>(entity =>
            {
                entity.ToTable("precios");
                entity.HasKey(e => e.IdPrecio);
                entity.Property(e => e.IdPrecio).HasColumnName("id_precio");
                entity.Property(e => e.IdProducto).HasColumnName("id_producto");
                entity.Property(e => e.Valor).HasColumnName("precio");
                entity.Property(e => e.FechaRegistro).HasColumnName("fecha_registro");
                entity.Property(e => e.Estado).HasColumnName("estado");

                entity.HasOne(e => e.Producto)
                      .WithMany(p => p.Precios)
                      .HasForeignKey(e => e.IdProducto);
            });
        }
    }
}
