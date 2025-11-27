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
        private readonly IClientAPI _clientAPI;

        public ReporteServiceAPI(IReporteRepositoryMemory memory, IClientAPI clientAPI)
        {
            _memory = memory;
            _clientAPI = clientAPI;
        }

        public ApiResult<ReporteModel> GetByIDServices(int id)
        {
            return _memory.GetByIDModel(id);
        }

        public List<ReporteModel> GetServices()
        {
            return _memory.GetModels();
        }

        public async Task<ApiResult<ReporteModel>> RemoveServicesPut(int id)
        {
            return await _clientAPI.DeleteAsync<ReporteModel>($"Reporte/Remove-Reporte?id={id}");
        }

        public async Task<ApiResult<ReporteModel>> SaveServicesPost(CreateReporteModel model)
        {
            return await _clientAPI.PostAsJsonAsync<CreateReporteModel, ReporteModel>("Reporte/create-Reporte", model);
        }

        public async Task<ApiResult<ReporteModel>> UpdateServicesPut(UpdateReporteModel model)
        {
            return await _clientAPI.PutAsJsonAsync<UpdateReporteModel, ReporteModel>("Reporte/update-Reporte", model);
        }
    }
}
