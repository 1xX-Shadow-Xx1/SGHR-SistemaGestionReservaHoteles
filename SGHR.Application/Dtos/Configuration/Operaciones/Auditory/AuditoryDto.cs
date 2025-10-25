namespace SGHR.Application.Dtos.Configuration.Operaciones.Auditory
{
    public class AuditoryDto
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public int? IdSesion { get; set; }
        public string Operacion { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;
        public string Detalle { get; set; }
    }
}
