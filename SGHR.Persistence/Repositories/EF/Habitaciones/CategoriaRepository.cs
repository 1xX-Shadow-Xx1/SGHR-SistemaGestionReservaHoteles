using Microsoft.Extensions.Logging;
using SchoolPoliApp.Persistence.Base;
using SGHR.Domain.Base;
using SGHR.Domain.Entities.Configuration.Habitaciones;
using SGHR.Domain.Validators.ConfigurationRules.Habitaciones;
using SGHR.Persistence.Contex;
using SGHR.Persistence.Interfaces.Habitaciones;
using System.Linq.Expressions;

namespace SGHR.Persistence.Repositories.EF.Habitaciones
{
    public sealed class CategoriaRepository : BaseRepository<Categoria>, ICategoriaRepository
    {
        private readonly SGHRContext _context;
        private readonly ILogger<CategoriaRepository> _logger;
        private readonly CategoriaValidator _categoriaValidator;


        public CategoriaRepository(SGHRContext context,
                                   CategoriaValidator categoriaValidator,
                                   ILogger<CategoriaRepository> logger,
                                   ILogger<BaseRepository<Categoria>> loggerBase) : base(context, loggerBase)
        {
            _context = context;
            _logger = logger;
            _categoriaValidator = categoriaValidator;

        }

        public override async Task<OperationResult<Categoria>> SaveAsync(Categoria entity)
        {
            if (!_categoriaValidator.Validate(entity, out string errorMessage))
            {
                _logger.LogWarning("Fallo al guardar Categoria: {fail}", errorMessage);
                return OperationResult<Categoria>.Fail(errorMessage);
            }

            _logger.LogInformation("Guardando categoría...");
            return await base.SaveAsync(entity);
        }
        public override async Task<OperationResult<Categoria>> UpdateAsync(Categoria entity)
        {
            if (!_categoriaValidator.Validate(entity, out string errorMessage))
            {
                _logger.LogWarning("Fallo al actualizar Categoria: {fail}", errorMessage);
                return OperationResult<Categoria>.Fail(errorMessage);
            }

            _logger.LogInformation("Actualizando categoría...");
            return await base.UpdateAsync(entity);
        }
        public override async Task<OperationResult<Categoria>> DeleteAsync(Categoria entity)
        {
            _logger.LogInformation("Eliminando categoría...");
            return await base.DeleteAsync(entity);
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
        public async Task<OperationResult<Categoria>> GetByNombreAsync(string nombre)
        {
            try
            {
                var result = base.GetAllAsync().Result.Data.Where(c => c.Nombre == nombre);

                if (result.Any())
                {
                    _logger.LogInformation("Categoría con nombre {Nombre} obtenida correctamente", nombre);
                    return OperationResult<Categoria>.Ok(result.First());
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
