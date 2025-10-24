using Microsoft.Extensions.Logging;
using SchoolPoliApp.Persistence.Base;
using SGHR.Domain.Base;
using SGHR.Domain.Entities.Configuration.Habitaciones;
using SGHR.Persistence.Contex;
using SGHR.Persistence.Interfaces.Habitaciones;
using System.Linq.Expressions;

namespace SGHR.Persistence.Repositories.EF.Habitaciones
{
    public sealed class AmenityRepository : BaseRepository<Amenity>, IAmenityRepository
    {
        private readonly SGHRContext _context;
        private readonly ILogger<AmenityRepository> _logger;

        public AmenityRepository(SGHRContext context,
                                   ILogger<AmenityRepository> logger,
                                   ILogger<BaseRepository<Amenity>> loggerBase) : base(context, loggerBase)
        {
            _context = context;
            _logger = logger;

        }
        public override async Task<OperationResult<Amenity>> SaveAsync(Amenity entity, int? sesionId = null)
        {
            _logger.LogInformation("Guardando Amenity.");
            var result = await base.SaveAsync(entity, sesionId);
            if (result.Success)
                _logger.LogInformation("Amenity creado correctamente con Id {Id}", entity.Id);
            return result;
        }

        public override async Task<OperationResult<Amenity>> UpdateAsync(Amenity entity, int? sesionId = null)
        {
            _logger.LogInformation("Actualizando Amenity.");
            var result = await base.UpdateAsync(entity, sesionId);
            if (result.Success)
                _logger.LogInformation("Amenity actualizado correctamente con Id {Id}", entity.Id);
            return result;
        }

        public override async Task<OperationResult<Amenity>> DeleteAsync(Amenity entity, int? sesionId = null)
        {
            _logger.LogInformation("Eliminando Amenity.");
            var result = await base.DeleteAsync(entity, sesionId);
            if (result.Success)
                _logger.LogInformation("Amenity eliminado correctamente con Id {Id}", entity.Id);
            return result;
        }

        public override async Task<OperationResult<Amenity>> GetByIdAsync(int id, bool includeDeleted = false)
        {
            _logger.LogInformation("Buscando Amenity con Id {id}.", id);
            var result = await base.GetByIdAsync(id, includeDeleted);
            if (result.Success)
                _logger.LogInformation("Amenity con Id {Id} obtenido correctamente", id);
            return result;
        }

        public override async Task<OperationResult<List<Amenity>>> GetAllAsync(bool includeDeleted = false)
        {
            _logger.LogInformation("Obteniendo todos los Amenities.");
            var result = await base.GetAllAsync(includeDeleted);
            if (result.Success)
                _logger.LogInformation("{Count} amenities obtenidos correctamente", result.Data.Count);
            return result;
        }

        public override async Task<OperationResult<List<Amenity>>> GetAllByAsync(Expression<Func<Amenity, bool>> filter, bool includeDeleted = false)
        {
            _logger.LogInformation("Obteniendo todos los Amenity con filtro.", filter);
            var result = await base.GetAllByAsync(filter, includeDeleted);
            if (result.Success)
                _logger.LogInformation("{Count} amenities obtenidos correctamente con filtro", result.Data.Count);
            return result;
        }

        public override async Task<OperationResult<bool>> ExistsAsync(Expression<Func<Amenity, bool>> filter, bool includeDeleted = false)
        {
            _logger.LogInformation("Verificando existencia del Amenity.");
            var result = await base.ExistsAsync(filter, includeDeleted);
            _logger.LogInformation("Existencia de amenity verificada: {Exists}", result.Data);
            return result;
        }
        public async Task<OperationResult<Amenity>> GetByNombreAsync(string nombre)
        {
            _logger.LogInformation("Obteniendo amenites con el nombre {name}", nombre);
            try
            {
                var result = await GetAllByAsync(a => a.Nombre == nombre, false);

                if (!result.Success || result.Data.Count == 0)
                {
                    _logger.LogWarning("Amenity con nombre {Nombre} no encontrado", nombre);
                    return OperationResult<Amenity>.Fail("Amenity no encontrado");
                }

                var amenity = result.Data.First();
                _logger.LogInformation("Amenity con nombre {Nombre} obtenido correctamente", nombre);
                return OperationResult<Amenity>.Ok(amenity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo amenity por nombre {Nombre}", nombre);
                return OperationResult<Amenity>.Fail("Ocurrió un error al obtener el amenity");
            }
        }

    }
}
