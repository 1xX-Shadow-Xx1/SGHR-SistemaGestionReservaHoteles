namespace SGHR.Application.Dtos.Configuration.Operaciones.CheckInOut
{
    public record UpdateCheckInOutDto
    {
        public int Id { get; set; }
        public int IdReserva { get; set; }
        public DateTime? FechaCheckIn { get; set; }
        public DateTime? FechaCheckOut { get; set; }
        public int AtendidoPor { get; set; }
    }
}
