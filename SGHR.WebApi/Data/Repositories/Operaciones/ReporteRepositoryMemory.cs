using SGHR.Web.Data.Interfaces.Operaciones;
using SGHR.Web.Data.Repositories.Base;
using SGHR.Web.Models;
using SGHR.Web.Models.Operaciones.Reporte;
using SGHR.Web.Services.ClienteAPIService.Interface;

namespace SGHR.Web.Data.Repositories.Operaciones
{
    public class ReporteRepositoryMemory : BaseRepositoryMemory<ReporteModel> , IReporteRepositoryMemory
    {
        public ReporteRepositoryMemory(IClientAPI<ReporteModel> clienteAPI) : base(clienteAPI)
        {
        }

        public override ServicesResultModel GetByIDModel(int id)
        {
            var result = base.GetByIDModel(id);
            if (result.Success)
                return ServicesResultModel.Ok(result.Statuscode, result.Data, "Reporte obtenido correctamente.");
            else
                return ServicesResultModel.Fail(result.Statuscode, "No se encontro un reporte con ese id");
        }

        public override List<ReporteModel> GetModels()
        {
            return base.GetModels();
        }

        public override async Task<ServicesResultModel> CheckDataAPI(string endpoint)
        {
            var result = await base.CheckDataAPI(endpoint);
            if (result.Success)
                return ServicesResultModel.Ok(result.Statuscode, "Lista de reportes actualizada correctamente.");
            else
                return ServicesResultModel.Fail(result.Statuscode, result.Message);
        }
    }
}
