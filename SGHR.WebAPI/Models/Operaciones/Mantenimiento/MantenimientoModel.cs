using SGHR.Web.Models.Base;
using SGHR.Web.Models.EnumsModel.Operaciones;
using System.Text.Json.Serialization;

namespace SGHR.Web.Models.Operaciones.Mantenimiento
{
    public class MantenimientoModel : GetBaseModel
    {
        [JsonPropertyName("numeroPiso")]
        public int? NumeroPiso { get; set; }
        [JsonPropertyName("numeroHabitacion")]
        public string NumeroHabitacion { get; set; }
        [JsonPropertyName("descripcion")]
        public string Descripcion { get; set; }
        [JsonPropertyName("fechaInicio")]
        public DateTime FechaInicio { get; set; }
        [JsonPropertyName("fechaFin")]
        public DateTime? FechaFin { get; set; }
        [JsonPropertyName("realizadoPor")]
        public string RealizadoPor { get; set; }
        [JsonPropertyName("estado")]
        public EstadoMantenimientoModel Estado { get; set; }
    }
}
