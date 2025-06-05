//// DTOs/PedidoReadDTO.cs
//using System;
//using System.Collections.Generic;

//namespace ModuloCompras.DTOs
//{
//    public class PedidoDetalleReadDTO
//    {
//        public int IdPedidoDetalle { get; set; }
//        public int IdProducto { get; set; }
//        // Ya no se incluye IdProveedor aquí
//        public decimal Cotizacion { get; set; }
//        public int Cantidad { get; set; }
//        public decimal Iva { get; set; }

//        // Para mostrar datos adicionales (opcional)
//        public string NombreProducto { get; set; }
//        public string NombreProveedor { get; set; }
//    }

//    public class PedidoReadDTO
//    {
//        public int IdPedido { get; set; }
//        public int IdOrden { get; set; }
//        public int IdProveedor { get; set; }      // Sigue en la cabecera
//        public decimal MontoTotal { get; set; }

//        public DateTime FechaPedido { get; set; }
//        public DateTime FechaEntrega { get; set; }
//        public string Estado { get; set; }

//        public List<PedidoDetalleReadDTO> Detalles { get; set; }
//            = new List<PedidoDetalleReadDTO>();
//    }
//}
