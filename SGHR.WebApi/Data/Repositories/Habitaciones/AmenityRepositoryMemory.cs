using SGHR.Web.Data.Interfaces.Habitaciones;
using SGHR.Web.Data.Repositories.Base;
using SGHR.Web.Models;
using SGHR.Web.Models.Habitaciones.Amenity;
using SGHR.Web.Services.ClienteAPIService.Interface;

namespace SGHR.Web.Data.Repositories.Habitaciones
{
    public class AmenityRepositoryMemory : BaseRepositoryMemory<AmenityModel> , IAmenityRepositoryMemory
    {
        public AmenityRepositoryMemory(IClientAPI<AmenityModel> clienteAPI) : base(clienteAPI)
        {
        }

        public override ServicesResultModel GetByIDModel(int id)
        {
            var result = base.GetByIDModel(id);
            if (result.Success)
                return ServicesResultModel.Ok(result.Statuscode, result.Data, "Amenity obtenido correctamente.");
            else
                return ServicesResultModel.Fail(result.Statuscode, "No se encontro un amenity con ese id");
        }

        public override List<AmenityModel> GetModels()
        {
            return base.GetModels();
        }

        public override async Task<ServicesResultModel> CheckDataAPI(string endpoint)
        {
            var result = await base.CheckDataAPI(endpoint);
            if (result.Success)
                return ServicesResultModel.Ok(result.Statuscode, "Lista de amenities actualizada correctamente.");
            else
                return ServicesResultModel.Fail(result.Statuscode, result.Message);
        }
    }
}
