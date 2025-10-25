
namespace SGHR.Application.Dtos.Configuration.Reservas.Reserva
{
    public class ReservaDto
    {
        public int Id { get; set; }
        public string CedulaCliente { get; set; }
        public string NumeroHabitacion { get; set; }
        public string? CorreoCliente { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public decimal CostoTotal { get; set; }
        public string Estado { get; set; } 
    }
}
