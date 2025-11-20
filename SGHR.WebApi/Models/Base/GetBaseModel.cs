using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace SGHR.Web.Models.Base
{
    public abstract class GetBaseModel
    {
        [JsonProperty("id")]
        [JsonPropertyName("id")]
        public int Id { get; set; }
    }
}
