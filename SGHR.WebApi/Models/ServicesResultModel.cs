
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

    public class ServicesResultModel
    {
        public int Statuscode { get; set; }
        public bool Success { get; set; }
        public string? Message { get; set; }
        public dynamic? Data { get; set; }


        public static ServicesResultModel Ok(int statuscode, dynamic? data = null, string? message = null ) => new ServicesResultModel { Success = true, Statuscode = statuscode, Message = message, Data = data };
        public static ServicesResultModel Fail(int statuscode, string message) => new ServicesResultModel { Success = false, Statuscode = statuscode, Message = message };
    }
}