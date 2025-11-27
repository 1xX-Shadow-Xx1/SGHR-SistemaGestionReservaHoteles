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
        private readonly IClientAPI _clientAPI;

        public CategoriaServiceAPI(ICategoriaRepositoryMemory memory, IClientAPI clientAPI)
        {
            _memory = memory;
            _clientAPI = clientAPI;
        }

        public ApiResult<CategoriaModel> GetByIDServices(int id)
        {
            return _memory.GetByIDModel(id);
        }

        public List<CategoriaModel> GetServices()
        {
            return _memory.GetModels();
        }

        public async Task<ApiResult<CategoriaModel>> RemoveServicesPut(int id)
        {
            return await _clientAPI.DeleteAsync<CategoriaModel>($"Categoria/Remove-Categoria?id={id}");
        }

        public async Task<ApiResult<CategoriaModel>> SaveServicesPost(CreateCategoriaModel model)
        {
            return await _clientAPI.PostAsJsonAsync<CreateCategoriaModel, CategoriaModel>("Categoria/Create-Categoria", model);
        }

        public async Task<ApiResult<CategoriaModel>> UpdateServicesPut(UpdateCategoriaModel model)
        {
            return await _clientAPI.PutAsJsonAsync<UpdateCategoriaModel, CategoriaModel>("Categoria/Update-Categoria", model);
        }
    }
}
