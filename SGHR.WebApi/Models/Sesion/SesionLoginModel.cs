using Newtonsoft.Json;
using SGHR.Web.Models.EnumsModel.Usuario;
using System.Text.Json.Serialization;

namespace SGHR.Web.Models.Sesion
{
    public class SesionLoginModel
    {
        [JsonProperty("idUser")]
        [JsonPropertyName("idUser")]
        public int IdUser { get; set; }
        [JsonProperty("idsesion")]
        [JsonPropertyName("idsesion")]
        public int Idsesion { get; set; }
        [JsonProperty("userName")]
        [JsonPropertyName("userName")]
        public string UserName { get; set; }
        [JsonProperty("rolUser")]
        [JsonPropertyName("rolUser")]
        public RolUsuarioModel RolUser {  get; set; }
    }
}
