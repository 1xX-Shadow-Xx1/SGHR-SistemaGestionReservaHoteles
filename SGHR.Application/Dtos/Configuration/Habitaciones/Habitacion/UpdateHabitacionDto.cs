using SGHR.Domain.Enum.Habitaciones;
using System.ComponentModel.DataAnnotations;

namespace SGHR.Application.Dtos.Configuration.Habitaciones.Habitacion
{
    public record UpdateHabitacionDto
    {
        [Required(ErrorMessage = "El ID de la habitación es obligatorio.")]
        public int Id { get; set; }

        [StringLength(50, MinimumLength = 3, ErrorMessage = "El nombre de la categoría debe tener entre 3 y 50 caracteres.")]
        public string CategoriaName { get; set; }

        [Range(1, 100, ErrorMessage = "El número de piso debe estar entre 1 y 100.")]
        public int NumeroPiso { get; set; }

        [StringLength(50, ErrorMessage = "El nombre de la amenidad no puede superar los 50 caracteres.")]
        public string? AmenityName { get; set; }

        [RegularExpression(@"^[A-Z]-\d{3}$", ErrorMessage = "Ingrese un número de habitación válido (ej. A-001)")]
        public string Numero { get; set; }

        [Range(1, 10, ErrorMessage = "La capacidad debe estar entre 1 y 10 personas.")]
        public int Capacidad { get; set; }

        [RegularExpression("^(Disponible|Ocupada|Mantenimiento|Reservada|Limpieza)$",
            ErrorMessage = "El estado debe ser 'Disponible', 'Ocupada', 'Limpieza', 'Mantenimiento' o 'Reservada'.")]
        public EstadoHabitacion Estado { get; set; }
    }
}
