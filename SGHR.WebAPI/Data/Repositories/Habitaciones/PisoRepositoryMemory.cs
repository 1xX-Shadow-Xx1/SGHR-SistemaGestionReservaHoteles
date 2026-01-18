using SGHR.Web.Data.Interfaces.Habitaciones;
using SGHR.Web.Data.Repositories.Base;
using SGHR.Web.Models;
using SGHR.Web.Models.Habitaciones.Piso;
using SGHR.Web.Services.ClienteAPIService.Interface;

namespace SGHR.Web.Data.Repositories.Habitaciones
{
    public class PisoRepositoryMemory : BaseRepositoryMemory<PisoModel> , IPisoRepositoryMemory
    {
        public PisoRepositoryMemory(IClientAPI clienteAPI) : base(clienteAPI)
        {
        }

        public override ApiResult<PisoModel> GetByIDModel(int id)
        {
            var result = base.GetByIDModel(id);
            if (result.Success)
                return ApiResult<PisoModel>.Ok(result.Data, "Piso obtenido correctamente.");
            else
                return ApiResult<PisoModel>.Fail(result.StatusCode, "No se encontro un piso con ese id");
        }

        public override List<PisoModel> GetModels()
        {
            return base.GetModels();
        }

        public override async Task<ApiResult<List<PisoModel>>> CheckDataAPI(string endpoint)
        {
            var result = await base.CheckDataAPI(endpoint);
            if (result.Success)
                return ApiResult<List<PisoModel>>.Ok(result.Data, "Lista de pisos actualizada correctamente.");
            else
                return ApiResult<List<PisoModel>>.Fail(result.StatusCode, result.Message);
        }
    }
}
