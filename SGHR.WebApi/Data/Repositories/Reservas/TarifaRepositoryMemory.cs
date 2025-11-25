using SGHR.Web.Data.Interfaces.Reservas;
using SGHR.Web.Data.Repositories.Base;
using SGHR.Web.Models;
using SGHR.Web.Models.Reservas.Tarifa;
using SGHR.Web.Services.ClienteAPIService.Interface;

namespace SGHR.Web.Data.Repositories.Reservas
{
    public class TarifaRepositoryMemory : BaseRepositoryMemory<TarifaModel>, ITarifaRepositoryMemory
    {
        public TarifaRepositoryMemory(IClientAPI<TarifaModel> clienteAPI) : base(clienteAPI)
        {
        }

        public override ServicesResultModel GetByIDModel(int id)
        {
            var result = base.GetByIDModel(id);
            if (result.Success)
                return ServicesResultModel.Ok(result.Statuscode, result.Data, "Tarifa obtenida correctamente.");
            else
                return ServicesResultModel.Fail(result.Statuscode, "No se encontro una tarifa con ese id");
        }

        public override  List<TarifaModel> GetModels()
        {
            return base.GetModels();   
        }
        public override async Task<ServicesResultModel> CheckDataAPI(string endpoint)
        {
            var result = await base.CheckDataAPI(endpoint);
            if (result.Success)
                return ServicesResultModel.Ok(result.Statuscode, "Lista de tarifas actualizada correctamente.");
            else
                return ServicesResultModel.Fail(result.Statuscode, result.Message);
        }
    }
}
