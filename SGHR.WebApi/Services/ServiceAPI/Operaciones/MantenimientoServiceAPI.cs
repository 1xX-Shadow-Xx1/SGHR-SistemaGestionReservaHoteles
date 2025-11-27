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
        private readonly IClientAPI _clientAPI;

        public MantenimientoServiceAPI(IMantenimientoRepositoryMemory memory, IClientAPI clientAPI)
        {
            _memory = memory;
            _clientAPI = clientAPI;
        }

        public ApiResult<MantenimientoModel> GetByIDServices(int id)
        {
            return _memory.GetByIDModel(id);
        }

        public List<MantenimientoModel> GetServices()
        {
            return _memory.GetModels();
        }

        public async Task<ApiResult<MantenimientoModel>> RemoveServicesPut(int id)
        {
            return await _clientAPI.DeleteAsync<MantenimientoModel>($"Mantenimiento/Remove-Mantenimiento?id={id}");
        }

        public async Task<ApiResult<MantenimientoModel>> SaveServicesPost(CreateMantenimientoModel model)
        {
            return await _clientAPI.PostAsJsonAsync<CreateMantenimientoModel, MantenimientoModel>("Mantenimiento/Create-Mantenimiento", model);
        }

        public async Task<ApiResult<MantenimientoModel>> UpdateServicesPut(UpdateMantenimientoModel model)
        {
            return await _clientAPI.PutAsJsonAsync<UpdateMantenimientoModel, MantenimientoModel>("Mantenimiento/Update-Mantenimiento", model);
        }
    }
}
