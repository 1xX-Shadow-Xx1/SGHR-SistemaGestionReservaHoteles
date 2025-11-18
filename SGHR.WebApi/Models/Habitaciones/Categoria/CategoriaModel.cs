using System.Text.Json.Serialization;

namespace SGHR.Web.Models.Habitaciones.Categoria
{
    public class CategoriaModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("nombre")]
        public string Nombre { get; set; }
        [JsonPropertyName("descripcion")]
        public string Descripcion { get; set; }
        [JsonPropertyName("precio")]
        public decimal Precio { get; set; }
    }
}
