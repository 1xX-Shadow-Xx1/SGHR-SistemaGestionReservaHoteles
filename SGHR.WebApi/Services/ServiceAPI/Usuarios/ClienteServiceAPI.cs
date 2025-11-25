using SGHR.Web.Data.Interfaces.Usuarios;
using SGHR.Web.Models;
using SGHR.Web.Models.Usuarios.Cliente;
using SGHR.Web.Services.ClienteAPIService.Interface;
using SGHR.Web.Services.Interfaces.Usuarios;

namespace SGHR.Web.Services.ServiceAPI.Usuarios
{
    public class ClienteServiceAPI : IClienteServiceAPI
    {
        private readonly IClientAPI<ClienteModel> _httpAPI;
        private readonly IClienteRepositoryMemory _clienteMemory;
        public ClienteServiceAPI(IClientAPI<ClienteModel> clientAPI, IClienteRepositoryMemory clienteRepositoryMemory)
        {
            _httpAPI = clientAPI;
            _clienteMemory = clienteRepositoryMemory;
        }
        public ServicesResultModel GetByIDServices(int id)
        {
            return _clienteMemory.GetByIDModel(id);
        }

        public List<ClienteModel> GetServices()
        {
            return _clienteMemory.GetModels();
        }

        public ServicesResultModel GetByCedulaCliente(string cedula)
        {
            if (string.IsNullOrWhiteSpace(cedula))
                return ServicesResultModel.Fail(400, "Tiene que introducir una cedula para comenzar a buscar.");

            return _clienteMemory.GetByCedulaModel(cedula);
        }

        public async Task<ServicesResultModel> RemoveServicesPut(int id)
        {
            return await _httpAPI.DeleteAsync($"Cliente/Remove-Cliente?id={id}");
        }

        public async Task<ServicesResultModel> SaveServicesPost(CreateClienteModel model)
        {
            return await _httpAPI.PostAsync("Cliente/Create-Cliente", model);
        }

        public async Task<ServicesResultModel> UpdateServicesPut(UpdateClienteModel model)
        {
            return await _httpAPI.PutAsync($"Cliente/Update-Cliente", model);
        }
    }
}
