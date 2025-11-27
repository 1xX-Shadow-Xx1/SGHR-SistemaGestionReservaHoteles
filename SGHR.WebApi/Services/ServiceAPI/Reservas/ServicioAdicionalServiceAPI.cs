using SGHR.Web.Data.Interfaces.Reservas;
using SGHR.Web.Models;
using SGHR.Web.Models.Reservas.ServicioAdicional;
using SGHR.Web.Models.Reservas.Tarifa;
using SGHR.Web.Services.ClienteAPIService.Interface;
using SGHR.Web.Services.Interfaces.Reservas;

namespace SGHR.Web.Services.ServiceAPI.Reservas
{
    public class ServicioAdicionalServiceAPI : IServicioAdicionalServiceAPI
    {
        private readonly IClientAPI _httpAPI;
        private readonly IServicioAdicionalRepositoryMemory _memory;

        public ServicioAdicionalServiceAPI(IClientAPI httpAPI, IServicioAdicionalRepositoryMemory memory)
        {
            _httpAPI = httpAPI;
            _memory = memory;
        }

        public ApiResult<ServicioAdicionalModel> GetByIDServices(int id)
        {
            return _memory.GetByIDModel(id);
        }

        public List<ServicioAdicionalModel> GetServices()
        {
            return _memory.GetModels();
        }

        public async Task<ApiResult<ServicioAdicionalModel>> RemoveServicesPut(int id)
        {
            return await _httpAPI.DeleteAsync<ServicioAdicionalModel>($"ServicioAdicional/Remove-Servicio-Adicional?id={id}");
        }

        public async Task<ApiResult<ServicioAdicionalModel>> SaveServicesPost(CreateServicioAdicionalModel model)
        {
            return await _httpAPI.PostAsJsonAsync<CreateServicioAdicionalModel, ServicioAdicionalModel>("ServicioAdicional/Create-Servicio-Adicional", model);
        }

        public async Task<ApiResult<ServicioAdicionalModel>> UpdateServicesPut(UpdateServicioAdicionalModel model)
        {
            return await _httpAPI.PutAsJsonAsync<UpdateServicioAdicionalModel, ServicioAdicionalModel>("ServicioAdicional/Update-Servicio-Adicional", model);
        }
    }
}
