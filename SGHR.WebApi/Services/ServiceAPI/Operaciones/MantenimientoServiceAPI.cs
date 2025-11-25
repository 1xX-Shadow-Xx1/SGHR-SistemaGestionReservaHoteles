using SGHR.Web.Data.Interfaces.Operaciones;
using SGHR.Web.Models;
using SGHR.Web.Models.Operaciones.Mantenimiento;
using SGHR.Web.Services.ClienteAPIService.Interface;
using SGHR.Web.Services.Interfaces.Operaciones;

namespace SGHR.Web.Services.ServiceAPI.Operaciones
{
    public class MantenimientoServiceAPI : IMantenimientoServiceAPI
    {
        private readonly IMantenimientoRepositoryMemory _memory;
        private readonly IClientAPI<MantenimientoModel> _clientAPI;

        public MantenimientoServiceAPI(IMantenimientoRepositoryMemory memory, IClientAPI<MantenimientoModel> clientAPI)
        {
            _memory = memory;
            _clientAPI = clientAPI;
        }

        public ServicesResultModel GetByIDServices(int id)
        {
            return _memory.GetByIDModel(id);
        }

        public List<MantenimientoModel> GetServices()
        {
            return _memory.GetModels();
        }

        public async Task<ServicesResultModel> RemoveServicesPut(int id)
        {
            return await _clientAPI.DeleteAsync($"Mantenimiento/Remove-Mantenimiento?id={id}");
        }

        public async Task<ServicesResultModel> SaveServicesPost(CreateMantenimientoModel model)
        {
            return await _clientAPI.PostAsync("Mantenimiento/Create-Mantenimiento", model);
        }

        public async Task<ServicesResultModel> UpdateServicesPut(UpdateMantenimientoModel model)
        {
            return await _clientAPI.PutAsync("Mantenimiento/Update-Mantenimiento", model);
        }
    }
}
