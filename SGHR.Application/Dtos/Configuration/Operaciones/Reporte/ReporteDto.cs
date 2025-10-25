
namespace SGHR.Application.Dtos.Configuration.Operaciones.Reporte
{
    public class ReporteDto
    {
        public int Id { get; set; }
        public string TipoReporte { get; set; }
        public DateTime FechaGeneracion { get; set; } 
        public int GeneradoPor { get; set; }
        public string RutaArchivo { get; set; }
    }
}
