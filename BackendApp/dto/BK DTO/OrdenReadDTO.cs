//// DTOs/OrdenReadDTO.cs
//using System;
//using System.Collections.Generic;

//namespace ModuloCompras.DTOs
//{
//    public class OrdenDetalleReadDTO
//    {
//        public int IdOrdenDetalle { get; set; }
//        public int IdProducto { get; set; }
//        public int Cantidad { get; set; }

//        // Para mostrar el nombre del producto (opcional)
//        public string NombreProducto { get; set; }
//    }

//    public class OrdenReadDTO
//    {
//        public int IdOrden { get; set; }
//        public DateTime Fecha { get; set; }      // Se añadió Fecha
//        public string Estado { get; set; }

//        public List<OrdenDetalleReadDTO> Detalles { get; set; }
//            = new List<OrdenDetalleReadDTO>();
//    }
//}
