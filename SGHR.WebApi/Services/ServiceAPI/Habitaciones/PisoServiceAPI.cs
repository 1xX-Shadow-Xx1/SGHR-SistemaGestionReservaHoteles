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
        private readonly IClientAPI<PisoModel> _clientAPI;

        public PisoServiceAPI(IPisoRepositoryMemory memory, IClientAPI<PisoModel> clientAPI)
        {
            _memory = memory;
            _clientAPI = clientAPI;
        }

        public ServicesResultModel GetByIDServices(int id)
        {
            return _memory.GetByIDModel(id);
        }

        public List<PisoModel> GetServices()
        {
            return _memory.GetModels();
        }

        public async Task<ServicesResultModel> RemoveServicesPut(int id)
        {
            return await _clientAPI.DeleteAsync($"Piso/Remove-Piso?id={id}");
        }

        public async Task<ServicesResultModel> SaveServicesPost(CreatePisoModel model)
        {
            return await _clientAPI.PostAsync("Piso/Create-Piso", model);
        }

        public async Task<ServicesResultModel> UpdateServicesPut(UpdatePisoModel model)
        {
            return await _clientAPI.PutAsync("Piso/Update-Piso", model);
        }
    }
}
