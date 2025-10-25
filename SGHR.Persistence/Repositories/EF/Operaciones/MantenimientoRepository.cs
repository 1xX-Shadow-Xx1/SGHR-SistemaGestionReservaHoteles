using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolPoliApp.Persistence.Base;
using SGHR.Domain.Base;
using SGHR.Domain.Entities.Configuration.Operaciones;
using SGHR.Domain.Enum.Operaciones;
using SGHR.Domain.Validators.ConfigurationRules.Operaciones;
using SGHR.Persistence.Contex;
using SGHR.Persistence.Interfaces.Operaciones;

namespace SGHR.Persistence.Repositories.EF.Operaciones
{
    public sealed class MantenimientoRepository : BaseRepository<Mantenimiento>, IMantenimientoRepository
    {
        private readonly SGHRContext _context;
        private readonly ILogger<MantenimientoRepository> _logger;
        private readonly MantenimientoValidator _mantenimientoValidator;

        public MantenimientoRepository(SGHRContext context,
                                       MantenimientoValidator validation,
                                       ILogger<MantenimientoRepository> logger,
                                       ILogger<BaseRepository<Mantenimiento>> loggerBase) : base(context, loggerBase)
        {
            _context = context;
            _logger = logger;
            _mantenimientoValidator = validation;

        }

        public override async Task<OperationResult<Mantenimiento>> SaveAsync(Mantenimiento entity)
        {
            if (!_mantenimientoValidator.Validate(entity, out string errorMessage))
            {
                _logger.LogWarning("Fallo al guardar el Mantenimiento: {fail}", errorMessage);
                return OperationResult<Mantenimiento>.Fail(errorMessage);
            }

            var result = await base.SaveAsync(entity);

            if (result.Success)
                _logger.LogInformation("Mantenimiento {Id} creado correctamente", result.Data.Id);
            else
                _logger.LogWarning("Error creando el mantenimiento");

            return result;
        }
        public override async Task<OperationResult<Mantenimiento>> UpdateAsync(Mantenimiento entity)
        {
            if (!_mantenimientoValidator.Validate(entity, out string errorMessage))
            {
                _logger.LogWarning("Fallo al actualizar el Mantenimiento: {fail}", errorMessage);
                return OperationResult<Mantenimiento>.Fail(errorMessage);
            }

            var result = await base.UpdateAsync(entity);

            if (result.Success)
                _logger.LogInformation("Mantenimiento {Id} actualizado correctamente", result.Data.Id);
            else
                _logger.LogWarning("Error actualizando el mantenimiento {Id}", entity.Id);

            return result;
        }
        public override async Task<OperationResult<Mantenimiento>> DeleteAsync(Mantenimiento entity)
        {
            var result = await base.DeleteAsync(entity);

            if (result.Success)
                _logger.LogInformation("Mantenimiento {Id} eliminado correctamente", entity.Id);
            else
                _logger.LogWarning("Error eliminando el mantenimiento {Id}", entity.Id);

            return result;
        }
        public async Task<OperationResult<List<Mantenimiento>>> GetActiveMaintenancesAsync()
        {
            try
            {
                var mantenimientos = await _context.Mantenimiento
                    .Where(m => m.Estado == EstadoMantenimiento.EnProceso && !m.IsDeleted)
                    .ToListAsync();

                _logger.LogInformation("Mantenimientos activos obtenidos, cantidad: {Count}", mantenimientos.Count);

                return OperationResult<List<Mantenimiento>>.Ok(mantenimientos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo mantenimientos activos");
                return OperationResult<List<Mantenimiento>>.Fail("Ocurrió un error al obtener los mantenimientos activos");
            }
        }
        public async Task<OperationResult<List<Mantenimiento>>> GetByHabitacionAsync(int idHabitacion)
        {
            try
            {
                var mantenimientos = await _context.Mantenimiento
                    .Where(m => m.IdHabitacion == idHabitacion && !m.IsDeleted)
                    .ToListAsync();

                _logger.LogInformation("Mantenimientos obtenidos por habitación {HabitacionId}, cantidad: {Count}", idHabitacion, mantenimientos.Count);

                return OperationResult<List<Mantenimiento>>.Ok(mantenimientos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo mantenimientos por habitación {HabitacionId}", idHabitacion);
                return OperationResult<List<Mantenimiento>>.Fail("Ocurrió un error al obtener los mantenimientos");
            }
        }
        public async Task<OperationResult<List<Mantenimiento>>> GetByPisoAsync(int idPiso)
        {
            try
            {
                var mantenimientos = await _context.Mantenimiento
                    .Where(m => m.IdPiso == idPiso && !m.IsDeleted)
                    .ToListAsync();

                _logger.LogInformation("Mantenimientos obtenidos por piso {PisoId}, cantidad: {Count}", idPiso, mantenimientos.Count);

                return OperationResult<List<Mantenimiento>>.Ok(mantenimientos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo mantenimientos por piso {PisoId}", idPiso);
                return OperationResult<List<Mantenimiento>>.Fail("Ocurrió un error al obtener los mantenimientos");
            }
        }
    }
}
