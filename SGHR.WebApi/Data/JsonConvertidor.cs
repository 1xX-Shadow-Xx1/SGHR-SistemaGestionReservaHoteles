using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SGHR.Web.Models;
using System.Text.Json;

namespace SGHR.Web.Data
{
    public class JsonConvertidor<TObjet> 
    {
        private readonly JsonSerializerOptions _jsonOptions;
        public JsonConvertidor()
        {
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

        }


        public async Task<ApiResult<TObjet>> Deserializar(HttpResponseMessage httpResponse)
        {

            try
            {
                string json = await httpResponse.Content.ReadAsStringAsync();

                var apiResult = System.Text.Json.JsonSerializer.Deserialize<ApiResult<TObjet>>(json, _jsonOptions);

                if (apiResult != null && apiResult.Success)
                    return ApiResult<TObjet>.Ok( apiResult.Data, apiResult.Message);
                else
                    return ApiResult<TObjet>.Fail((int)httpResponse.StatusCode, apiResult.Message);

            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error interno.", ex);
            }
        }

    }
}
