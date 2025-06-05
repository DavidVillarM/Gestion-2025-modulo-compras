using System.ComponentModel.DataAnnotations;

namespace ModuloCompras.DTOs
{
    public class CategoriaCreateDto
    {
        [Required] public string Nombre { get; set; }
        public string Descripcion { get; set; }
    }
}
