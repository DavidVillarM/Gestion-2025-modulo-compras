
using System;
using System.Collections.Generic;

namespace BackendApp.Dtos
{
    // DTO para cada línea de detalle de presupuesto
    public class PresupuestoDetalleDto
    {
        public long IdPresupuestoDetalle { get; set; }

        public long IdProducto { get; set; }         // 👈 NUEVO CAMPO
        public string Producto { get; set; } = "";
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }
        public decimal Iva5 { get; set; }
        public decimal Iva10 { get; set; }
        public decimal Subtotal { get; set; }
    }

    // DTO para agrupar la cabecera de Presupuesto con sus detalles
    public class PresupuestoConDetallesDto
    {
        public long IdPresupuesto { get; set; }
        public long IdProveedor { get; set; }
        public string Proveedor { get; set; } = "";
        public DateOnly? FechaEntrega { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Iva5 { get; set; }
        public decimal Iva10 { get; set; }
        public decimal Total { get; set; }

        public List<PresupuestoDetalleDto> Detalles { get; set; } = new();
    }
}