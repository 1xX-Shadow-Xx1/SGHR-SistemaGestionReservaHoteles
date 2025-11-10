using SGHR.Domain.Enum.Habitaciones;
using System.ComponentModel.DataAnnotations;

namespace SGHR.Application.Dtos.Configuration.Habitaciones.Piso
{
    public record UpdatePisoDto
    {
        [Required(ErrorMessage = "El ID del piso es obligatorio.")]
        public int Id { get; set; }

        [Range(0, 100, ErrorMessage = "El número de piso debe estar entre 0 y 100.")]
        public int NumeroPiso { get; set; }

        [StringLength(100, MinimumLength = 3, ErrorMessage = "La descripción debe tener entre 3 y 100 caracteres.")]
        public string? Descripcion { get; set; }

        public EstadoPiso Estado { get; set; }
    }
}
