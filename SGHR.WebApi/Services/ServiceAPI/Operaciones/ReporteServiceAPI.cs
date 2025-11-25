using SGHR.Web.Data.Interfaces.Operaciones;
using SGHR.Web.Models;
using SGHR.Web.Models.Operaciones.Reporte;
using SGHR.Web.Services.ClienteAPIService.Interface;
using SGHR.Web.Services.Interfaces.Operaciones;

namespace SGHR.Web.Services.ServiceAPI.Operaciones
{
    public class ReporteServiceAPI : IReporteServiceAPI
    {
        private readonly IReporteRepositoryMemory _memory;
        private readonly IClientAPI<ReporteModel> _clientAPI;

        public ReporteServiceAPI(IReporteRepositoryMemory memory, IClientAPI<ReporteModel> clientAPI)
        {
            _memory = memory;
            _clientAPI = clientAPI;
        }

        public ServicesResultModel GetByIDServices(int id)
        {
            return _memory.GetByIDModel(id);
        }

        public List<ReporteModel> GetServices()
        {
            return _memory.GetModels();
        }

        public async Task<ServicesResultModel> RemoveServicesPut(int id)
        {
            return await _clientAPI.DeleteAsync($"Reporte/Remove-Reporte?id={id}");
        }

        public async Task<ServicesResultModel> SaveServicesPost(CreateReporteModel model)
        {
            return await _clientAPI.PostAsync("Reporte/create-Reporte", model);
        }

        public async Task<ServicesResultModel> UpdateServicesPut(UpdateReporteModel model)
        {
            return await _clientAPI.PutAsync("Reporte/update-Reporte", model);
        }
    }
}
