using Microsoft.Extensions.Logging;
using SchoolPoliApp.Persistence.Base;
using SGHR.Domain.Base;
using SGHR.Domain.Entities.Configuration.Habitaciones;
using SGHR.Persistence.Contex;
using SGHR.Persistence.Interfaces.Habitaciones;
using System.Linq.Expressions;

namespace SGHR.Persistence.Repositories.EF.Habitaciones
{
    public sealed class CategoriaRepository : BaseRepository<Categoria>, ICategoriaRepository
    {
        private readonly SGHRContext _context;
        private readonly ILogger<CategoriaRepository> _logger;


        public CategoriaRepository(SGHRContext context,
                                   ILogger<CategoriaRepository> logger,
                                   ILogger<BaseRepository<Categoria>> loggerBase) : base(context, loggerBase)
        {
            _context = context;
            _logger = logger;

        }

        public override async Task<OperationResult<Categoria>> SaveAsync(Categoria entity, int? sesionId = null)
        {
            _logger.LogInformation("Guardando categoría...");
            return await base.SaveAsync(entity, sesionId);
        }
        public override async Task<OperationResult<Categoria>> UpdateAsync(Categoria entity, int? sesionId = null)
        {
            _logger.LogInformation("Actualizando categoría...");
            return await base.UpdateAsync(entity, sesionId);
        }
        public override async Task<OperationResult<Categoria>> DeleteAsync(Categoria entity, int? sesionId = null)
        {
            _logger.LogInformation("Eliminando categoría...");
            return await base.DeleteAsync(entity, sesionId);
        }
        public override async Task<OperationResult<Categoria>> GetByIdAsync(int id, bool includeDeleted = false)
        {
            _logger.LogInformation("Obteniendo categoría por Id {Id}", id);
            return await base.GetByIdAsync(id, includeDeleted);
        }
        public override async Task<OperationResult<List<Categoria>>> GetAllAsync(bool includeDeleted = false)
        {
            _logger.LogInformation("Obteniendo todas las categorías...");
            return await base.GetAllAsync(includeDeleted);
        }
        public override async Task<OperationResult<List<Categoria>>> GetAllByAsync(Expression<Func<Categoria, bool>> filter, bool includeDeleted = false)
        {
            _logger.LogInformation("Obteniendo categorías con filtro...");
            return await base.GetAllByAsync(filter, includeDeleted);
        }
        public override async Task<OperationResult<bool>> ExistsAsync(Expression<Func<Categoria, bool>> filter, bool includeDeleted = false)
        {
            _logger.LogInformation("Verificando existencia de categoría...");
            return await base.ExistsAsync(filter, includeDeleted);
        }
        public async Task<OperationResult<Categoria>> GetByNombreAsync(string nombre)
        {
            try
            {
                var result = await base.GetAllByAsync(c => c.Nombre == nombre);

                if (result.Success && result.Data.Any())
                {
                    _logger.LogInformation("Categoría con nombre {Nombre} obtenida correctamente", nombre);
                    return OperationResult<Categoria>.Ok(result.Data.First());
                }

                _logger.LogWarning("No se encontró ninguna categoría con nombre {Nombre}", nombre);
                return OperationResult<Categoria>.Fail("Categoría no encontrada");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo categoría con nombre {Nombre}", nombre);
                return OperationResult<Categoria>.Fail($"Ocurrió un error al obtener la categoría: {ex.Message}");
            }
        }

    }
}
