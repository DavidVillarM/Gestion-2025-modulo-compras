//// DTOs/PresupuestoCreateDesdeOrdenDto.cs
//using System;
//using System.Collections.Generic;

//namespace ModuloCompras.DTOs
//{
//    public class PresupuestoCreateDesdeOrdenDto
//    {
//        public int IdProveedor { get; set; }
//        public DateTime FechaEntrega { get; set; }

//        // Opcionalmente, se puede ignorar Subtotal/Iva/Total si los calculamos en el servidor.
//        public decimal Subtotal { get; set; } = 0;
//        public decimal Iva5 { get; set; } = 0;
//        public decimal Iva10 { get; set; } = 0;
//        public decimal Total { get; set; } = 0;

//        public List<PresupuestoDetalleDesdeOrdenDto> Detalles { get; set; }
//            = new List<PresupuestoDetalleDesdeOrdenDto>();
//    }

//    public class PresupuestoDetalleDesdeOrdenDto
//    {
//        public int IdProducto { get; set; }
//        public int Cantidad { get; set; }
//        public decimal Precio { get; set; }
//        public decimal Iva5 { get; set; }
//        public decimal Iva10 { get; set; }
//    }
//}
