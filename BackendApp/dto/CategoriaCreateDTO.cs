using System.ComponentModel.DataAnnotations;

namespace BackendApp.DTOs
{
    public class CategoriaCreateDto
    {
        [Required] public string Nombre { get; set; }
        public string Descripcion { get; set; }
    }
}
