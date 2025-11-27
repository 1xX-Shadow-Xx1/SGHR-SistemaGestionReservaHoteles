using SGHR.Web.Data.Interfaces.Habitaciones;
using SGHR.Web.Models;
using SGHR.Web.Models.Habitaciones.Piso;
using SGHR.Web.Services.ClienteAPIService.Interface;
using SGHR.Web.Services.Interfaces.Habitaciones;

namespace SGHR.Web.Services.ServiceAPI.Habitaciones
{
    public class PisoServiceAPI : IPisoServiceAPI
    {
        private readonly IPisoRepositoryMemory _memory;
        private readonly IClientAPI _clientAPI;

        public PisoServiceAPI(IPisoRepositoryMemory memory, IClientAPI clientAPI)
        {
            _memory = memory;
            _clientAPI = clientAPI;
        }

        public ApiResult<PisoModel> GetByIDServices(int id)
        {
            return _memory.GetByIDModel(id);
        }

        public List<PisoModel> GetServices()
        {
            return _memory.GetModels();
        }

        public async Task<ApiResult<PisoModel>> RemoveServicesPut(int id)
        {
            return await _clientAPI.DeleteAsync<PisoModel>($"Piso/Remove-Piso?id={id}");
        }

        public async Task<ApiResult<PisoModel>> SaveServicesPost(CreatePisoModel model)
        {
            return await _clientAPI.PostAsJsonAsync<CreatePisoModel, PisoModel>("Piso/Create-Piso", model);
        }

        public async Task<ApiResult<PisoModel>> UpdateServicesPut(UpdatePisoModel model)
        {
            return await _clientAPI.PutAsJsonAsync<UpdatePisoModel, PisoModel>("Piso/Update-Piso", model);
        }
    }
}
