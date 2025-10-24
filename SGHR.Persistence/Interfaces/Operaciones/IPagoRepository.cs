using SGHR.Domain.Base;
using SGHR.Domain.Entities.Configuration.Operaciones;
using SGHR.Domain.Repository;

namespace SGHR.Persistence.Interfaces.Reportes
{
    public interface IPagoRepository : IBaseRepository<Pago>
    {
        Task<OperationResult<List<Pago>>> GetByReservaAsync(int idReserva);
        Task<OperationResult<List<Pago>>> GetByFechaAsync(DateTime fecha);
    }
}
