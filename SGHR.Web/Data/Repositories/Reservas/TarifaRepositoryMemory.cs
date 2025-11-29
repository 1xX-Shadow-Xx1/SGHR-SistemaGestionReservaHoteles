using SGHR.Web.Data.Interfaces.Reservas;
using SGHR.Web.Data.Repositories.Base;
using SGHR.Web.Models;
using SGHR.Web.Models.Reservas.Tarifa;
using SGHR.Web.Services.ClienteAPIService.Interface;

namespace SGHR.Web.Data.Repositories.Reservas
{
    public class TarifaRepositoryMemory : BaseRepositoryMemory<TarifaModel>, ITarifaRepositoryMemory
    {
        public TarifaRepositoryMemory(IClientAPI clienteAPI) : base(clienteAPI)
        {
        }

        public override ApiResult<TarifaModel> GetByIDModel(int id)
        {
            var result = base.GetByIDModel(id);
            if (result.Success)
                return ApiResult<TarifaModel>.Ok(result.Data, "Tarifa obtenida correctamente.");
            else
                return ApiResult<TarifaModel>.Fail(result.StatusCode, "No se encontro una tarifa con ese id");
        }

        public override  List<TarifaModel> GetModels()
        {
            return base.GetModels();   
        }
        public override async Task<ApiResult<List<TarifaModel>>> CheckDataAPI(string endpoint)
        {
            var result = await base.CheckDataAPI(endpoint);
            if (result.Success)
                return ApiResult<List<TarifaModel>>.Ok(result.Data, "Lista de tarifas actualizada correctamente.");
            else
                return ApiResult<List<TarifaModel>>.Fail(result.StatusCode, result.Message);
        }
    }
}
