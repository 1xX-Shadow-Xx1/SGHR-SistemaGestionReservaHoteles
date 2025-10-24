using SGHR.Domain.Base;
using SGHR.Domain.Entities.Configuration.Usuers;
using SGHR.Domain.Repository;


namespace SGHR.Persistence.Interfaces.Users
{
    public interface IClienteRepository : IBaseRepository<Cliente>
    {
        Task<OperationResult<Cliente>> GetByCedulaAsync(string cedula);
        Task<OperationResult<List<Cliente>>> GetByNombreAsync(string nombre);
    }
}
