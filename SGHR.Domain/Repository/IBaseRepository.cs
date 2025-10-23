using SGHR.Domain.Base;
using System.Linq.Expressions;

namespace SGHR.Domain.Repository
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        Task<OperationResult<TEntity>> Save(TEntity entity);
        Task<OperationResult<TEntity>> Update(TEntity entity);
        Task<OperationResult<TEntity>> Delete(TEntity entity);
        Task<OperationResult<List<TEntity>>> GetAll();
        Task<OperationResult<List<TEntity>>> GetAllBY(Expression<Func<TEntity, bool>> filter);
        Task<OperationResult<TEntity>> GetById(int id);
        Task<OperationResult<bool>> ExistsAsync(Expression<Func<TEntity, bool>> filter);
    }
}
