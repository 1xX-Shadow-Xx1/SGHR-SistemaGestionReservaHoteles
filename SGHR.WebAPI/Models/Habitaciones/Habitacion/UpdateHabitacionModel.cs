using SGHR.Web.Models.EnumsModel.Habitaciones;

namespace SGHR.Web.Models.Habitaciones.Habitacion
{
    public record UpdateHabitacionModel
    {
        public int Id { get; set; }
        public string CategoriaName { get; set; }
        public int NumeroPiso { get; set; }
        public string? AmenityName { get; set; }
        public string Numero { get; set; }
        public int Capacidad { get; set; }
        public EstadoHabitacionModel Estado { get; set; }
    }
}
