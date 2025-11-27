using SGHR.Web.Data.Interfaces.Habitaciones;
using SGHR.Web.Data.Repositories.Base;
using SGHR.Web.Models;
using SGHR.Web.Models.Habitaciones.Amenity;
using SGHR.Web.Services.ClienteAPIService.Interface;

namespace SGHR.Web.Data.Repositories.Habitaciones
{
    public class AmenityRepositoryMemory : BaseRepositoryMemory<AmenityModel> , IAmenityRepositoryMemory
    {
        public AmenityRepositoryMemory(IClientAPI clienteAPI) : base(clienteAPI)
        {
        }

        public override ApiResult<AmenityModel> GetByIDModel(int id)
        {
            var result = base.GetByIDModel(id);
            if (result.Success)
                return ApiResult<AmenityModel>.Ok( result.Data, "Amenity obtenido correctamente.");
            else
                return ApiResult<AmenityModel>.Fail(result.StatusCode, "No se encontro un amenity con ese id");
        }

        public override List<AmenityModel> GetModels()
        {
            return base.GetModels();
        }

        public override async Task<ApiResult<List<AmenityModel>>> CheckDataAPI(string endpoint)
        {
            var result = await base.CheckDataAPI(endpoint);
            if (result.Success)
                return ApiResult<List<AmenityModel>>.Ok(result.Data, "Lista de amenities actualizada correctamente.");
            else
                return ApiResult<List<AmenityModel>>.Fail(result.StatusCode, result.Message);
        }
    }
}
