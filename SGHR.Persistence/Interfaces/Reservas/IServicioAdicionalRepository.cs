using SGHR.Domain.Base;
using SGHR.Domain.Entities.Configuration.Reservas;
using SGHR.Domain.Repository;


namespace SGHR.Persistence.Interfaces.Reservas
{
    public interface IServicioAdicionalRepository : IBaseRepository<ServicioAdicional>
    {
        Task<OperationResult<ServicioAdicional>> GetByNombreAsync(string nombre);
    }
}
