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
        private readonly IClientAPI<AmenityModel> _clientAPI;

        public AmenityServiceAPI(IAmenityRepositoryMemory memory, IClientAPI<AmenityModel> clientAPI)
        {
            _memory = memory;
            _clientAPI = clientAPI;
        }

        public ServicesResultModel GetByIDServices(int id)
        {
            return _memory.GetByIDModel(id);
        }

        public List<AmenityModel> GetServices()
        {
            return _memory.GetModels();
        }

        public async Task<ServicesResultModel> RemoveServicesPut(int id)
        {
            return await _clientAPI.DeleteAsync($"Amenity/Remove-Amenity?id={id}");
        }

        public async Task<ServicesResultModel> SaveServicesPost(CreateAmenityModel model)
        {
            return await _clientAPI.PostAsync("Amenity/Create-Amenity", model);
        }

        public async Task<ServicesResultModel> UpdateServicesPut(UpdateAmenityModel model)
        {
            return await _clientAPI.PutAsync("Amenity/Update-Amenity", model);
        }
    }
}
