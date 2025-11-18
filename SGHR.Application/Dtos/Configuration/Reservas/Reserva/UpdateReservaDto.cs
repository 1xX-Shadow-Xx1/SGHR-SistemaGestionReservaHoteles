
using SGHR.Domain.Enum.Reservas;
using System.ComponentModel.DataAnnotations;

namespace SGHR.Application.Dtos.Configuration.Reservas.Reserva
{
    public record UpdateReservaDto
    {
        public int Id { get; set; }
        public string? CedulaCliente { get; set; }
        public string? NumeroHabitacion { get; set; }
        public string? CorreoCliente { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public EstadoReserva? Estado { get; set; }
    }
}
