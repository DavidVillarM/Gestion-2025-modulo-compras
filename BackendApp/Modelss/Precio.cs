// Models/Precio.cs
using System;

namespace BackendApp.Modelss
{
    public class Precio
    {
        public int IdPrecio { get; set; }
        public int IdProducto { get; set; }
        public decimal Valor { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string Estado { get; set; }

        public Producto Producto { get; set; }
    }
}