using Newtonsoft.Json;
using SGHR.Web.Models.EnumsModel.Operaciones;
using System.Text.Json.Serialization;

namespace SGHR.Web.Models.Operaciones.Pago
{
    public class PagoModel 
    {
        [JsonProperty("id")]
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonProperty("idReserva")]
        [JsonPropertyName("idReserva")]
        public int IdReserva { get; set; }
        [JsonProperty("monto")]
        [JsonPropertyName("monto")]
        public decimal Monto { get; set; }
        [JsonProperty("metodoPago")]
        [JsonPropertyName("metodoPago")]
        public MetodoPagoModel MetodoPago { get; set; }
        [JsonProperty("fechaPago")]
        [JsonPropertyName("fechaPago")]
        public DateTime FechaPago { get; set; }
        [JsonProperty("estado")]
        [JsonPropertyName("estado")]
        public EstadoPagoModel Estado { get; set; }
    }
}
