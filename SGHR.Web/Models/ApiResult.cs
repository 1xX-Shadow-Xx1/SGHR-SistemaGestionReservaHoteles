
using System.Text.Json.Serialization;

namespace SGHR.Web.Models
{
    public class ApiResult<T>
    {
        [JsonPropertyName("success")]
        public bool Success { get; init; }
        [JsonPropertyName("data")]
        public T? Data { get; init; }
        [JsonPropertyName("message")]
        public string? Message { get; init; } = string.Empty;
        public int StatusCode { get; init; }
        public static ApiResult<T> Ok(T data, string? message = null, int statusCode = 200) => new ApiResult<T> { Success = true, Data = data, StatusCode = statusCode, Message = message };
        public static ApiResult<T> Fail(int statusCode = 500, string? message = null) => new ApiResult<T> { Success = false, Message = message, StatusCode = statusCode  };
    }
}