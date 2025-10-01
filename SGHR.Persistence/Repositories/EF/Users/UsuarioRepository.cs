using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolPoliApp.Persistence.Base;
using SGHR.Domain.Base;
using SGHR.Domain.Entities.Configuration.Usuers;
using SGHR.Domain.Repository;
using SGHR.Persistence.Contex;


namespace SGHR.Persistence.Repositories.EF.Users
{
    public sealed class UsuarioRepository : BaseRepository<Usuario> ,IUsuarioRepository
    {
        private readonly SGHRContext _context;
        private readonly ILogger<UsuarioRepository> _logger;

        public UsuarioRepository(SGHRContext context, ILogger<UsuarioRepository> logger)
            : base(context)
        {
            _context = context;
            _logger = logger;
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
