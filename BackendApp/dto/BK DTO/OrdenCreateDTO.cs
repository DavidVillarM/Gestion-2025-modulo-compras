//// DTOs/OrdenCreateDTO.cs
//using System;
//using System.Collections.Generic;

//namespace ModuloCompras.DTOs
//{
//    public class OrdenCreateDTO
//    {
//        // Se añadió Fecha (la entidad Orden ahora la exige)
//        public DateTime Fecha { get; set; }

//        //public string Estado { get; set; }

//        // Lista de detalles para esta orden
//        public List<OrdenDetalleCreateDTO> OrdenDetalles { get; set; }
//            = new List<OrdenDetalleCreateDTO>();
//    }

//    public class OrdenDetalleCreateDTO
//    {
//        public int IdProducto { get; set; }
//        public int Cantidad { get; set; }
//    }
//}
