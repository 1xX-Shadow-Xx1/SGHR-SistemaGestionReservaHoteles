using SGHR.Domain.Base;
using SGHR.Domain.Entities.Configuration.Usuers;

namespace SGHR.Domain.Repository
{
    public interface IUsuarioRepository : IBaseRepository<Usuario>
    {
        Task<OperationResult<Usuario>> GetByCorreoAsync(string correo);
        Task<OperationResult<List<Usuario>>> GetByRolAsync(string rol);
        Task<OperationResult<List<Usuario>>> GetActivosAsync();
    }
}
