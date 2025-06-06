//// DTOs/PresupuestoReadParaOrdenDto.cs
//using System;
//using System.Collections.Generic;

//namespace ModuloCompras.DTOs
//{
//    public class PresupuestoReadParaOrdenDto
//    {
//        public int IdPresupuesto { get; set; }
//        public int IdProveedor { get; set; }
//        public string NombreProveedor { get; set; }
//        public DateTime FechaEntrega { get; set; }
//        public decimal Subtotal { get; set; }
//        public decimal Iva5 { get; set; }
//        public decimal Iva10 { get; set; }
//        public decimal Total { get; set; }

//        public List<PresupuestoDetalleReadDto> Detalles { get; set; }
//            = new List<PresupuestoDetalleReadDto>();
//    }

//    public class PresupuestoDetalleReadDto
//    {
//        public int IdPresupuestoDetalle { get; set; }
//        public int IdProducto { get; set; }
//        public int Cantidad { get; set; }
//        public decimal Precio { get; set; }
//        public decimal Iva5 { get; set; }
//        public decimal Iva10 { get; set; }
//        public string NombreProducto { get; set; }
//    }
//}
