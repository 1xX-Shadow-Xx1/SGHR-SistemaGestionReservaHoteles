using Microsoft.Extensions.Logging;
using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Users.Usuario;
using SGHR.Application.Interfaces.Usuarios;
using SGHR.Domain.Repository;

namespace SGHR.Application.Services.Usuarios
{
    public class UsuarioServices : IUsuarioServices
    {
        public readonly ILogger<UsuarioServices> _logger;
        public readonly IUsuarioRepository _usuarioRepository;

        public UsuarioServices(ILogger<UsuarioServices> logger,
                               IUsuarioRepository usuarioRepository)
        {
            _logger = logger;
            _usuarioRepository = usuarioRepository;
        }

        public async Task<ServiceResult> CreateAsync(CreateUsuarioDto CreateDto)
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

        public async Task<ServiceResult> UpdateAsync(UpdateUsuarioDto UpdateDto)
        {
            throw new NotImplementedException();
        }

    }
}
