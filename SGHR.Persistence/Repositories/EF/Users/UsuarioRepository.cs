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
                _logger.LogWarning("Error al crear usuario: {Message}", result.Message);
                return result;
            }
            _logger.LogInformation("Usuario creado con ID: {Id}", entity.ID);
            return await base.Save(entity);
        }

        public override async Task<OperationResult<Usuario>> Update(Usuario entity)
        {
            var result = UsuarioValidator.Validate(entity);
            if (!result.Success)
            {
                _logger.LogWarning("Error al actualizar usuario: {Message}", result.Message);
                return result;
            }
            _logger.LogInformation("Usuario actualizado con ID: {Id}", entity.ID);
            return await base.Update(entity);
        }

        public override async Task<OperationResult<Usuario>> Delete(Usuario entity)
        {
            var result = UsuarioValidator.Validate(entity);
            if (!result.Success)
            {
                return result;
            }

            return await base.Delete(entity);
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
                    .FirstOrDefaultAsync(u => u.Correo == correo && !u.is_deleted);

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
        public async Task<OperationResult<Usuario>> GetByEmailAndPassword(string correo, string password)
        {
            try
            {
                var usuario = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.Correo == correo && u.Contraseña == password && !u.is_deleted);

                if (usuario == null)
                    return OperationResult<Usuario>.Fail("Usuario no encontrado");

                return OperationResult<Usuario>.Ok(usuario);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo usuario por correo y contraseña");
                return OperationResult<Usuario>.Fail($"Error: {ex.Message}");
            }
        }
        public override Task<OperationResult<List<Usuario>>> GetAll()
        {
            return base.GetAll();
        }
    }
}
