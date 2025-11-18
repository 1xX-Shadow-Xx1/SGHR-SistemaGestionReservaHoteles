namespace SGHR.Web.Models.Operaciones.Reporte
{
    public record UpdateReporteModel
    {
        public int Id { get; set; }
        public string? TipoReporte { get; set; }
        public string? GeneradoPor { get; set; }
        public string? RutaArchivo { get; set; }
    }
}
