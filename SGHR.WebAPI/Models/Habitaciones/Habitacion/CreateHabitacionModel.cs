namespace SGHR.Web.Models.Habitaciones.Habitacion
{
    public class CreateHabitacionModel
    {
        public string Numero { get; set; }
        public int Capacidad { get; set; }
        public string CategoriaName { get; set; }
        public string? AmenityName { get; set; }
        public int NumeroPiso { get; set; }
    }
}
