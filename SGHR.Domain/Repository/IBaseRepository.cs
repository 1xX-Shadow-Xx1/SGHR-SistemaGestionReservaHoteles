using SGHR.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SGHR.Domain.Repository
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        Task<OperationResult> Save(TEntity entity);
        Task<OperationResult> Update(TEntity entity);
        Task<OperationResult> Delete(TEntity entity);
        Task<OperationResult> GetALL();
        Task<OperationResult> GetALL(Expression<Func<TEntity, bool>> filter);
        Task<OperationResult> GetEntityByID(int Id);
        Task<bool> Exists(Expression<Func<TEntity, bool>> filter);
    }
}
