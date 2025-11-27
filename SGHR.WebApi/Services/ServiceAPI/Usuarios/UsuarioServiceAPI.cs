using SGHR.Web.Data.Interfaces.Usuarios;
using SGHR.Web.Models;
using SGHR.Web.Models.Usuarios.Usuario;
using SGHR.Web.Services.ClienteAPIService.Interface;
using SGHR.Web.Services.Interfaces.Usuarios;

namespace SGHR.Web.Services.ServiceAPI.Usuarios
{
    public class UsuarioServiceAPI : IUsuarioServiceAPI
    {
        private readonly IClientAPI _httpAPI;
        private readonly IUsuarioRepositoryMemory _memory;

        public UsuarioServiceAPI(IClientAPI clientAP,
                                 IUsuarioRepositoryMemory repositoryMemory)
        {
            _httpAPI = clientAP;
            _memory = repositoryMemory;
        }

        public ApiResult<UsuarioModel> GetByIDServices(int id)
        {
            return _memory.GetByIDModel(id);
        }

        public List<UsuarioModel> GetServices()
        {
            return _memory.GetModels();
        }

        public async Task<ApiResult<UsuarioModel>> RemoveServicesPut(int id)
        {
            return await _httpAPI.DeleteAsync<UsuarioModel>($"Usuario/Remove-Usuario?id={id}");
        }

        public async Task<ApiResult<UsuarioModel>> SaveServicesPost(CreateUsuarioModel model)
        {
            return await _httpAPI.PostAsJsonAsync<CreateUsuarioModel, UsuarioModel>($"Usuario/create-Usuario", model);
        }

        public async Task<ApiResult<UsuarioModel>> UpdateServicesPut(UpdateUsuarioModel model)
        {
            return await _httpAPI.PutAsJsonAsync<UpdateUsuarioModel, UsuarioModel>($"Usuario/update-Usuario", model);
        }

    }
}
