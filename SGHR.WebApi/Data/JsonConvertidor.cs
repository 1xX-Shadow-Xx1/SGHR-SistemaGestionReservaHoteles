using Newtonsoft.Json;
using SGHR.Web.Models;

namespace SGHR.Web.Data
{
    public class JsonConvertidor<TObjet> where TObjet : class
    {

        public JsonConvertidor()
        {
        }

        public async Task<ServicesResultModel<TObjet>> Deserializar(HttpResponseMessage httpResponse)
        {
            var result = new ServicesResultModel<TObjet>();

            try
            {
                string json = await httpResponse.Content.ReadAsStringAsync();

                result = JsonConvert.DeserializeObject<ServicesResultModel<TObjet>>(json);
            }
            catch (Exception ex)
            {
                return new ServicesResultModel<TObjet>
                {
                    Success = false,
                    Message = $"Error al deserializar: {ex.Message}",
                    Data = default
                };
            }

            return result;
        }

        public async Task<ServicesResultModel<List<TObjet>>> DeserializarList(HttpResponseMessage httpResponse)
        {
            var result = new ServicesResultModel<List<TObjet>>();

            try
            {
                string json = await httpResponse.Content.ReadAsStringAsync();

                result = JsonConvert.DeserializeObject<ServicesResultModel<List<TObjet>>>(json);
            }
            catch (Exception ex)
            {
                return new ServicesResultModel<List<TObjet>>
                {
                    Success = false,
                    Message = $"Error al deserializar: {ex.Message}",
                    Data = default
                };
            }

            return result;
        }

    }
}
