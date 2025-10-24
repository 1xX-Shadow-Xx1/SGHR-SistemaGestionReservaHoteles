using SGHR.Domain.Base;
using SGHR.Domain.Entities.Configuration.Sesiones;
using SGHR.Domain.Repository;

namespace SGHR.Persistence.Interfaces.Sesiones
{
    public interface ISesionRepository : IBaseRepository<Sesion>
    {
        Task<OperationResult<List<Sesion>>> GetByUsuarioAsync(int usuarioId);
        Task<OperationResult<List<Sesion>>> GetActiveSessionsAsync();
    }
}
