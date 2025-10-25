using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolPoliApp.Persistence.Base;
using SGHR.Domain.Base;
using SGHR.Domain.Entities.Configuration.Operaciones;
using SGHR.Persistence.Contex;
using SGHR.Persistence.Interfaces.Operaciones;
using System.Linq.Expressions;

namespace SGHR.Persistence.Repositories.EF.Operaciones
{
    public sealed class AuditoryRepository : BaseRepository<Auditory>, IAuditoryRepository
    {
        private readonly SGHRContext _context;
        private readonly ILogger<AuditoryRepository> _logger;

        public AuditoryRepository(SGHRContext context,
                                  ILogger<AuditoryRepository> logger,
                                  ILogger<BaseRepository<Auditory>> loggerBase) : base(context, loggerBase)
        {
            _context = context;
            _logger = logger;

        }

        public override async Task<OperationResult<Auditory>> SaveAsync(Auditory entity, int? sesionId = null)
        {
            var result = await base.SaveAsync(entity, sesionId);

            if (result.Success)
                _logger.LogInformation("Auditoría creada correctamente (ID {Id}, Operación {Operacion})", result.Data?.Id, entity.Operacion);
            else
                _logger.LogWarning("No se pudo crear auditoría (Operación {Operacion}). Mensaje: {Message}", entity.Operacion, result.Message);

            return result;
        }

        public override async Task<OperationResult<Auditory>> UpdateAsync(Auditory entity, int? sesionId = null)
        {
            var result = await base.UpdateAsync(entity, sesionId);

            if (result.Success)
                _logger.LogInformation("Auditoría actualizada correctamente (ID {Id})", result.Data?.Id);
            else
                _logger.LogWarning("No se pudo actualizar auditoría (ID {Id}). Mensaje: {Message}", entity.Id, result.Message);

            return result;
        }

        public override async Task<OperationResult<Auditory>> DeleteAsync(Auditory entity, int? sesionId = null)
        {
            var result = await base.DeleteAsync(entity, sesionId);

            if (result.Success)
                _logger.LogInformation("Auditoría marcada como eliminada (ID {Id})", result.Data?.Id);
            else
                _logger.LogWarning("No se pudo eliminar auditoría (ID {Id}). Mensaje: {Message}", entity.Id, result.Message);

            return result;
        }

        public override async Task<OperationResult<Auditory>> GetByIdAsync(int id, bool includeDeleted = false)
        {
            var result = await base.GetByIdAsync(id, includeDeleted);

            if (result.Success)
                _logger.LogInformation("Auditoría obtenida correctamente (ID {Id})", id);
            else
                _logger.LogWarning("Auditoría con ID {Id} no encontrada. includeDeleted={IncludeDeleted}", id, includeDeleted);

            return result;
        }

        public override async Task<OperationResult<List<Auditory>>> GetAllAsync(bool includeDeleted = false)
        {
            var result = await base.GetAllAsync(includeDeleted);

            if (result.Success)
                _logger.LogInformation("Se obtuvieron {Count} auditorías (includeDeleted={IncludeDeleted})", result.Data?.Count ?? 0, includeDeleted);
            else
                _logger.LogWarning("No se pudieron obtener auditorías. includeDeleted={IncludeDeleted}", includeDeleted);

            return result;
        }

        public override async Task<OperationResult<List<Auditory>>> GetAllByAsync(
            Expression<Func<Auditory, bool>> filter, bool includeDeleted = false)
        {
            var result = await base.GetAllByAsync(filter, includeDeleted);

            if (result.Success)
                _logger.LogInformation("Auditorías filtradas obtenidas. Count={Count}", result.Data?.Count ?? 0);
            else
                _logger.LogWarning("Error obteniendo auditorías filtradas.");

            return result;
        }

        public override async Task<OperationResult<bool>> ExistsAsync(
            Expression<Func<Auditory, bool>> filter, bool includeDeleted = false)
        {
            var result = await base.ExistsAsync(filter, includeDeleted);

            if (result.Success)
                _logger.LogInformation("Verificación existencia auditoría: {Exists}", result.Data);
            else
                _logger.LogWarning("Error verificando existencia de auditoría.");

            return result;
        }

        public async Task<OperationResult<List<Auditory>>> GetByFechaAsync(DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                var auditorias = await _context.Auditory
                    .Where(a => a.Fecha >= fechaInicio && a.Fecha <= fechaFin && !a.IsDeleted)
                    .ToListAsync();

                if (auditorias == null || auditorias.Count == 0)
                {
                    _logger.LogWarning("No se encontraron auditorías entre {Inicio} y {Fin}", fechaInicio, fechaFin);
                    return OperationResult<List<Auditory>>.Fail("No se encontraron auditorías en el rango de fechas indicado");
                }

                _logger.LogInformation("Se obtuvieron {Count} auditorías entre {Inicio} y {Fin}", auditorias.Count, fechaInicio, fechaFin);
                return OperationResult<List<Auditory>>.Ok(auditorias);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener auditorías entre {Inicio} y {Fin}", fechaInicio, fechaFin);
                return OperationResult<List<Auditory>>.Fail("Ocurrió un error al obtener las auditorías por rango de fechas");
            }
        }

        public async Task<OperationResult<List<Auditory>>> GetBySesionAsync(int sesionId)
        {
            try
            {
                var auditorias = await _context.Auditory
                    .Where(a => a.IdSesion == sesionId && !a.IsDeleted)
                    .ToListAsync();

                if (auditorias == null || auditorias.Count == 0)
                {
                    _logger.LogWarning("No se encontraron auditorías para la sesión con ID {SesionId}", sesionId);
                    return OperationResult<List<Auditory>>.Fail("No se encontraron auditorías para la sesión indicada");
                }

                _logger.LogInformation("Se obtuvieron {Count} auditorías para la sesión {SesionId}", auditorias.Count, sesionId);
                return OperationResult<List<Auditory>>.Ok(auditorias);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener auditorías por sesión {SesionId}", sesionId);
                return OperationResult<List<Auditory>>.Fail("Ocurrió un error al obtener las auditorías por sesión");
            }
        }

    }
}
