using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolPoliApp.Persistence.Base;
using SGHR.Domain.Base;
using SGHR.Domain.Entities.Configuration.Habitaciones;
using SGHR.Domain.Validators.ConfigurationRules.Habitaciones;
using SGHR.Persistence.Context;
using SGHR.Persistence.Interfaces.Habitaciones;

namespace SGHR.Persistence.Repositories.EF.Habitaciones
{
    public sealed class PisoRepository : BaseRepository<Piso>, IPisoRepository
    {
        private readonly SGHRContext _context;
        private readonly ILogger<PisoRepository> _logger;
        private readonly PisoValidator _validator;


        public PisoRepository(SGHRContext context,
                              PisoValidator pisoValidator,
                              ILogger<PisoRepository> logger) : base(context)
        {
            _context = context;
            _logger = logger;
            _validator = pisoValidator;

        }

        public override async Task<OperationResult<Piso>> SaveAsync(Piso entity)
        {
            if (!_validator.Validate(entity, out string errorMessage))
            {
                _logger.LogWarning("Fallo al guardar el Piso: {fail}", errorMessage);
                return OperationResult<Piso>.Fail(errorMessage);
            }

            _logger.LogInformation("Guardando un nuevo piso: {@Piso}", entity);
            var result = await base.SaveAsync(entity);

            if (result.Success)
                _logger.LogInformation("Piso guardado exitosamente con ID {Id}", result.Data.Id);
            else
                _logger.LogWarning("Error al guardar piso: {Message}", result.Message);

            return result;
        }
        public override async Task<OperationResult<Piso>> UpdateAsync(Piso entity)
        {
            if (!_validator.Validate(entity, out string errorMessage))
            {
                _logger.LogWarning("Fallo al actualizar el Piso: {fail}", errorMessage);
                return OperationResult<Piso>.Fail(errorMessage);
            }

            _logger.LogInformation("Actualizando piso con ID {Id}", entity.Id);
            var result = await base.UpdateAsync(entity);

            if (result.Success)
                _logger.LogInformation("Piso actualizado correctamente con ID {Id}", result.Data.Id);
            else
                _logger.LogWarning("Error al actualizar piso con ID {Id}: {Message}", entity.Id, result.Message);

            return result;
        }
        public override async Task<OperationResult<Piso>> DeleteAsync(Piso entity)
        {
            _logger.LogInformation("Eliminando piso con ID {Id}", entity.Id);
            var result = await base.DeleteAsync(entity);

            if (result.Success)
                _logger.LogInformation("Piso eliminado exitosamente con ID {Id}", result.Data.Id);
            else
                _logger.LogWarning("Error al eliminar piso con ID {Id}: {Message}", entity.Id, result.Message);

            return result;
        }
        public override async Task<OperationResult<Piso>> GetByIdAsync(int id, bool includeDeleted = false)
        {
            _logger.LogInformation("Obteniendo piso por ID {Id}", id);
            var result = await base.GetByIdAsync(id, includeDeleted);

            if (result.Success && result.Data != null)
                _logger.LogInformation("Piso encontrado con ID {Id}", result.Data.Id);
            else
                _logger.LogWarning("No se encontró piso con ID {Id}", id);

            return result;
        }
        public override async Task<OperationResult<List<Piso>>> GetAllAsync(bool includeDeleted = false)
        {
            _logger.LogInformation("Obteniendo todos los pisos (includeDeleted={Include})", includeDeleted);
            var result = await base.GetAllAsync(includeDeleted);

            _logger.LogInformation("Se obtuvieron {Count} pisos", result.Data?.Count ?? 0);
            return result;
        }
        public async Task<OperationResult<Piso>> GetByNumeroPisoAsync(int numeroPiso, int? idpiso = 0)
        {
            try
            {
                _logger.LogInformation("Buscando piso con número {NumeroPiso}", numeroPiso);

                var piso = await _context.Piso
                    .FirstOrDefaultAsync(p => p.NumeroPiso == numeroPiso && !p.IsDeleted && p.Id != idpiso);

                if (piso == null)
                {
                    _logger.LogWarning("No se encontró ningún piso con número {NumeroPiso}", numeroPiso);
                    return OperationResult<Piso>.Fail($"No se encontró ningún piso con número {numeroPiso}");
                }

                _logger.LogInformation("Piso encontrado correctamente con número {NumeroPiso}", numeroPiso);
                return OperationResult<Piso>.Ok(piso);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener piso con número {NumeroPiso}", numeroPiso);
                return OperationResult<Piso>.Fail("Ocurrió un error al obtener el piso.");
            }
        }
    }
}
