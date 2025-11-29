
using SGHR.Web.Models.Base;
using SGHR.Web.Models.EnumsModel.Habitaciones;
using System.Text.Json.Serialization;

namespace SGHR.Web.Models.Habitaciones.Piso
{
    public class PisoModel : GetBaseModel
    {
        [JsonPropertyName("numeroPiso")]
        public int NumeroPiso { get; set; }
        [JsonPropertyName("descripcion")]
        public string Descripcion { get; set; }
        [JsonPropertyName("estado")]
        public EstadoPisoModel Estado { get; set; }
    }
}
