using SGHR.Web.Data.Interfaces.Usuarios;
using SGHR.Web.Models;
using SGHR.Web.Models.Usuarios.Cliente;
using SGHR.Web.Services.ClienteAPIService.Interface;
using SGHR.Web.Services.Interfaces.Usuarios;

namespace SGHR.Web.Services.ServiceAPI.Usuarios
{
    public class ClienteServiceAPI : IClienteServiceAPI
    {
        private readonly IClientAPI _httpAPI;
        private readonly IClienteRepositoryMemory _memory;
        public ClienteServiceAPI(IClientAPI clientAPI, IClienteRepositoryMemory clienteRepositoryMemory)
        {
            _httpAPI = clientAPI;
            _memory = clienteRepositoryMemory;
        }
        public ApiResult<ClienteModel> GetByIDServices(int id)
        {
            return _memory.GetByIDModel(id);
        }

        public List<ClienteModel> GetServices()
        {
            return _memory.GetModels();
        }

        public ApiResult<ClienteModel> GetByCedulaCliente(string cedula)
        {
            if (string.IsNullOrWhiteSpace(cedula))
                return ApiResult<ClienteModel>.Fail(400, "Tiene que introducir una cedula para comenzar a buscar.");

            return _memory.GetByCedulaModel(cedula);
        }

        public async Task<ApiResult<ClienteModel>> RemoveServicesPut(int id)
        {
            return await _httpAPI.DeleteAsync<ClienteModel>($"Cliente/Remove-Cliente?id={id}");
        }

        public async Task<ApiResult<ClienteModel>> SaveServicesPost(CreateClienteModel model)
        {
            return await _httpAPI.PostAsJsonAsync<CreateClienteModel ,ClienteModel>("Cliente/Create-Cliente", model);
        }

        public async Task<ApiResult<ClienteModel>> UpdateServicesPut(UpdateClienteModel model)
        {
            return await _httpAPI.PutAsJsonAsync<UpdateClienteModel, ClienteModel>($"Cliente/Update-Cliente", model);
        }
    }
}
