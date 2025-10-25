using SGHR.Domain.Base;

namespace SGHR.Domain.Repository
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        Task<OperationResult<TEntity>> SaveAsync(TEntity entity);
        Task<OperationResult<TEntity>> UpdateAsync(TEntity entity);
        Task<OperationResult<TEntity>> DeleteAsync(TEntity entity); 
        Task<OperationResult<TEntity>> GetByIdAsync(int id, bool includeDeleted = false);
        Task<OperationResult<List<TEntity>>> GetAllAsync(bool includeDeleted = false);
    }
}
