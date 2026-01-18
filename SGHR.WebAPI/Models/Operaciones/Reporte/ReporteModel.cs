using SGHR.Web.Models.Base;
using System.Text.Json.Serialization;

namespace SGHR.Web.Models.Operaciones.Reporte
{
    public class ReporteModel : GetBaseModel
    {
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
