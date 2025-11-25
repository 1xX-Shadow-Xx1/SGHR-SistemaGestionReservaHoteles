using SGHR.Web.Data.Interfaces.Reservas;
using SGHR.Web.Models;
using SGHR.Web.Models.Reservas.ServicioAdicional;
using SGHR.Web.Services.ClienteAPIService.Interface;
using SGHR.Web.Services.Interfaces.Reservas;

namespace SGHR.Web.Services.ServiceAPI.Reservas
{
    public class ServicioAdicionalServiceAPI : IServicioAdicionalServiceAPI
    {
        private readonly IClientAPI<ServicioAdicionalModel> _httpAPI;
        private readonly IServicioAdicionalRepositoryMemory _memory;

        public ServicioAdicionalServiceAPI(IClientAPI<ServicioAdicionalModel> httpAPI, IServicioAdicionalRepositoryMemory memory)
        {
            _httpAPI = httpAPI;
            _memory = memory;
        }

        public ServicesResultModel GetByIDServices(int id)
        {
            return _memory.GetByIDModel(id);
        }

        public List<ServicioAdicionalModel> GetServices()
        {
            return _memory.GetModels();
        }

        public async Task<ServicesResultModel> RemoveServicesPut(int id)
        {
            return await _httpAPI.DeleteAsync($"ServicioAdicional/Remove-Servicio-Adicional?id={id}");
        }

        public async Task<ServicesResultModel> SaveServicesPost(CreateServicioAdicionalModel model)
        {
            return await _httpAPI.PostAsync("ServicioAdicional/Create-Servicio-Adicional", model);
        }

        public async Task<ServicesResultModel> UpdateServicesPut(UpdateServicioAdicionalModel model)
        {
            return await _httpAPI.PutAsync("ServicioAdicional/Update-Servicio-Adicional", model);
        }
    }
}
