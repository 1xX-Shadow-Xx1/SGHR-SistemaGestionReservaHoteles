namespace SGHR.Application.Dtos.Configuration.Sesiones.Sesion
{
    public class SesionDto
    {
        public int IdUser { get; set; }
        public int Idsesion { get; set; }
        public bool Estado { get; set; }
        public DateTime FechaInicio { get; set; } 
        public DateTime? FechaFin { get; set; } 
        public DateTime UltimaActividad { get; set; } 
    }
}
