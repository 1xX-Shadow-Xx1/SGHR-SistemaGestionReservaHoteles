using SGHR.Web.Data.Interfaces.Reservas;
using SGHR.Web.Data.Repositories.Base;
using SGHR.Web.Models;
using SGHR.Web.Models.Reservas.ServicioAdicional;
using SGHR.Web.Services.ClienteAPIService.Interface;

namespace SGHR.Web.Data.Repositories.Reservas
{
    public class ServicioAdicionalRepositoryMemory : BaseRepositoryMemory<ServicioAdicionalModel> , IServicioAdicionalRepositoryMemory
    {
        public ServicioAdicionalRepositoryMemory(IClientAPI<ServicioAdicionalModel> clienteAPI) : base(clienteAPI)
        {
        }

        public override ServicesResultModel GetByIDModel(int id)
        {
            var result = base.GetByIDModel(id);
            if (result.Success)
                return ServicesResultModel.Ok(result.Statuscode, result.Data, "Servicio obtenido correctamente.");
            else
                return ServicesResultModel.Fail(result.Statuscode, "No se encontro un servicio con ese id");
        }

        public override List<ServicioAdicionalModel> GetModels()
        {
            return base.GetModels();
        }

        public override async Task<ServicesResultModel> CheckDataAPI(string endpoint)
        {
            var result = await base.CheckDataAPI(endpoint);
            if (result.Success)
                return ServicesResultModel.Ok(result.Statuscode, "Lista de serivicios actualizada correctamente.");
            else
                return ServicesResultModel.Fail(result.Statuscode, result.Message);
        }
    }
}
