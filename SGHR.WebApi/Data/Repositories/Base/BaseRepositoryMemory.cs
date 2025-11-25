using SGHR.Web.Data.Interfaces.Base;
using SGHR.Web.Models;
using SGHR.Web.Models.Base;
using SGHR.Web.Services.ClienteAPIService.Interface;
using SGHR.Web.Validador;

namespace SGHR.Web.Data.Repositories.Base
{
    public class BaseRepositoryMemory<TModel> : IBaseRepositoryMemory<TModel> where TModel : GetBaseModel
    {
        private readonly IClientAPI<TModel> _clienteAPI;

        public BaseRepositoryMemory(IClientAPI<TModel> clienteAPI)
        {
            _clienteAPI = clienteAPI;
        }

        public virtual ServicesResultModel GetByIDModel(int id)
        {
            var model = baseModelsData.OfType<TModel>().FirstOrDefault(m => m.Id == id);
            if (model == null)
            {
                return ServicesResultModel.Fail(200, "No se encontro ningun modelo con ese id.");
            }
            else
            {
                return ServicesResultModel.Ok(200, model, "Modelo obtenido correctamente.");
            }
        }


        public virtual List<TModel> GetModels()
        {
            return baseModelsData.OfType<TModel>().ToList();
        }

        public virtual async Task<ServicesResultModel> CheckDataAPI(string endpoint)
        {
            try
            {
                var result = await _clienteAPI.GetListAsync(endpoint);

                if (result == null)
                    return ServicesResultModel.Fail(500, "No hubo respuesta de la API.");

                if (!result.Success)
                    return ServicesResultModel.Fail(result.Statuscode, result.Message);

                baseModelsData.Clear();

                if (result.Data is IEnumerable<TModel> lista)
                {
                    baseModelsData.AddRange(lista);
                    return ServicesResultModel.Ok(result.Statuscode);
                }
                else
                {
                    return ServicesResultModel.Fail(500, "El formato de datos devuelto por la API no es válido.");
                }

            }
            catch (HttpRequestException ex)
            {
                var validate = new ValidateStatusCode().ValidatorStatus(503, out string errorMessage);
                return ServicesResultModel.Fail(503, errorMessage);
            }catch (Exception ex)
            {
                var validate = new ValidateStatusCode().ValidatorStatus(500, out string errorMessage);
                return ServicesResultModel.Fail(500, errorMessage);
            }
            
        }



        protected static List<TModel> baseModelsData = new List<TModel>();
    }
}
