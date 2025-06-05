//// DTOs/PedidoCreateDTO.cs
//using System;
//using System.Collections.Generic;

//namespace ModuloCompras.DTOs
//{
//    public class PedidoCreateDTO
//    {
//        public int IdOrden { get; set; }           // FK a Orden
//        public int IdProveedor { get; set; }       // FK a Proveedor (se define en la cabecera)
//        public decimal MontoTotal { get; set; } = 0m;

//        public DateTime FechaPedido { get; set; }
//        public DateTime FechaEntrega { get; set; }
//        public string Estado { get; set; }

//        // Lista de líneas de detalle para este pedido
//        public List<PedidoDetalleCreateDTO> Detalles { get; set; }
//            = new List<PedidoDetalleCreateDTO>();
//    }

//    public class PedidoDetalleCreateDTO
//    {
//        public int IdProducto { get; set; }
//        // Ya no se incluye IdProveedor aquí (la cabecera Pedido lo tiene)
//        public decimal Cotizacion { get; set; }
//        public int Cantidad { get; set; }
//        public decimal Iva { get; set; }
//    }
//}
