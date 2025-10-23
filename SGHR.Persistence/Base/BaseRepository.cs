
using Microsoft.EntityFrameworkCore;
using SGHR.Domain.Base;
using SGHR.Domain.Entities.Configuration.Operaciones;
using SGHR.Domain.Repository;
using SGHR.Persistence.Contex;
using System.Linq.Expressions;

namespace SchoolPoliApp.Persistence.Base
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly SGHRContext _context;
        private readonly DbSet<TEntity> _dbSet;
        public BaseRepository(SGHRContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public virtual async Task<OperationResult<TEntity>> Save(TEntity entity)
        {
            try
            {
                await _dbSet.AddAsync(entity);
                await _context.SaveChangesAsync();

                return OperationResult<TEntity>.Ok(entity, "Registro creado correctamente");
            }
            catch (Exception ex)
            {
                return OperationResult<TEntity>.Fail($"Error al crear: {ex.InnerException?.Message}");
            }
        }

        public virtual async Task<OperationResult<TEntity>> Update(TEntity entity)
        {
            try
            {
                _dbSet.Update(entity);
                await _context.SaveChangesAsync();

                return OperationResult<TEntity>.Ok(entity, "Registro actualizado correctamente");
            }
            catch (Exception ex)
            {
                return OperationResult<TEntity>.Fail($"Error al actualizar: {ex.Message}");
            }
        }

        public virtual async Task<OperationResult<TEntity>> Delete(TEntity entity)
        {
            try
            {
                entity.is_deleted = true; 
                _dbSet.Update(entity);
                await _context.SaveChangesAsync();
                                
                return OperationResult<TEntity>.Ok(entity, "Registro eliminado correctamente");
            }
            catch (Exception ex)
            {
                return OperationResult<TEntity>.Fail($"Error al eliminar: {ex.Message}");
            }
        }

        public virtual async Task<OperationResult<List<TEntity>>> GetAll()
        {
            try
            {
                var list = await _dbSet.Where(e => !e.is_deleted).ToListAsync();

                return OperationResult<List<TEntity>>.Ok(list);
            }
            catch (Exception ex)
            {
                return OperationResult<List<TEntity>>.Fail($"Error al obtener registros: {ex.Message}");
            }
        }

        public virtual async Task<OperationResult<List<TEntity>>> GetAllBY(Expression<Func<TEntity, bool>> filter)
        {
            try
            {
                var list = await _dbSet.Where(filter).Where(e => !e.is_deleted).ToListAsync();
                return OperationResult<List<TEntity>>.Ok(list);
            }
            catch (Exception ex)
            {
                return OperationResult<List<TEntity>>.Fail($"Error al obtener registros filtrados: {ex.Message}");
            }
        }

        public virtual async Task<OperationResult<TEntity>> GetById(int id)
        {
            if (id <= 0)
            {
                return OperationResult<TEntity>.Fail("Id inválido");
            }
            try
            {
                var entity = await _dbSet.FirstOrDefaultAsync(e => e.ID == id && !e.is_deleted);
                if (entity == null)
                    return OperationResult<TEntity>.Fail("Registro no encontrado");

                return OperationResult<TEntity>.Ok(entity);
            }
            catch (Exception ex)
            {
                return OperationResult<TEntity>.Fail($"Error al obtener registro: {ex.Message}");
            }
        }

        public virtual async Task<OperationResult<bool>> ExistsAsync(Expression<Func<TEntity, bool>> filter)
        {
            try
            {
                var exists = await _dbSet.AnyAsync(filter);
                
                return OperationResult<bool>.Ok(exists);
            }
            catch (Exception ex)
            {
                return OperationResult<bool>.Fail($"Error al verificar existencia: {ex.Message}");
            }
        }
    }
}
