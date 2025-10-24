using SGHR.Domain.Base;
using SGHR.Domain.Entities.Configuration.Operaciones;
using SGHR.Domain.Repository;

namespace SGHR.Persistence.Interfaces.Operaciones
{
    public interface IAuditoryRepository : IBaseRepository<Auditory>
    {
        Task<OperationResult<List<Auditory>>> GetBySesionAsync(int sesionId);
        Task<OperationResult<List<Auditory>>> GetByFechaAsync(DateTime fechaInicio, DateTime fechaFin);
    }
}
