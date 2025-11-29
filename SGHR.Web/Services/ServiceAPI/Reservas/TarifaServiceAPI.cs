using SGHR.Web.Data.Interfaces.Reservas;
using SGHR.Web.Models;
using SGHR.Web.Models.Reservas.Tarifa;
using SGHR.Web.Services.ClienteAPIService.Interface;
using SGHR.Web.Services.Interfaces.Reservas;

namespace SGHR.Web.Services.ServiceAPI.Reservas
{
    public class TarifaServiceAPI : ITarifaServiceAPI
    {
        private readonly IClientAPI _httpAPI;
        private readonly ITarifaRepositoryMemory _memory;

        public TarifaServiceAPI(IClientAPI clientAP,
                                 ITarifaRepositoryMemory repositoryMemory)
        {
            _httpAPI = clientAP;
            _memory = repositoryMemory;
        }

        public ApiResult<TarifaModel> GetByIDServices(int id)
        {
            return _memory.GetByIDModel(id);
        }

        public List<TarifaModel> GetServices()
        {
            return _memory.GetModels();
        }

        public async Task<ApiResult<TarifaModel>> RemoveServicesPut(int id)
        {
            return await _httpAPI.DeleteAsync<TarifaModel>($"Tarifa/Remove-Tarifa?id={id}");
        }

        public async Task<ApiResult<TarifaModel>> SaveServicesPost(CreateTarifaModel model)
        {
            return await _httpAPI.PostAsJsonAsync<CreateTarifaModel, TarifaModel>("Tarifa/Create-Tarifa", model);
        }

        public async Task<ApiResult<TarifaModel>> UpdateServicesPut(UpdateTarifaModel model)
        {
            return await _httpAPI.PutAsJsonAsync<UpdateTarifaModel, TarifaModel>("Tarifa/Update-Tarifa", model);
        }
    }
}
