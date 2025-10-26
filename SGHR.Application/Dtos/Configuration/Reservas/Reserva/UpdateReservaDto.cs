
using System.ComponentModel.DataAnnotations;

namespace SGHR.Application.Dtos.Configuration.Reservas.Reserva
{
    public record UpdateReservaDto
    {
        [Required(ErrorMessage = "El ID de la reserva es obligatorio.")]
        public int Id { get; set; }

        [RegularExpression(@"^\d{3}-\d{7}-\d{1}$", ErrorMessage = "La cédula debe tener el formato 000-0000000-0.")]
        public string? CedulaCliente { get; set; }

        [StringLength(10, MinimumLength = 1, ErrorMessage = "El número de habitación debe tener entre 1 y 10 caracteres.")]
        public string? NumeroHabitacion { get; set; }

        [EmailAddress(ErrorMessage = "El correo electrónico no tiene un formato válido.")]
        [StringLength(100, ErrorMessage = "El correo no puede superar los 100 caracteres.")]
        public string? CorreoCliente { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Formato de fecha inválido.")]
        public DateTime FechaInicio { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Formato de fecha inválido.")]
        public DateTime FechaFin { get; set; }

        [Range(0.01, 999999.99, ErrorMessage = "El costo total debe ser mayor a 0 y menor a 1,000,000.")]
        public decimal CostoTotal { get; set; }

        [RegularExpression("^(Pendiente|Confirmada|Cancelada|Finalizada|Activa)$",
            ErrorMessage = "El estado debe ser 'Pendiente', 'Confirmada', 'Activa' ,'Cancelada' o 'Finalizada'.")]
        public string? Estado { get; set; }
    }
}
