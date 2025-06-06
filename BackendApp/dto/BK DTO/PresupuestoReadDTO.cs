//// DTOs/PresupuestoReadDTO.cs
//using System;
//using System.Collections.Generic;

//namespace ModuloCompras.DTOs
//{
//    public class PresupuestoDetalleReadDTO
//    {
//        public int IdPresupuestoDetalle { get; set; }
//        public int IdProducto { get; set; }
//        public int Cantidad { get; set; }
//        public decimal Precio { get; set; }
//        public decimal Iva5 { get; set; }
//        public decimal Iva10 { get; set; }

//        // Para mostrar nombre de producto (opcional)
//        public string NombreProducto { get; set; }
//    }

//    public class PresupuestoReadDTO
//    {
//        public int IdPresupuesto { get; set; }
//        public int IdOrden { get; set; }
//        public int IdProveedor { get; set; }

//        // Si quieres mostrar el nombre del proveedor
//        public string NombreProveedor { get; set; }

//        public DateTime FechaEntrega { get; set; }
//        public decimal Subtotal { get; set; }
//        public decimal Iva5 { get; set; }
//        public decimal Iva10 { get; set; }
//        public decimal Total { get; set; }

//        public List<PresupuestoDetalleReadDTO> Detalles { get; set; }
//            = new List<PresupuestoDetalleReadDTO>();
//    }
//}
