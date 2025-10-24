using SGHR.Domain.Base;
using SGHR.Domain.Entities.Configuration.Operaciones;
using SGHR.Domain.Repository;


namespace SGHR.Persistence.Interfaces.Operaciones
{
    public interface ICheckInOutRepository : IBaseRepository<CheckInOut>
    {
        Task<OperationResult<CheckInOut>> GetByReservaAsync(int idReserva);
        Task<OperationResult<List<CheckInOut>>> GetByFechaAsync(DateTime fecha);
        Task<OperationResult<List<CheckInOut>>> GetCheckInsPendientesAsync();
        Task<OperationResult<List<CheckInOut>>> GetByDateRangeAsync(DateTime inicio, DateTime fin);
    }
}
