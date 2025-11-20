using SGHR.Web.Models.Base;
using SGHR.Web.Models.EnumsModel.Habitaciones;
using System.Text.Json.Serialization;

namespace SGHR.Web.Models.Habitaciones.Habitacion
{
    public class HabitacionModel : GetBaseModel
    {
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
