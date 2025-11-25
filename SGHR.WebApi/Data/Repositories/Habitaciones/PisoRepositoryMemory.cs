using SGHR.Web.Data.Interfaces.Habitaciones;
using SGHR.Web.Data.Repositories.Base;
using SGHR.Web.Models;
using SGHR.Web.Models.Habitaciones.Piso;
using SGHR.Web.Services.ClienteAPIService.Interface;

namespace SGHR.Web.Data.Repositories.Habitaciones
{
    public class PisoRepositoryMemory : BaseRepositoryMemory<PisoModel> , IPisoRepositoryMemory
    {
        public PisoRepositoryMemory(IClientAPI<PisoModel> clienteAPI) : base(clienteAPI)
        {
        }

        public override ServicesResultModel GetByIDModel(int id)
        {
            var result = base.GetByIDModel(id);
            if (result.Success)
                return ServicesResultModel.Ok(result.Statuscode, result.Data, "Piso obtenido correctamente.");
            else
                return ServicesResultModel.Fail(result.Statuscode, "No se encontro un piso con ese id");
        }

        public override List<PisoModel> GetModels()
        {
            return base.GetModels();
        }

        public override async Task<ServicesResultModel> CheckDataAPI(string endpoint)
        {
            var result = await base.CheckDataAPI(endpoint);
            if (result.Success)
                return ServicesResultModel.Ok(result.Statuscode, "Lista de pisos actualizada correctamente.");
            else
                return ServicesResultModel.Fail(result.Statuscode, result.Message);
        }
    }
}
