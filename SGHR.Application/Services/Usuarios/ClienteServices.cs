
using Microsoft.Extensions.Logging;
using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Users.Cliente;
using SGHR.Application.Interfaces.Usuarios;
using SGHR.Persistence.Interfaces.Users;

namespace SGHR.Application.Services.Usuarios
{
    public class ClienteServices : IClienteServices
    {
        public readonly ILogger<ClienteServices> _logger;
        public readonly IClienteRepository _clienteRepository;

        public ClienteServices(ILogger<ClienteServices> logger, 
                               IClienteRepository clienteRepository)
        {
            _logger = logger;
            _clienteRepository = clienteRepository;           
        }

        public async Task<ServiceResult> CreateAsync(CreateClienteDto CreateDto)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResult> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResult> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResult> GetByNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResult> UpdateAsync(UpdateClienteDto UpdateDto)
        {
            throw new NotImplementedException();
        }
    }
}
