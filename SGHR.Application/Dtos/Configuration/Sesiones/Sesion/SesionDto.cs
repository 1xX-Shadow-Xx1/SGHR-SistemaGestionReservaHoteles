namespace SGHR.Application.Dtos.Configuration.Sesiones.Sesion
{
    public class SesionDto
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public bool Estado { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
    }
}
