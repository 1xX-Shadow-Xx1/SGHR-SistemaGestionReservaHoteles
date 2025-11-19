using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace SGHR.Web.Models.Sesion
{
    public class SesionModel
    {
        [JsonProperty("idUser")]
        [JsonPropertyName("idUser")]
        public int IdUser { get; set; }

        [JsonProperty("idsesion")]
        [JsonPropertyName("idsesion")]
        public int Idsesion { get; set; }

        [JsonProperty("estado")]
        [JsonPropertyName("estado")]
        public bool Estado { get; set; }

        [JsonProperty("fechaInicio")]
        [JsonPropertyName("fechaInicio")]
        public DateTime FechaInicio { get; set; }

        [JsonProperty("fechaFin")]
        [JsonPropertyName("fechaFin")]
        public DateTime? FechaFin { get; set; }

        [JsonProperty("ultimaActividad")]
        [JsonPropertyName("ultimaActividad")]
        public DateTime UltimaActividad { get; set; }
    }
}
