using SGHR.Web.Data.Interfaces.Habitaciones;
using SGHR.Web.Models;
using SGHR.Web.Models.Habitaciones.Amenity;
using SGHR.Web.Services.ClienteAPIService.Interface;
using SGHR.Web.Services.Interfaces.Habitaciones;

namespace SGHR.Web.Services.ServiceAPI.Habitaciones
{
    public class AmenityServiceAPI : IAmenityServiceAPI
    {
        private readonly IAmenityRepositoryMemory _memory;
        private readonly IClientAPI _clientAPI;

        public AmenityServiceAPI(IAmenityRepositoryMemory memory, IClientAPI clientAPI)
        {
            _memory = memory;
            _clientAPI = clientAPI;
        }

        public ApiResult<AmenityModel> GetByIDServices(int id)
        {
            return _memory.GetByIDModel(id);
        }

        public List<AmenityModel> GetServices()
        {
            return _memory.GetModels();
        }

        public async Task<ApiResult<AmenityModel>> RemoveServicesPut(int id)
        {
            return await _clientAPI.DeleteAsync<AmenityModel>($"Amenity/Remove-Amenity?id={id}");
        }

        public async Task<ApiResult<AmenityModel>> SaveServicesPost(CreateAmenityModel model)
        {
            return await _clientAPI.PostAsJsonAsync<CreateAmenityModel, AmenityModel>("Amenity/Create-Amenity", model);
        }

        public async Task<ApiResult<AmenityModel>> UpdateServicesPut(UpdateAmenityModel model)
        {
            return await _clientAPI.PutAsJsonAsync<UpdateAmenityModel, AmenityModel>("Amenity/Update-Amenity", model);
        }
    }
}
