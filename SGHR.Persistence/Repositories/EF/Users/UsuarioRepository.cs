using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SchoolPoliApp.Persistence.Base;
using SGHR.Domain.Base;
using SGHR.Domain.Entities.Configuration.Usuers;
using SGHR.Domain.Repository;
using SGHR.Domain.Validators.Users;
using SGHR.Persistence.Contex;


namespace SGHR.Persistence.Repositories.EF.Users
{
    public sealed class UsuarioRepository : BaseRepository<Usuario> ,IUsuarioRepository
    {
        private readonly SGHRContext _context;
        private readonly ILogger<UsuarioRepository> _logger;
        private readonly IConfiguration _configuration;

        public UsuarioRepository(SGHRContext context,
                                 ILogger<UsuarioRepository> logger,
                                 IConfiguration configuration) : base(context)
        {
            _context = context;
            _logger = logger;
            _configuration = configuration;
        }
        public override async Task<OperationResult<Usuario>> Save(Usuario entity)
        {
            var result = UsuarioValidator.Validate(entity);
            if (!result.Success)
            {
                return result;
            }

            if (result.Success)
                _logger.LogInformation("Usuario creado correctamente con correo {Correo}", entity.Correo);
            else
                _logger.LogError("Error al crear Usuario con correo {Correo}: {Message}", entity.Correo, result.Message);

            return result;
        }

        public override async Task<OperationResult<Usuario>> Update(Usuario entity)
        {
            var result = UsuarioValidator.Validate(entity);
            if (!result.Success)
            {
                return result;
            }

            if (result.Success)
                _logger.LogInformation("Usuario actualizado correctamente: {Id} - {Correo}", entity.Id, entity.Correo);
            else
                _logger.LogError("Error al actualizar Usuario {Id}: {Message}", entity.Id, result.Message);

            return result;
        }

        public override async Task<OperationResult<Usuario>> Delete(Usuario entity)
        {
            var result = UsuarioValidator.Validate(entity);
            if (!result.Success)
            {
                return result;
            }

            if (result.Success)
                _logger.LogInformation("Usuario eliminado correctamente: {Id} - {Correo}", entity.Id, entity.Correo);
            else
                _logger.LogError("Error al eliminar Usuario {Id}: {Message}", entity.Id, result.Message);

            return result;
        }

        public override async Task<OperationResult<Usuario>> GetById(int id)
        {
            var result = await base.GetById(id);

            if (result.Success)
                _logger.LogInformation("Usuario encontrado: {Id}", id);
            else
                _logger.LogWarning("No se encontró el Usuario con Id {Id}", id);

            return result;
        }
        public async Task<OperationResult<Usuario>> GetByCorreoAsync(string correo)
        {
            try
            {
                var usuario = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.Correo == correo && !u.IsDeleted);

                if (usuario == null)
                    return OperationResult<Usuario>.Fail("Usuario no encontrado");

                return OperationResult<Usuario>.Ok(usuario);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo usuario por correo");
                return OperationResult<Usuario>.Fail($"Error: {ex.Message}");
            }
        }
    }
}
