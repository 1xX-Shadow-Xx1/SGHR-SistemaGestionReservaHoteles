using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolPoliApp.Persistence.Base;
using SGHR.Domain.Base;
using SGHR.Domain.Entities.Configuration.Usuers;
using SGHR.Domain.Enum.Usuario;
using SGHR.Domain.Repository;
using SGHR.Persistence.Contex;
using System.Linq.Expressions;


namespace SGHR.Persistence.Repositories.EF.Users
{
    public sealed class UsuarioRepository : BaseRepository<Usuario> ,IUsuarioRepository
    {
        private readonly SGHRContext _context;
        private readonly ILogger<UsuarioRepository> _logger;

        public UsuarioRepository(SGHRContext context,
                                 ILogger<UsuarioRepository>logger,
                                 ILogger<BaseRepository<Usuario>> loggerBase) : base(context, loggerBase)
        {
            _context = context;
            _logger = logger;

        }

        public override async Task<OperationResult<Usuario>> SaveAsync(Usuario entity, int? sesionId = null)
        {
            try
            {
                var result = await base.SaveAsync(entity, sesionId);
                if (result.Success)
                    _logger.LogInformation("Usuario {Nombre} creado correctamente.", entity.Nombre);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando el usuario {Nombre}", entity.Nombre);
                return OperationResult<Usuario>.Fail("Ocurrió un error al guardar el usuario.");
            }
        }

        public override async Task<OperationResult<Usuario>> UpdateAsync(Usuario entity, int? sesionId = null)
        {
            try
            {
                var result = await base.UpdateAsync(entity, sesionId);
                if (result.Success)
                    _logger.LogInformation("Usuario {Nombre} actualizado correctamente.", entity.Nombre);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error actualizando el usuario {Nombre}", entity.Nombre);
                return OperationResult<Usuario>.Fail("Ocurrió un error al actualizar el usuario.");
            }
        }

        public override async Task<OperationResult<Usuario>> DeleteAsync(Usuario entity, int? sesionId = null)
        {
            try
            {
                var result = await base.DeleteAsync(entity, sesionId);
                if (result.Success)
                    _logger.LogInformation("Usuario {Nombre} eliminado correctamente.", entity.Nombre);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error eliminando el usuario {Nombre}", entity.Nombre);
                return OperationResult<Usuario>.Fail("Ocurrió un error al eliminar el usuario.");
            }
        }

        public override async Task<OperationResult<Usuario>> GetByIdAsync(int id, bool includeDeleted = false)
        {
            try
            {
                var result = await base.GetByIdAsync(id, includeDeleted);
                if (result.Success)
                    _logger.LogInformation("Usuario con ID {Id} obtenido correctamente.", id);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo el usuario con ID {Id}", id);
                return OperationResult<Usuario>.Fail("Ocurrió un error al obtener el usuario.");
            }
        }

        public override async Task<OperationResult<List<Usuario>>> GetAllAsync(bool includeDeleted = false)
        {
            try
            {
                var result = await base.GetAllAsync(includeDeleted);
                if (result.Success)
                    _logger.LogInformation("Usuarios obtenidos correctamente, total: {Count}", result.Data.Count);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo todos los usuarios");
                return OperationResult<List<Usuario>>.Fail("Ocurrió un error al obtener los usuarios.");
            }
        }

        public override async Task<OperationResult<List<Usuario>>> GetAllByAsync(
            Expression<Func<Usuario, bool>> filter, bool includeDeleted = false)
        {
            try
            {
                var result = await base.GetAllByAsync(filter);
                if (result.Success)
                    _logger.LogInformation("Usuarios filtrados obtenidos correctamente, total: {Count}", result.Data.Count);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo usuarios filtrados");
                return OperationResult<List<Usuario>>.Fail("Ocurrió un error al obtener los usuarios filtrados.");
            }
        }

        public override async Task<OperationResult<bool>> ExistsAsync(
            Expression<Func<Usuario, bool>> filter, bool includeDeleted = false)
        {
            try
            {
                var result = await base.ExistsAsync(filter, includeDeleted);
                _logger.LogInformation("Comprobación de existencia realizada, resultado: {Exists}", result.Data);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error comprobando existencia de usuario");
                return OperationResult<bool>.Fail("Ocurrió un error al comprobar la existencia del usuario.");
            }
        }

        public async Task<OperationResult<Usuario>> GetByCorreoAsync(string correo)
        {
            try
            {
                var usuario = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.Correo == correo && !u.IsDeleted);

                if (usuario == null)
                {
                    _logger.LogWarning("Usuario con correo {Correo} no encontrado", correo);
                    return OperationResult<Usuario>.Fail("Usuario no encontrado");
                }

                _logger.LogInformation("Usuario con correo {Correo} obtenido correctamente", correo);
                return OperationResult<Usuario>.Ok(usuario);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo usuario por correo {Correo}", correo);
                return OperationResult<Usuario>.Fail("Ocurrió un error al obtener el usuario");
            }
        }

        public async Task<OperationResult<List<Usuario>>> GetByRolAsync(string rol)
        {
            try
            {
                var usuarios = await _context.Usuarios
                    .Where(u => u.Rol == rol && !u.IsDeleted)
                    .ToListAsync();

                _logger.LogInformation("{Count} usuarios obtenidos correctamente con rol {Rol}", usuarios.Count, rol);
                return OperationResult<List<Usuario>>.Ok(usuarios);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo usuarios por rol {Rol}", rol);
                return OperationResult<List<Usuario>>.Fail("Ocurrió un error al obtener los usuarios");
            }
        }

        public async Task<OperationResult<List<Usuario>>> GetActivosAsync()
        {
            try
            {
                var usuarios = await _context.Usuarios
                    .Where(u => u.Estado == EstadoUsuario.Activo && !u.IsDeleted)
                    .ToListAsync();

                _logger.LogInformation("{Count} usuarios activos obtenidos correctamente", usuarios.Count);
                return OperationResult<List<Usuario>>.Ok(usuarios);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo usuarios activos");
                return OperationResult<List<Usuario>>.Fail("Ocurrió un error al obtener los usuarios activos");
            }
        }

    }
}
