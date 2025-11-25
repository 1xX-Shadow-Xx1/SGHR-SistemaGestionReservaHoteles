using SGHR.Web.Data.Interfaces.Habitaciones;
using SGHR.Web.Models;
using SGHR.Web.Models.Habitaciones.Categoria;
using SGHR.Web.Services.ClienteAPIService.Interface;
using SGHR.Web.Services.Interfaces.Habitaciones;

namespace SGHR.Web.Services.ServiceAPI.Habitaciones
{
    public class CategoriaServiceAPI : ICategoriaServiceAPI
    {
        private readonly ICategoriaRepositoryMemory _memory;
        private readonly IClientAPI<CategoriaModel> _clientAPI;

        public CategoriaServiceAPI(ICategoriaRepositoryMemory memory, IClientAPI<CategoriaModel> clientAPI)
        {
            _memory = memory;
            _clientAPI = clientAPI;
        }

        public ServicesResultModel GetByIDServices(int id)
        {
            return _memory.GetByIDModel(id);
        }

        public List<CategoriaModel> GetServices()
        {
            return _memory.GetModels();
        }

        public async Task<ServicesResultModel> RemoveServicesPut(int id)
        {
            return await _clientAPI.DeleteAsync($"Categoria/Remove-Categoria?id={id}");
        }

        public async Task<ServicesResultModel> SaveServicesPost(CreateCategoriaModel model)
        {
            return await _clientAPI.PostAsync("Categoria/Create-Categoria", model);
        }

        public async Task<ServicesResultModel> UpdateServicesPut(UpdateCategoriaModel model)
        {
            return await _clientAPI.PutAsync("Categoria/Update-Categoria", model);
        }
    }
}
