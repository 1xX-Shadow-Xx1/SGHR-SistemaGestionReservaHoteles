
namespace SGHR.Application.Dtos.Configuration.Habitaciones.Habitacion
{
    public class HabitacionDto
    {
        public int Id { get; set; }
        public string CategoriaName { get; set; }

        public int numeroPiso { get; set; }

        public string? AmenityName { get; set; }

        public string Numero { get; set; }

        public int Capacidad { get; set; }

        public string Estado { get; set; }
    }
}
