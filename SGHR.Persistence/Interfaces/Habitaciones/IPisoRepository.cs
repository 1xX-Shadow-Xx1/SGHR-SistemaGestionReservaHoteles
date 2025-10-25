using SGHR.Domain.Base;
using SGHR.Domain.Entities.Configuration.Habitaciones;
using SGHR.Domain.Repository;

namespace SGHR.Persistence.Interfaces.Habitaciones
{
    public interface IPisoRepository : IBaseRepository<Piso>
    {
        Task<OperationResult<Piso>> GetByNumeroPisoAsync(int numeroPiso, int? idpiso = 0);
    }
}
