using SGHR.Domain.Base;
using SGHR.Domain.Entities.Configuration.Habitaciones;
using SGHR.Domain.Repository;

namespace SGHR.Persistence.Interfaces.Habitaciones
{
    public interface ICategoriaRepository : IBaseRepository<Categoria>
    {
        Task<OperationResult<Categoria>> GetByNombreAsync(string nombre);
    }
}
