using SGHR.Web.Data.Interfaces.Operaciones;
using SGHR.Web.Data.Repositories.Base;
using SGHR.Web.Models;
using SGHR.Web.Models.Operaciones.Reporte;
using SGHR.Web.Models.Reservas.Reserva;
using SGHR.Web.Services.ClienteAPIService.Interface;

namespace SGHR.Web.Data.Repositories.Operaciones
{
    public class ReporteRepositoryMemory : BaseRepositoryMemory<ReporteModel> , IReporteRepositoryMemory
    {
        public ReporteRepositoryMemory(IClientAPI clienteAPI) : base(clienteAPI)
        {
        }

        public override ApiResult<ReporteModel> GetByIDModel(int id)
        {
            var result = base.GetByIDModel(id);
            if (result.Success)
                return ApiResult<ReporteModel>.Ok( result.Data, "Reporte obtenido correctamente.");
            else
                return ApiResult<ReporteModel>.Fail(result.StatusCode, "No se encontro un reporte con ese id");
        }

        public override List<ReporteModel> GetModels()
        {
            return base.GetModels();
        }

        public override async Task<ApiResult<List<ReporteModel>>> CheckDataAPI(string endpoint)
        {
            var result = await base.CheckDataAPI(endpoint);
            if (result.Success)
                return ApiResult<List<ReporteModel>>.Ok(result.Data, "Lista de reportes actualizada correctamente.");
            else
                return ApiResult<List<ReporteModel>>.Fail(result.StatusCode, result.Message);
        }
    }
}
