using SGHR.Web.Data.Interfaces.Reservas;
using SGHR.Web.Models;
using SGHR.Web.Models.Reservas.Tarifa;
using SGHR.Web.Services.ClienteAPIService.Interface;
using SGHR.Web.Services.Interfaces.Reservas;

namespace SGHR.Web.Services.ServiceAPI.Reservas
{
    public class TarifaServiceAPI : ITarifaServiceAPI
    {
        private readonly IClientAPI<TarifaModel> _httpAPI;
        private readonly ITarifaRepositoryMemory _tarifaMemory;

        public TarifaServiceAPI(IClientAPI<TarifaModel> clientAP,
                                 ITarifaRepositoryMemory repositoryMemory)
        {
            _httpAPI = clientAP;
            _tarifaMemory = repositoryMemory;
        }

        public ServicesResultModel GetByIDServices(int id)
        {
            return _tarifaMemory.GetByIDModel(id);
        }

        public List<TarifaModel> GetServices()
        {
            return _tarifaMemory.GetModels();
        }

        public async Task<ServicesResultModel> RemoveServicesPut(int id)
        {
            return await _httpAPI.DeleteAsync($"Tarifa/Remove-Tarifa?id={id}");
        }

        public async Task<ServicesResultModel> SaveServicesPost(CreateTarifaModel model)
        {
            return await _httpAPI.PostAsync("Tarifa/Create-Tarifa", model);
        }

        public async Task<ServicesResultModel> UpdateServicesPut(UpdateTarifaModel model)
        {
            return await _httpAPI.PutAsync("Tarifa/Update-Tarifa", model);
        }
    }
}
