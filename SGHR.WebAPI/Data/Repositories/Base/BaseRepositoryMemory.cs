using SGHR.Web.Data.Interfaces.Base;
using SGHR.Web.Models;
using SGHR.Web.Models.Base;
using SGHR.Web.Services.ClienteAPIService.Interface;

namespace SGHR.Web.Data.Repositories.Base
{
    public class BaseRepositoryMemory<TModel> : IBaseRepositoryMemory<TModel> where TModel : GetBaseModel
    {
        private readonly IClientAPI _clienteAPI;

        public BaseRepositoryMemory(IClientAPI clienteAPI)
        {
            _clienteAPI = clienteAPI;
        }

        public virtual ApiResult<TModel> GetByIDModel(int id)
        {
            var model = baseModelsData.OfType<TModel>().FirstOrDefault(m => m.Id == id);
            if (model == null)
            {
                return ApiResult<TModel>.Fail(200, "No se encontro ningun modelo con ese id.");
            }
            else
            {
                return ApiResult<TModel>.Ok(model, "Modelo obtenido correctamente.");
            }
        }

        public virtual List<TModel> GetModels()
        {
            return baseModelsData.OfType<TModel>().ToList();
        }

        public virtual async Task<ApiResult<List<TModel>>> CheckDataAPI(string endpoint)
        {
            try
            {
                var result = await _clienteAPI.GetAsync<List<TModel>>(endpoint);

                if (result == null)
                    return result;

                if (!result.Success)
                    return result;

                baseModelsData.Clear();

                if (result.Data is IEnumerable<TModel> lista)
                {
                    baseModelsData.AddRange(lista);
                    return result;
                }
                else
                {
                    return ApiResult<List<TModel>>.Fail(500, "El formato de datos devuelto por la API no es válido.");
                }

            }
            catch (Exception ex)
            {
                return ApiResult<List<TModel>>.Fail(500);
            }

        }

        protected static List<TModel> baseModelsData = new List<TModel>();

    }
}
