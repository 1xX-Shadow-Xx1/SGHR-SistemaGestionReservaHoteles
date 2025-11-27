using SGHR.Web.Models;

namespace SGHR.Web.Services.ClienteAPIService.Interface
{
    public interface IClientAPI
    {
        Task<ApiResult<T>> GetAsync<T>(string endpoint);
        Task<ApiResult<TResponse>> PostAsJsonAsync<TRequest, TResponse>(string endpoint, TRequest body);
        Task<ApiResult<TResponse>> PutAsJsonAsync<TRequest, TResponse>(string endpoint, TRequest body);
        Task<ApiResult<TResponse>> PostAsync<TResponse>(string endpoint, HttpContent? content = null);
        Task<ApiResult<TResponse>> PutAsync<TResponse>(string endpoint, HttpContent? content = null);
        Task<ApiResult<T>> DeleteAsync<T>(string endpoint);
    }

}