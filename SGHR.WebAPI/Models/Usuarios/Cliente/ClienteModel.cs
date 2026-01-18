using SGHR.Web.Models.Base;
using System.Text.Json.Serialization;

namespace SGHR.Web.Models.Usuarios.Cliente
{
    public class ClienteModel : GetBaseModel
    {
        [JsonPropertyName("correo")]
        public string? Correo { get; set; }
        [JsonPropertyName("nombre")]
        public string Nombre { get; set; }
        [JsonPropertyName("apellido")]
        public string Apellido { get; set; }
        [JsonPropertyName("cedula")]
        public string Cedula { get; set; }
        [JsonPropertyName("telefono")]
        public string? Telefono { get; set; }
        [JsonPropertyName("direccion")]
        public string? Direccion { get; set; }
    }
}
