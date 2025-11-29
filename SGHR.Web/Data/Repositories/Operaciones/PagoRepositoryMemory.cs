using SGHR.Web.Data.Interfaces.Operaciones;
using SGHR.Web.Data.Repositories.Base;
using SGHR.Web.Models;
using SGHR.Web.Models.Operaciones.Pago;
using SGHR.Web.Services.ClienteAPIService.Interface;

namespace SGHR.Web.Data.Repositories.Operaciones
{
    public class PagoRepositoryMemory : BaseRepositoryMemory<PagoModel> , IPagoRepositoryMemory
    {
        public PagoRepositoryMemory(IClientAPI clienteAPI) : base(clienteAPI)
        {
        }

        public override ApiResult<PagoModel> GetByIDModel(int id)
        {
            var result = base.GetByIDModel(id);
            if (result.Success)
                return ApiResult<PagoModel>.Ok(result.Data, "Pago obtenido correctamente.");
            else
                return ApiResult<PagoModel>.Fail(result.StatusCode, "No se encontro un pago con ese id");
        }

        public override List<PagoModel> GetModels()
        {
            return base.GetModels();
        }

        public override async Task<ApiResult<List<PagoModel>>> CheckDataAPI(string endpoint)
        {
            var result = await base.CheckDataAPI(endpoint);
            if (result.Success)
                return ApiResult<List<PagoModel>>.Ok(result.Data, "Lista de pagos actualizada correctamente.");
            else
                return ApiResult<List<PagoModel>>.Fail(result.StatusCode, result.Message);
        }
    }
}
