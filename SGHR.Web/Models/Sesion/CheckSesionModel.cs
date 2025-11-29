using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace SGHR.Web.Models.Sesion
{
    public class CheckSesionModel
    {
        [JsonProperty("idSesion")]
        [JsonPropertyName("idSesion")]
        public int IdSesion { get; set; }
        [JsonProperty("idUsuario")]
        [JsonPropertyName("idUsuario")]
        public int IdUsuario { get; set; }
        [JsonProperty("estado")]
        [JsonPropertyName("estado")]
        public bool Estado { get; set; }
    }
}
