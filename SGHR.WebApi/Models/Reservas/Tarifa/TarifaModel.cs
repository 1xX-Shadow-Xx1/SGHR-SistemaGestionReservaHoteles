using System.Text.Json.Serialization;

namespace SGHR.Web.Models.Reservas.Tarifa
{
    public class TarifaModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("nombreCategoria")]
        public string NombreCategoria { get; set; }
        [JsonPropertyName("fecha_inicio")]
        public DateTime Fecha_inicio { get; set; }
        [JsonPropertyName("fecha_fin")]
        public DateTime Fecha_fin { get; set; }
        [JsonPropertyName("precio")]
        public decimal Precio { get; set; }
    }
}
