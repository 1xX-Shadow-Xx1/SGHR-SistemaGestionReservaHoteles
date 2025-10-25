
using System.ComponentModel.DataAnnotations;

namespace SGHR.Application.Dtos.Configuration.Habitaciones.Categoria
{
    public record UpdateCategoriaDto
    {
        [Required(ErrorMessage = "El ID de la categoría es obligatorio.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre de la categoría es obligatorio.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 50 caracteres.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La descripción de la categoría es obligatoria.")]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "La descripción debe tener entre 5 y 200 caracteres.")]
        public string Descripcion { get; set; }
    }
}
