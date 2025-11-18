
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace SGHR.Web.Models.Operaciones.Pago
{
    public class ResumenPagoModel 
    {
        [JsonProperty("totalRecaudado")]
        [JsonPropertyName("totalRecaudado")]
        public decimal TotalRecaudado { get; set; }
        [JsonProperty("totalRechazado")]
        [JsonPropertyName("totalRechazado")]
        public decimal TotalRechazado { get; set; }
        [JsonProperty("pendientes")]
        [JsonPropertyName("pendientes")]
        public int Pendientes { get; set; }
        [JsonProperty("completados")]
        [JsonPropertyName("completados")]
        public int Completados { get; set; }
        [JsonProperty("parciales")]
        [JsonPropertyName("parciales")]
        public int Parciales { get; set; }
        [JsonProperty("rechazados")]
        [JsonPropertyName("rechazados")]
        public int Rechazados { get; set; }
        [JsonProperty("totalPagos")]
        [JsonPropertyName("totalPagos")]
        public int TotalPagos { get; set; }
    }
}
