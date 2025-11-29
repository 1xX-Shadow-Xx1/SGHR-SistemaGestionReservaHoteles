using SGHR.Web.Data.Interfaces.Reservas;
using SGHR.Web.Data.Repositories.Base;
using SGHR.Web.Models;
using SGHR.Web.Models.Reservas.ServicioAdicional;
using SGHR.Web.Services.ClienteAPIService.Interface;

namespace SGHR.Web.Data.Repositories.Reservas
{
    public class ServicioAdicionalRepositoryMemory : BaseRepositoryMemory<ServicioAdicionalModel> , IServicioAdicionalRepositoryMemory
    {
        public ServicioAdicionalRepositoryMemory(IClientAPI clienteAPI) : base(clienteAPI)
        {
        }

        public override ApiResult<ServicioAdicionalModel> GetByIDModel(int id)
        {
            var result = base.GetByIDModel(id);
            if (result.Success)
                return ApiResult<ServicioAdicionalModel>.Ok(result.Data, "Servicio obtenido correctamente.");
            else
                return ApiResult<ServicioAdicionalModel>.Fail(result.StatusCode, "No se encontro un servicio con ese id");
        }

        public override List<ServicioAdicionalModel> GetModels()
        {
            return base.GetModels();
        }

        public override async Task<ApiResult<List<ServicioAdicionalModel>>> CheckDataAPI(string endpoint)
        {
            var result = await base.CheckDataAPI(endpoint);
            if (result.Success)
                return ApiResult<List<ServicioAdicionalModel>>.Ok(result.Data, "Lista de serivicios actualizada correctamente.");
            else
                return ApiResult<List<ServicioAdicionalModel>>.Fail(result.StatusCode, result.Message);
        }
    }
}
