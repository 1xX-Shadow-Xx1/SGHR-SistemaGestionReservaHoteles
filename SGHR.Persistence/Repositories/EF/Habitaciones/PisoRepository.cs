using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolPoliApp.Persistence.Base;
using SGHR.Domain.Base;
using SGHR.Domain.Entities.Configuration.Habitaciones;
using SGHR.Persistence.Contex;
using SGHR.Persistence.Interfaces.Habitaciones;

namespace SGHR.Persistence.Repositories.EF.Habitaciones
{
    public sealed class PisoRepository : BaseRepository<Piso>, IPisoRepository
    {
        private readonly SGHRContext _context;
        private readonly ILogger<PisoRepository> _logger;


        public PisoRepository(SGHRContext context,
                              ILogger<PisoRepository> logger,
                              ILogger<BaseRepository<Piso>> loggerBase) : base(context, loggerBase)
        {
            _context = context;
            _logger = logger;

        }

        public override async Task<OperationResult<Piso>> SaveAsync(Piso entity, int? sesionId = null)
        {
            _logger.LogInformation("Guardando un nuevo piso: {@Piso}", entity);
            var result = await base.SaveAsync(entity, sesionId);

            if (result.Success)
                _logger.LogInformation("Piso guardado exitosamente con ID {Id}", result.Data.Id);
            else
                _logger.LogWarning("Error al guardar piso: {Message}", result.Message);

            return result;
        }

        public override async Task<OperationResult<Piso>> UpdateAsync(Piso entity, int? sesionId = null)
        {
            _logger.LogInformation("Actualizando piso con ID {Id}", entity.Id);
            var result = await base.UpdateAsync(entity, sesionId);

            if (result.Success)
                _logger.LogInformation("Piso actualizado correctamente con ID {Id}", result.Data.Id);
            else
                _logger.LogWarning("Error al actualizar piso con ID {Id}: {Message}", entity.Id, result.Message);

            return result;
        }

        public override async Task<OperationResult<Piso>> DeleteAsync(Piso entity, int? sesionId = null)
        {
            _logger.LogInformation("Eliminando piso con ID {Id}", entity.Id);
            var result = await base.DeleteAsync(entity, sesionId);

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

        public override async Task<OperationResult<List<Piso>>> GetAllByAsync(System.Linq.Expressions.Expression<Func<Piso, bool>> filter, bool includeDeleted = false)
        {
            _logger.LogInformation("Obteniendo pisos con un filtro aplicado (includeDeleted={Include})", includeDeleted);
            var result = await base.GetAllByAsync(filter, includeDeleted);

            _logger.LogInformation("Se obtuvieron {Count} pisos filtrados", result.Data?.Count ?? 0);
            return result;
        }

        public override async Task<OperationResult<bool>> ExistsAsync(System.Linq.Expressions.Expression<Func<Piso, bool>> filter, bool includeDeleted = false)
        {
            _logger.LogInformation("Verificando existencia de pisos con un filtro aplicado");
            var result = await base.ExistsAsync(filter, includeDeleted);

            _logger.LogInformation("Resultado de existencia: {Exists}", result.Data);
            return result;
        }

        public async Task<OperationResult<Piso>> GetByNumeroPisoAsync(int numeroPiso)
        {
            try
            {
                _logger.LogInformation("Buscando piso con número {NumeroPiso}", numeroPiso);

                var piso = await _context.Piso
                    .FirstOrDefaultAsync(p => p.NumeroPiso == numeroPiso && !p.IsDeleted);

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
