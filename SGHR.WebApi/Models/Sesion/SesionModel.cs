using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace SGHR.Web.Models.Sesion
{
    public class SesionModel
    {
        [JsonProperty("id")]
        [JsonPropertyName("id")]
        public int Idsesion { get; set; }
    }
}
