using Newtonsoft.Json;
using SGHR.Web.Models.EnumsModel.Reserva;
using System.Text.Json.Serialization;

namespace SGHR.Web.Models.Reservas.Reserva
{
    public class ReservaModel 
    {
        [JsonProperty("id")]
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonProperty("cedulaCliente")]
        [JsonPropertyName("cedulaCliente")]
        public string CedulaCliente { get; set; }
        [JsonProperty("numeroHabitacion")]
        [JsonPropertyName("numeroHabitacion")]
        public string NumeroHabitacion { get; set; }
        [JsonProperty("correoCliente")]
        [JsonPropertyName("correoCliente")]
        public string CorreoCliente { get; set; }
        [JsonProperty("fechaInicio")]
        [JsonPropertyName("fechaInicio")]
        public DateTime FechaInicio { get; set; }
        [JsonProperty("fechaFin")]
        [JsonPropertyName("fechaFin")]
        public DateTime FechaFin { get; set; }
        [JsonProperty("costoTotal")]
        [JsonPropertyName("costoTotal")]
        public decimal CostoTotal { get; set; }
        [JsonProperty("estado")]
        [JsonPropertyName("estado")]
        public EstadoReservaModel Estado { get; set; }
    }
}
