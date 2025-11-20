using SGHR.Web.Models.Base;
using SGHR.Web.Models.EnumsModel.Reserva;
using System.Text.Json.Serialization;

namespace SGHR.Web.Models.Reservas.ServicioAdicional
{
    public class ServicioAdicionalModel : GetBaseModel
    {
        [JsonPropertyName("nombre")]
        public string Nombre { get; set; }
        [JsonPropertyName("descripcion")]
        public string Descripcion { get; set; }
        [JsonPropertyName("precio")]
        public decimal Precio { get; set; }
        [JsonPropertyName("estado")]
        public EstadoServicioAdicionalModel Estado { get; set; }
    }
}
