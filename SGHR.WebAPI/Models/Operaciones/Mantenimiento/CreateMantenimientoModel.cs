namespace SGHR.Web.Models.Operaciones.Mantenimiento
{
    public class CreateMantenimientoModel
    {
        public int NumeroPiso { get; set; }
        public string NumeroHabitacion { get; set; }
        public string Descripcion { get; set; }
        public string RealizadoPor { get; set; }
        public DateTime FechaInicio { get; set; }
    }
}
