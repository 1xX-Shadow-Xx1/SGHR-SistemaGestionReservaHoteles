
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SGHR.Domain.Base;
using SGHR.Domain.Repository;
using SGHR.Persistence.Context;

namespace SchoolPoliApp.Persistence.Base
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly SGHRContext _context;
        private readonly DbSet<TEntity> _dbSet;
        private readonly ILogger<BaseRepository<TEntity>> _logger;
        public BaseRepository(SGHRContext context,
                              ILogger<BaseRepository<TEntity>> logger)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
            _logger = logger;
        }

        public BaseRepository(SGHRContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public virtual async Task<OperationResult<TEntity>> SaveAsync(TEntity entity)
        {
            try
            {
                await _dbSet.AddAsync(entity);
                await _context.SaveChangesAsync();

                return OperationResult<TEntity>.Ok(entity, $"{typeof(TEntity).Name} creada exitosamente");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public virtual async Task<OperationResult<TEntity>> UpdateAsync(TEntity entity)
        {
            try
            {
                entity.FechaActualizacion = DateTime.Now;

                _dbSet.Update(entity);
                await _context.SaveChangesAsync();

                return OperationResult<TEntity>.Ok(entity, $"{typeof(TEntity).Name} actualizada exitosamente");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public virtual async Task<OperationResult<TEntity>> DeleteAsync(TEntity entity)
        {
            try
            {
                entity.IsDeleted = true;
                entity.FechaActualizacion = DateTime.Now;

                _dbSet.Update(entity);
                await _context.SaveChangesAsync();

                return OperationResult<TEntity>.Ok(entity, $"{typeof(TEntity).Name} eliminada exitosamente");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public virtual async Task<OperationResult<TEntity>> GetByIdAsync(int id, bool includeDeleted = false)
        {
            try
            {
                var query = _dbSet.AsQueryable();
                if (!includeDeleted)
                    query = query.Where(e => !e.IsDeleted);

                var entity = await query.FirstOrDefaultAsync(e => e.Id == id);

                if (entity == null)
                {
                    return OperationResult<TEntity>.Fail($"{typeof(TEntity).Name} no encontrada.");
                }

                return OperationResult<TEntity>.Ok(entity,$"{typeof(TEntity).Name} con Id {id} obtenida correctamente.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public virtual async Task<OperationResult<List<TEntity>>> GetAllAsync(bool includeDeleted = false)
        {
            try
            {
                var entities = await _dbSet
                    .Where(e => includeDeleted || !e.IsDeleted)
                    .ToListAsync();

                return OperationResult<List<TEntity>>.Ok(entities,$"{entities.Count} {typeof(TEntity).Name} obtenidas correctamente.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
