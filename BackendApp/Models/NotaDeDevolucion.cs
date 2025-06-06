using System;

namespace BackendApp.Models
{
    public class NotaDeDevolucion
    {
        public int Id { get; set; }
        public int OrdenId { get; set; }
        public int ProductoId { get; set; }

        public string Motivo { get; set; }
        public DateTime Fecha { get; set; }
    }
}
