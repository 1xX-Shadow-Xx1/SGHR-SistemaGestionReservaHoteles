using SGHR.Domain.Base;
using SGHR.Domain.Entities.Configuration.Operaciones;
using SGHR.Domain.Repository;

namespace SGHR.Persistence.Interfaces.Operaciones
{
    public interface IMantenimientoRepository : IBaseRepository<Mantenimiento>
    {
        Task<OperationResult<List<Mantenimiento>>> GetByHabitacionAsync(int idHabitacion);
        Task<OperationResult<List<Mantenimiento>>> GetByPisoAsync(int idPiso);
        Task<OperationResult<List<Mantenimiento>>> GetActiveMaintenancesAsync();
    }
}
