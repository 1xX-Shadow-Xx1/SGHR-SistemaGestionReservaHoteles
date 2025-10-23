using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SchoolPoliApp.Persistence.Base;
using SGHR.Domain.Base;
using SGHR.Domain.Entities.Configuration.Operaciones;
using SGHR.Domain.Entities.Configuration.Usuers;
using SGHR.Domain.Repository;
using SGHR.Domain.Validators.Users;
using SGHR.Persistence.Contex;
using SGHR.Persistence.Interfaces.Operaciones;
using System.ClientModel.Primitives;


namespace SGHR.Persistence.Repositories.EF.Users
{
    public sealed class UsuarioRepository : BaseRepository<Usuario> ,IUsuarioRepository
    {
        private readonly SGHRContext _context;
        private readonly ILogger<UsuarioRepository> _logger;
        private readonly IConfiguration _configuration;
        private readonly IAuditoryRepository _auditoryRepository;

        public UsuarioRepository(SGHRContext context,
                                 ILogger<UsuarioRepository> logger,
                                 IAuditoryRepository auditoryRepository,
                                 IConfiguration configuration) : base(context)
        {
            _context = context;
            _logger = logger;
            _configuration = configuration;
            _auditoryRepository = auditoryRepository;

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
                _logger.LogWarning("Hubo un problema con la eliminacion logica: " + result.Message);
                return result;
            }
            return await base.Delete(entity);
        }
        public override async Task<OperationResult<Usuario>> GetById(int id)
        {
            _logger.LogInformation($"Obteniendo al usuario con id {id}");
            var result = await base.GetById(id);

            if (!result.Success)
                _logger.LogWarning($"Hubo problema al obtener el usuario: {result.Message}");

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
        public override async Task<OperationResult<List<Usuario>>> GetAll()
        {
            _logger.LogInformation("Iniciando obtencion de todos los usuarios.");
            var result = await base.GetAll();

            if (!result.Success)
                _logger.LogWarning($"Hubo un problema con la obtecion de los usuarios: {result.Message}");

            return result;
        }
    }
}
