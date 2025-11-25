using SGHR.Web.Data.Interfaces.Usuarios;
using SGHR.Web.Models;
using SGHR.Web.Models.Usuarios.Usuario;
using SGHR.Web.Services.ClienteAPIService.Interface;
using SGHR.Web.Services.Interfaces.Usuarios;

namespace SGHR.Web.Services.ServiceAPI.Usuarios
{
    public class UsuarioServiceAPI : IUsuarioServiceAPI
    {
        private readonly IClientAPI<UsuarioModel> _httpAPI;
        private readonly IUsuarioRepositoryMemory _usuarioMemory;

        public UsuarioServiceAPI(IClientAPI<UsuarioModel> clientAP,
                                 IUsuarioRepositoryMemory repositoryMemory)
        {
            _httpAPI = clientAP;
            _usuarioMemory = repositoryMemory;
        }

        public ServicesResultModel GetByIDServices(int id)
        {
            return _usuarioMemory.GetByIDModel(id);
        }

        public List<UsuarioModel> GetServices()
        {
            return _usuarioMemory.GetModels();
        }

        public async Task<ServicesResultModel> RemoveServicesPut(int id)
        {
            return await _httpAPI.DeleteAsync($"Usuario/Remove-Usuario?id={id}");
        }

        public async Task<ServicesResultModel> SaveServicesPost(CreateUsuarioModel model)
        {
            return await _httpAPI.PostAsync($"Usuario/create-Usuario", model);
        }

        public async Task<ServicesResultModel> UpdateServicesPut(UpdateUsuarioModel model)
        {
            return await _httpAPI.PutAsync($"Usuario/update-Usuario", model);
        }
    }
}
