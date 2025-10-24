using SGHR.Domain.Base;
using SGHR.Domain.Entities.Configuration.Habitaciones;

namespace SGHR.Domain.Repository
{
    public interface IHabitacionRepository : IBaseRepository<Habitacion>
    {
        Task<OperationResult<List<Habitacion>>> GetByPisoAsync(int idPiso, bool includeDeleted = false);
        Task<OperationResult<List<Habitacion>>> GetByCategoriaAsync(int idCategoria, bool includeDeleted = false);
        Task<OperationResult<List<Habitacion>>> GetAvailableAsync(DateTime fechaInicio, DateTime fechaFin);
    }
}
