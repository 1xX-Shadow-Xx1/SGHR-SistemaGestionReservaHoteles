using SGHR.Web.Data;
using SGHR.Web.Models;
using SGHR.Web.Models.Operaciones.Pago;
using SGHR.Web.Models.Sesion;
using SGHR.Web.Services.ClienteAPIService.Interface;
using SGHR.Web.Validador;

namespace SGHR.Web.Services.ClienteAPIService
{
    public class ClienteAPI<T> : IClientAPI<T> where T : class
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ClienteAPI(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ServicesResultModel> DeleteAsync(string endpoint)
        {
            try
            {
                using (HttpClient httpClient = _httpClientFactory.CreateClient("SGHRAPI"))
                {
                    var responsive = await httpClient.PutAsync(endpoint, null);
                    var validate = new ValidateStatusCode().ValidatorStatus((int)responsive.StatusCode, out string errorMessage);
                    if (!validate)
                        return ServicesResultModel.Fail((int)responsive.StatusCode, errorMessage);

                    var result = await new JsonConvertidor<T>().Deserializar(responsive);

                    return result;
                }
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
                using (HttpClient httpClient = _httpClientFactory.CreateClient("SGHRAPI"))
                {
                    var responsive = await httpClient.GetAsync(endpoint);

                    var result = await new JsonConvertidor<T>().Deserializar(responsive);

                    return result;
                }
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

        public async Task<ServicesResultModel> GetListAsync(string endpoint)
        {
            try
            {
                using (HttpClient httpClient = _httpClientFactory.CreateClient("SGHRAPI"))
                {
                    var responsive = await httpClient.GetAsync(endpoint);

                    var result = await new JsonConvertidor<T>().DeserializarList(responsive);

                    return result;
                }
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

        public async Task<ServicesResultModel> GetSesionAsync(string endpoint)
        {
            try
            {
                using (HttpClient httpClient = _httpClientFactory.CreateClient("SGHRAPI"))
                {
                    var responsive = await httpClient.GetAsync(endpoint);

                    var result = await new JsonConvertidor<CheckSesionModel>().Deserializar(responsive);

                    return result;
                }
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
                using (HttpClient httpClient = _httpClientFactory.CreateClient("SGHRAPI"))
                {
                    var responsive = await httpClient.PostAsJsonAsync(endpoint, data);

                    var result = await new JsonConvertidor<T>().Deserializar(responsive);

                    return result;
                }
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
                using (HttpClient httpClient = _httpClientFactory.CreateClient("SGHRAPI"))
                {
                    var responsive = await httpClient.PutAsJsonAsync(endpoint, data);


                    var result = await new JsonConvertidor<T>().Deserializar(responsive);

                    return result;
                }
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
        public async Task<ServicesResultModel> GetResumenPagoAsync(string endpoint)
        {
            try
            {
                using (HttpClient httpClient = _httpClientFactory.CreateClient("SGHRAPI"))
                {
                    var responsive = await httpClient.GetAsync(endpoint);

                    var result = await new JsonConvertidor<ResumenPagoModel>().Deserializar(responsive);

                    return result;
                }
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
