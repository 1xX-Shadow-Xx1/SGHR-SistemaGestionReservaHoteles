using SGHR.Domain.Enum.Operaciones;

namespace SGHR.Application.Dtos.Configuration.Operaciones.Mantenimiento
{
    public class MantenimientoDto
    {
        public int Id { get; set; }
        public int? NumeroPiso { get; set; }
        public string NumeroHabitacion { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public string RealizadoPor { get; set; }
        public EstadoMantenimiento Estado { get; set; }
    }
}
