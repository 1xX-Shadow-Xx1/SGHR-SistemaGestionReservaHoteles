namespace SGHR.Application.Dtos.Configuration.Operaciones.Auditory
{
    public record UpdateAuditoryDto
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public string TablaAfectada { get; set; }
        public string Operacion { get; set; }
        public DateTime Fecha { get; set; } 
        public string Detalle { get; set; }
    }
}
