using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace BackendApp.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Proveedor> Proveedores { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<OrdenCompra> OrdenesCompra { get; set; }
        public DbSet<DetalleOrdenCompra> DetallesOrdenCompra { get; set; }
        public DbSet<Factura> Facturas { get; set; }
        public DbSet<Stock> Stock { get; set; }
        public DbSet<NotaDeDevolucion> NotasDeDevolucion { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Relaciones simples
            modelBuilder.Entity<DetalleOrdenCompra>()
                .HasKey(d => new { d.OrdenCompraId, d.ProductoId });

            modelBuilder.Entity<DetalleOrdenCompra>()
                .HasOne(d => d.Producto)
                .WithMany()
                .HasForeignKey(d => d.ProductoId);

            modelBuilder.Entity<DetalleOrdenCompra>()
                .HasOne(d => d.OrdenCompra)
                .WithMany(o => o.Detalles)
                .HasForeignKey(d => d.OrdenCompraId);
        }
    }
}
