
using System.ComponentModel.DataAnnotations;

namespace SGHR.Application.Dtos.Configuration.Habitaciones.Habitacion
{
    public class CreateHabitacionDto
    {
        public string Numero { get; set; }
        public int Capacidad { get; set; }
        public string CategoriaName { get; set; }
        public string? AmenityName { get; set; }
        public int NumeroPiso { get; set; }
    }
}
