using SGHR.Web.Data.Interfaces.Operaciones;
using SGHR.Web.Data.Repositories.Base;
using SGHR.Web.Models;
using SGHR.Web.Models.Operaciones.Mantenimiento;
using SGHR.Web.Services.ClienteAPIService.Interface;

namespace SGHR.Web.Data.Repositories.Operaciones
{
    public class MantenimientoRepositoryMemory : BaseRepositoryMemory<MantenimientoModel> , IMantenimientoRepositoryMemory
    {
        public MantenimientoRepositoryMemory(IClientAPI clienteAPI) : base(clienteAPI)
        {
        }

        public override ApiResult<MantenimientoModel> GetByIDModel(int id)
        {
            var result = base.GetByIDModel(id);
            if (result.Success)
                return ApiResult<MantenimientoModel>.Ok(result.Data, "Mantenimiento obtenido correctamente.");
            else
                return ApiResult<MantenimientoModel>.Fail(result.StatusCode, "No se encontro un mantenimiento con ese id");
        }

        public override List<MantenimientoModel> GetModels()
        {
            return base.GetModels();
        }

        public override async Task<ApiResult<List<MantenimientoModel>>> CheckDataAPI(string endpoint)
        {
            var result = await base.CheckDataAPI(endpoint);
            if (result.Success)
                return ApiResult<List<MantenimientoModel>>.Ok(result.Data, "Lista de mantenimientos actualizada correctamente.");
            else
                return ApiResult<List<MantenimientoModel>>.Fail(result.StatusCode, result.Message);
        }
    }
}
