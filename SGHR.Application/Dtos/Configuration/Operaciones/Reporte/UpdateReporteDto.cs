namespace SGHR.Application.Dtos.Configuration.Operaciones.Reporte
{
    public record UpdateReporteDto
    {
        public int Id { get; set; }
        public string TipoReporte { get; set; }
        public string GeneradoPor { get; set; }
        public string RutaArchivo { get; set; }
    }
}
