
using System.Text.Json.Serialization;

namespace SGHR.Web.Models
{
    public class ServicesResultModel<TModel> where TModel : class
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }
        [JsonPropertyName("message")]
        public string? Message { get; set; }
        [JsonPropertyName("data")]
        public TModel? Data { get; set; }
    }
}
