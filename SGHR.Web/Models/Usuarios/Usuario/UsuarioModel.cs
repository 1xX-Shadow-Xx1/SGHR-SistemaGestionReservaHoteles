using Newtonsoft.Json;
using SGHR.Web.Models.Base;
using SGHR.Web.Models.EnumsModel.Usuario;
using System.Text.Json.Serialization;

namespace SGHR.Web.Models.Usuarios.Usuario
{
    public class UsuarioModel : GetBaseModel
    {
        [JsonPropertyName("nombre")]
        public string Nombre { get; set; } = null!;
        [JsonPropertyName("correo")]
        public string Correo { get; set; } = null!;
        [JsonProperty("contraseña")]
        [JsonPropertyName("contraseña")]
        public string Contraseña { get; set; } = null!;
        [JsonPropertyName("rol")]
        public RolUsuarioModel Rol { get; set; }
        [JsonPropertyName("estado")]
        public EstadoUsuarioModel Estado { get; set; } 
    }
}
