
using System.ComponentModel.DataAnnotations;

namespace SGHR.Application.Dtos.Configuration.Reservas.Tarifa
{
    public class CreateTarifaDto
    {
        [Required(ErrorMessage = "El nombre de la categoría es obligatorio.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El nombre de la categoría debe tener entre 3 y 50 caracteres.")]
        public string NombreCategoria { get; set; }

        [Required(ErrorMessage = "La temporada es obligatoria.")]
        [RegularExpression("^(Alta|Baja|Media)$", ErrorMessage = "La temporada debe ser 'Alta', 'Media' o 'Baja'.")]
        public string Temporada { get; set; }

        [Required(ErrorMessage = "El precio es obligatorio.")]
        [Range(0.01, 999999.99, ErrorMessage = "El precio debe ser mayor a 0 y menor a 1,000,000.")]
        public decimal Precio { get; set; }
    }
}
