using System.Text.Json.Serialization;

namespace SGHR.Web.Models.Operaciones.Reporte
{
    public class ReporteModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("tipoReporte")]
        public string TipoReporte { get; set; }
        [JsonPropertyName("fechaGeneracion")]
        public DateTime FechaGeneracion { get; set; }
        [JsonPropertyName("generadoPor")]
        public string GeneradoPor { get; set; }
        [JsonPropertyName("rutaArchivo")]
        public string RutaArchivo { get; set; }
    }
}
