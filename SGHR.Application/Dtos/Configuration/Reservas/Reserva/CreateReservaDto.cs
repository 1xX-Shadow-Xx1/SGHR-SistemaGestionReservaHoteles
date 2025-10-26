
using System.ComponentModel.DataAnnotations;

namespace SGHR.Application.Dtos.Configuration.Reservas.Reserva
{
    public class CreateReservaDto
    {
        [Required(ErrorMessage = "La cédula del cliente es obligatoria.")]
        [RegularExpression(@"^\d{3}-\d{7}-\d{1}$", ErrorMessage = "La cédula debe tener el formato 000-0000000-0.")]
        public string CedulaCliente { get; set; }

        [Required(ErrorMessage = "El número de habitación es obligatorio.")]
        [StringLength(10, MinimumLength = 1, ErrorMessage = "El número de habitación debe tener entre 1 y 10 caracteres.")]
        public string NumeroHabitacion { get; set; }

        [EmailAddress(ErrorMessage = "El correo electrónico no tiene un formato válido.")]
        [StringLength(100, ErrorMessage = "El correo no puede superar los 100 caracteres.")]
        public string? CorreoCliente { get; set; }

        [Required(ErrorMessage = "La fecha de inicio es obligatoria.")]
        [DataType(DataType.Date, ErrorMessage = "Formato de fecha inválido.")]
        public DateTime FechaInicio { get; set; }

        [Required(ErrorMessage = "La fecha de fin es obligatoria.")]
        [DataType(DataType.Date, ErrorMessage = "Formato de fecha inválido.")]
        public DateTime FechaFin { get; set; }
    }
}
