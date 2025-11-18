using SGHR.Web.Models.EnumsModel.Reserva;

namespace SGHR.Web.Models.Reservas.Reserva
{
    public record UpdateReservaModel
    {
        public int Id { get; set; }
        public string? CedulaCliente { get; set; }
        public string? NumeroHabitacion { get; set; }
        public string? CorreoCliente { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public EstadoReservaModel? Estado { get; set; }
    }
}
