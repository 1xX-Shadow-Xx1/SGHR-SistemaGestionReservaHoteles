using SGHR.Domain.Base;
using SGHR.Domain.Entities.Configuration.Reservas;
using SGHR.Domain.Repository;

namespace SGHR.Persistence.Interfaces.Reservas
{
    public interface ITarifaRepository : IBaseRepository<Tarifa>
    {
        Task<OperationResult<List<Tarifa>>> GetByCategoriaAsync(int idCategoria);
        Task<OperationResult<Tarifa>> GetByCategoriaAndTemporadaAsync(int idCategoria, string temporada);
        Task<OperationResult<List<Tarifa>>> GetByTemporadaAsync(string temporada);
    }
}
