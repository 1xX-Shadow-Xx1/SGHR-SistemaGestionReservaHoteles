using SGHR.Web.Data.Interfaces.Reservas;
using SGHR.Web.Data.Repositories.Base;
using SGHR.Web.Models;
using SGHR.Web.Models.Reservas.Reserva;
using SGHR.Web.Services.ClienteAPIService.Interface;

namespace SGHR.Web.Data.Repositories.Reservas
{
    public class ReservaRepositoryMemory : BaseRepositoryMemory<ReservaModel> , IReservaRepositoryMemory
    {
        public ReservaRepositoryMemory(IClientAPI clienteAPI) : base(clienteAPI)
        {
        }

        public override ApiResult<ReservaModel> GetByIDModel(int id)
        {
            var result = base.GetByIDModel(id);
            if (result.Success)
                return ApiResult<ReservaModel>.Ok( result.Data, "Reserva obtenida correctamente.");
            else
                return ApiResult<ReservaModel>.Fail(result.StatusCode, "No se encontro una reserva con ese id");
        }

        public override List<ReservaModel> GetModels()
        {
            return base.GetModels();
        }

        public override async Task<ApiResult<List<ReservaModel>>> CheckDataAPI(string endpoint)
        {
            var result = await base.CheckDataAPI(endpoint);
            if (result.Success)
                return ApiResult<List<ReservaModel>>.Ok(result.Data, "Lista de reservas actualizada correctamente.");
            else
                return ApiResult<List<ReservaModel>>.Fail(result.StatusCode, result.Message);
        }
    }
}
