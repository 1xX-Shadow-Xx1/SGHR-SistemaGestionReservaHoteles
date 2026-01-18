

using SGHR.Web.Models.EnumsModel.Operaciones;

namespace SGHR.Web.Models.Operaciones.Mantenimiento
{
    public record UpdateMantenimientoModel
    {
        public int Id { get; set; }
        public int NumeroPiso { get; set; }
        public string NumeroHabitacion { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public string RealizadoPor { get; set; }
        public EstadoMantenimientoModel Estado { get; set; }
    }
}
