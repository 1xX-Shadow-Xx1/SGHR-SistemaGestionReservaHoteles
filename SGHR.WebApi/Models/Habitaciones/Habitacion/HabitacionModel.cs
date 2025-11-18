using SGHR.Web.Models.EnumsModel.Habitaciones;
using System.Text.Json.Serialization;

namespace SGHR.Web.Models.Habitaciones.Habitacion
{
    public class HabitacionModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("categoriaName")]
        public string CategoriaName { get; set; }
        [JsonPropertyName("numeroPiso")]
        public int NumeroPiso { get; set; }
        [JsonPropertyName("amenityName")]
        public string? AmenityName { get; set; }
        [JsonPropertyName("numero")]
        public string Numero { get; set; }
        [JsonPropertyName("capacidad")]
        public int Capacidad { get; set; }
        [JsonPropertyName("estado")]
        public EstadoHabitacionModel Estado { get; set; }
    }
}
