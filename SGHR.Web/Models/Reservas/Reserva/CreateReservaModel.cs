namespace SGHR.Web.Models.Reservas.Reserva
{
    public class CreateReservaModel
    {
        public string CedulaCliente { get; set; }
        public string NumeroHabitacion { get; set; }
        public string? CorreoCliente { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
    }
}
