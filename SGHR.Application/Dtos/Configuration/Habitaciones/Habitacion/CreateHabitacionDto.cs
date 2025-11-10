
using System.ComponentModel.DataAnnotations;

namespace SGHR.Application.Dtos.Configuration.Habitaciones.Habitacion
{
    public class CreateHabitacionDto
    {
        [Required(ErrorMessage = "El número de la habitación es obligatorio.")]
        [StringLength(5, MinimumLength = 1, ErrorMessage = "El número de la habitación debe tener entre 1 y 5 caracteres.")]
        [RegularExpression(@"^[A-Z]-\d{3}$", ErrorMessage = "Ingrese un número de habitación válido (ej. A-001)")]
        public string Numero { get; set; }

        [Required(ErrorMessage = "La capacidad es obligatoria.")]
        [Range(1, 100, ErrorMessage = "La capacidad debe ser de al menos 1 y no mayor a 100 personas.")]
        public int Capacidad { get; set; }

        [Required(ErrorMessage = "El nombre de la categoría es obligatorio.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El nombre de la categoría debe tener entre 3 y 50 caracteres.")]
        public string CategoriaName { get; set; }

        [StringLength(50, ErrorMessage = "El nombre de la amenidad no puede superar los 50 caracteres.")]
        public string? AmenityName { get; set; }

        [Required(ErrorMessage = "El número de piso es obligatorio.")]
        [Range(1, 100, ErrorMessage = "El número de piso debe estar entre 1 y 100.")]
        public int NumeroPiso { get; set; }
    }
}
