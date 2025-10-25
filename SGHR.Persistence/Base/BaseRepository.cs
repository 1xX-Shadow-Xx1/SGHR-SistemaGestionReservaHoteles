
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SGHR.Domain.Base;
using SGHR.Domain.Repository;
using SGHR.Persistence.Contex;

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

        public virtual async Task<OperationResult<TEntity>> SaveAsync(TEntity entity)
        {
            try
            {
                await _dbSet.AddAsync(entity);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Entidad {EntityType} creada correctamente con Id {Id}", typeof(TEntity).Name, entity.Id);
                return OperationResult<TEntity>.Ok(entity, $"{typeof(TEntity).Name} creada exitosamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando la entidad {EntityType}", typeof(TEntity).Name);
                return OperationResult<TEntity>.Fail($"Error al guardar: {ex.Message.ToString()}");
            }
        }
        public virtual async Task<OperationResult<TEntity>> UpdateAsync(TEntity entity)
        {
            try
            {
                entity.FechaActualizacion = DateTime.Now;

                _dbSet.Update(entity);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Entidad {EntityType} actualizada correctamente con Id {Id}", typeof(TEntity).Name, entity.Id);
                return OperationResult<TEntity>.Ok(entity, $"{typeof(TEntity).Name} actualizada exitosamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error actualizando la entidad {EntityType} con Id {Id}", typeof(TEntity).Name, entity.Id);
                return OperationResult<TEntity>.Fail($"Error al actualizar: {ex.Message}");
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

                _logger.LogInformation("Entidad {EntityType} eliminada correctamente con Id {Id}", typeof(TEntity).Name, entity.Id);
                return OperationResult<TEntity>.Ok(entity, $"{typeof(TEntity).Name} eliminada exitosamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error eliminando la entidad {EntityType} con Id {Id}", typeof(TEntity).Name, entity.Id);
                return OperationResult<TEntity>.Fail($"Error al eliminar: {ex.Message}");
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
                    _logger.LogWarning("Entidad {EntityType} con Id {Id} no encontrada", typeof(TEntity).Name, id);
                    return OperationResult<TEntity>.Fail("Entidad no encontrada");
                }

                _logger.LogInformation("Entidad {EntityType} con Id {Id} obtenida correctamente", typeof(TEntity).Name, id);
                return OperationResult<TEntity>.Ok(entity,$"{typeof(TEntity).Name} con Id {id} obtenida correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo la entidad {EntityType} con Id {Id}", typeof(TEntity).Name, id);
                return OperationResult<TEntity>.Fail($"Error al obtener: {ex.Message}");
            }
        }
        public virtual async Task<OperationResult<List<TEntity>>> GetAllAsync(bool includeDeleted = false)
        {
            try
            {
                var entities = await _dbSet
                    .Where(e => includeDeleted || !e.IsDeleted)
                    .ToListAsync();

                _logger.LogInformation("{Count} entidades {EntityType} obtenidas correctamente", entities.Count, typeof(TEntity).Name);
                return OperationResult<List<TEntity>>.Ok(entities,$"{entities.Count} {typeof(TEntity).Name} obtenidas correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo entidades {EntityType}", typeof(TEntity).Name);
                return OperationResult<List<TEntity>>.Fail($"Error al obtener la lista: {ex.Message}");
            }
        }
    }
}
