using SGHR.Domain.Base;
using SGHR.Domain.Entities.Configuration.Habitaciones;
using SGHR.Domain.Repository;

namespace SGHR.Persistence.Interfaces.Habitaciones
{
    public interface IAmenityRepository : IBaseRepository<Amenity>
    {
        Task<OperationResult<Amenity>> GetByNombreAsync(string nombre);
    }
}
