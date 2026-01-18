using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using SGHR.Web.Data;
using SGHR.Web.Models;
using SGHR.Web.Services.ClienteAPIService.Interface;

namespace SGHR.Web.Services.ClienteAPIService
{
    public class ClienteAPI : IClientAPI
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ClienteAPI(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        private HttpClient CreateClient() => _httpClientFactory.CreateClient("SGHRAPI");

        public async Task<ApiResult<T>> GetAsync<T>(string endpoint)
        {
            try
            {
                var client = CreateClient();
                using var response = await client.GetAsync(endpoint);

                if (!response.IsSuccessStatusCode)
                {
                    return ApiResult<T>.Fail((int)response.StatusCode);
                }

                return await new JsonConvertidor<T>().Deserializar(response);

            }
            catch (HttpRequestException)
            {
                return ApiResult<T>.Fail(503);
            }
            catch (TaskCanceledException)
            {
                return ApiResult<T>.Fail(504);
            }
            catch (Exception)
            {
                return ApiResult<T>.Fail(500);
            }
        }

        public async Task<ApiResult<TResponse>> PostAsJsonAsync<TRequest, TResponse>(string endpoint, TRequest body)
        {
            try
            {
                var client = CreateClient();
                using var response = await client.PostAsJsonAsync(endpoint, body);

                if (!response.IsSuccessStatusCode)
                {
                    return ApiResult<TResponse>.Fail((int)response.StatusCode);
                }

                return await new JsonConvertidor<TResponse>().Deserializar(response);

            }
            catch (HttpRequestException)
            {
                return ApiResult<TResponse>.Fail(503);
            }
            catch (TaskCanceledException)
            {
                return ApiResult<TResponse>.Fail(504);
            }
            catch (Exception)
            {
                return ApiResult<TResponse>.Fail(500);
            }
        }

        public async Task<ApiResult<TResponse>> PutAsJsonAsync<TRequest, TResponse>(string endpoint, TRequest body )
        {
            try
            {
                var client = CreateClient();
                using var response = await client.PutAsJsonAsync(endpoint, body);

                if (!response.IsSuccessStatusCode)
                {
                    return ApiResult<TResponse>.Fail((int)response.StatusCode);
                }

                return await new JsonConvertidor<TResponse>().Deserializar(response);

            }
            catch (HttpRequestException)
            {
                return ApiResult<TResponse>.Fail(503);
            }
            catch (TaskCanceledException)
            {
                return ApiResult<TResponse>.Fail(504);
            }
            catch (Exception)
            {
                return ApiResult<TResponse>.Fail(500);
            }
        }

        public async Task<ApiResult<T>> DeleteAsync<T>(string endpoint)
        {
            try
            {
                var client = CreateClient();
                using var response = await client.PutAsync(endpoint, null);

                if (!response.IsSuccessStatusCode)
                {
                    return ApiResult<T>.Fail((int)response.StatusCode);
                }

                return await new JsonConvertidor<T>().Deserializar(response);

            }
            catch (HttpRequestException)
            {
                return ApiResult<T>.Fail(503);
            }
            catch (TaskCanceledException)
            {
                return ApiResult<T>.Fail(504);
            }
            catch (Exception)
            {
                return ApiResult<T>.Fail(500);
            }
        }
        public async Task<ApiResult<TResponse>> PostAsync<TResponse>(string endpoint, HttpContent? content = null)
        {
            try
            {
                var client = CreateClient();
                using var response = await client.PostAsync(endpoint, content);

                if (!response.IsSuccessStatusCode)
                {
                    return ApiResult<TResponse>.Fail((int)response.StatusCode);
                }

                return await new JsonConvertidor<TResponse>().Deserializar(response);

            }
            catch (HttpRequestException)
            {
                return ApiResult<TResponse>.Fail(503);
            }
            catch (TaskCanceledException)
            {
                return ApiResult<TResponse>.Fail(504);
            }
            catch (Exception)
            {
                return ApiResult<TResponse>.Fail(500);
            }
        }
        public async Task<ApiResult<TResponse>> PutAsync<TResponse>(string endpoint, HttpContent? content = null)
        {
            try
            {
                var client = CreateClient();
                using var response = await client.PutAsync(endpoint, content);

                if (!response.IsSuccessStatusCode)
                {
                    return ApiResult<TResponse>.Fail((int)response.StatusCode);
                }

                return await new JsonConvertidor<TResponse>().Deserializar(response);

            }
            catch (HttpRequestException)
            {
                return ApiResult<TResponse>.Fail(503);
            }
            catch (TaskCanceledException)
            {
                return ApiResult<TResponse>.Fail(504);
            }
            catch (Exception)
            {
                return ApiResult<TResponse>.Fail(500);
            }
        }
    }

}
