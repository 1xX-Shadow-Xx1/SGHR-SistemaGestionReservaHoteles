using Newtonsoft.Json;
using SGHR.Web.Models;

namespace SGHR.Web.Data
{
    public class JsonConvertidor<TObjet> where TObjet : class
    {

        public JsonConvertidor()
        {
        }

        public async Task<ServicesResultModel> Deserializar(HttpResponseMessage httpResponse)
        {

            try
            {
                string json = await httpResponse.Content.ReadAsStringAsync();

                var resultModel = JsonConvert.DeserializeObject<ServicesResultModel<TObjet>>(json);

                if(resultModel != null && resultModel.Success)
                    return ServicesResultModel.Ok((int)httpResponse.StatusCode, resultModel.Data, resultModel.Message);
                else
                    return ServicesResultModel.Fail((int)httpResponse.StatusCode, resultModel.Message);

            }
            catch (Exception ex)
            {
                return ServicesResultModel.Fail(500, $"Error al deserializar: {ex.Message}");
            }
        }

        public async Task<ServicesResultModel> DeserializarList(HttpResponseMessage httpResponse)
        {
            try
            {
                string json = await httpResponse.Content.ReadAsStringAsync();

                var resultModel = JsonConvert.DeserializeObject<ServicesResultModel<List<TObjet>>>(json);

                if (resultModel != null && resultModel.Success)
                    return ServicesResultModel.Ok((int)httpResponse.StatusCode, resultModel.Data, resultModel.Message);
                else
                    return ServicesResultModel.Fail((int)httpResponse.StatusCode, resultModel.Message);

            }
            catch (Exception ex)
            {
                return ServicesResultModel.Fail(500, $"Error al deserializar: {ex.Message}");
            }

        }

    }
}
