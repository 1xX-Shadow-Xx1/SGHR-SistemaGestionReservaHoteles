using SGHR.Web.Data;
using SGHR.Web.Models;
using SGHR.Web.Services.ClienteAPIService.Interface;
using SGHR.Web.Validador;

namespace SGHR.Web.Services.ClienteAPIService
{
    public class ClienteAPI<T> : IClientAPI<T> where T : class
    {
        private readonly HttpClient _httpClient;

        public ClienteAPI(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("SGHRAPI");        
        }

        public async Task<ServicesResultModel> DeleteAsync(string endpoint)
        {
            try
            {
                var responsive = await _httpClient.PutAsync(endpoint, null);

                var result = await new JsonConvertidor<T>().Deserializar(responsive);

                return result;
            }
            catch (HttpRequestException ex)
            {
                var validate = new ValidateStatusCode().ValidatorStatus(503, out string errorMessage);
                return ServicesResultModel.Fail(503, errorMessage);

            }
            catch (Exception ex)
            {
                var validate = new ValidateStatusCode().ValidatorStatus(500, out string errorMessage);
                return ServicesResultModel.Fail(500, errorMessage);
            }
        }

        public async Task<ServicesResultModel> GetAsync(string endpoint)
        {
            try
            {
                var responsive = await _httpClient.GetAsync(endpoint);

                var result = await new JsonConvertidor<T>().DeserializarList(responsive);

                return result;
            }
            catch (HttpRequestException ex)
            {
                var validate = new ValidateStatusCode().ValidatorStatus(503, out string errorMessage);
                return ServicesResultModel.Fail(503, errorMessage);

            }
            catch (Exception ex)
            {
                var validate = new ValidateStatusCode().ValidatorStatus(500, out string errorMessage);
                return ServicesResultModel.Fail(500, errorMessage);
            }
        }

        public async Task<ServicesResultModel> PostAsync(string endpoint, object? data = null)
        {
            try
            {
                var responsive = await _httpClient.PostAsJsonAsync(endpoint, data);

                var result = await new JsonConvertidor<T>().Deserializar(responsive);

                return result;
            }
            catch (HttpRequestException ex)
            {
                var validate = new ValidateStatusCode().ValidatorStatus(503, out string errorMessage);
                return ServicesResultModel.Fail(503, errorMessage);

            }
            catch (Exception ex)
            {
                var validate = new ValidateStatusCode().ValidatorStatus(500, out string errorMessage);
                return ServicesResultModel.Fail(500, errorMessage);
            }
        }

        public async Task<ServicesResultModel> PutAsync(string endpoint, object? data = null)
        {
            try
            {
                var responsive = await _httpClient.PutAsJsonAsync(endpoint, data);


                var result = await new JsonConvertidor<T>().Deserializar(responsive);

                return result;
            }
            catch (HttpRequestException ex)
            {
                var validate = new ValidateStatusCode().ValidatorStatus(503, out string errorMessage);
                return ServicesResultModel.Fail(503, errorMessage);

            }
            catch (Exception ex)
            {
                var validate = new ValidateStatusCode().ValidatorStatus(500, out string errorMessage);
                return ServicesResultModel.Fail(500, errorMessage);
            }
        }
    }

}
