using System.ComponentModel.DataAnnotations;

namespace SGHR.Application.Dtos.Configuration.Habitaciones.Piso
{
    public class CreatePisoDto
    {
        [Required(ErrorMessage = "El número de piso es obligatorio.")]
        [Range(1, 100, ErrorMessage = "El número de piso debe estar entre 1 y 100.")]
        public int NumeroPiso { get; set; }

        [Required(ErrorMessage = "La descripción del piso es obligatoria.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "La descripción debe tener entre 3 y 100 caracteres.")]
        public string Descripcion { get; set; }

    }
}
