using SGHR.Domain.Base;
using System.Linq.Expressions;

namespace SGHR.Domain.Repository
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        Task<OperationResult<TEntity>> SaveAsync(TEntity entity, int? sesionId = null);
        Task<OperationResult<TEntity>> UpdateAsync(TEntity entity, int? sesionId = null);
        Task<OperationResult<TEntity>> DeleteAsync(TEntity entity, int? sesionId = null); 
        Task<OperationResult<TEntity>> GetByIdAsync(int id, bool includeDeleted = false);
        Task<OperationResult<List<TEntity>>> GetAllAsync(bool includeDeleted = false);
        Task<OperationResult<List<TEntity>>> GetAllByAsync(Expression<Func<TEntity, bool>> filter, bool includeDeleted = false);
        Task<OperationResult<bool>> ExistsAsync(Expression<Func<TEntity, bool>> filter, bool includeDeleted = false);
    }
}
