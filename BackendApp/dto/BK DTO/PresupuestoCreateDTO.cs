//// DTOs/PresupuestoCreateDTO.cs
//using System;
//using System.Collections.Generic;

//namespace ModuloCompras.DTOs
//{
//    public class PresupuestoCreateDTO
//    {
//        public int IdOrden { get; set; }          // FK a Orden
//        public int IdProveedor { get; set; }      // FK a Proveedor
//        public DateTime FechaEntrega { get; set; }

//        // El front puede enviar estos totales, o se pueden recalcular en el backend
//        public decimal Subtotal { get; set; } = 0m;
//        public decimal Iva5 { get; set; } = 0m;
//        public decimal Iva10 { get; set; } = 0m;
//        public decimal Total { get; set; } = 0m;

//        public List<PresupuestoDetalleCreateDTO> Detalles { get; set; }
//            = new List<PresupuestoDetalleCreateDTO>();
//    }

//    public class PresupuestoDetalleCreateDTO
//    {
//        public int IdProducto { get; set; }
//        public int Cantidad { get; set; }
//        public decimal Precio { get; set; }
//        public decimal Iva5 { get; set; }
//        public decimal Iva10 { get; set; }
//    }
//}
