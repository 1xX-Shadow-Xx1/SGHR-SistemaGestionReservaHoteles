using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolPoliApp.Persistence.Base;
using SGHR.Domain.Base;
using SGHR.Domain.Entities.Configuration.Operaciones;
using SGHR.Domain.Validators.ConfigurationRules.Operaciones;
using SGHR.Persistence.Context;
using SGHR.Persistence.Interfaces.Operaciones;

namespace SGHR.Persistence.Repositories.EF.Operaciones
{
    public sealed class AuditoryRepository : BaseRepository<Auditory>, IAuditoryRepository
    {
        private readonly SGHRContext _context;
        private readonly ILogger<AuditoryRepository> _logger;
        private readonly AuditoryValidator _auditoryValidator;

        public AuditoryRepository(SGHRContext context,
                                  AuditoryValidator auditoryValidator,
                                  ILogger<AuditoryRepository> logger,
                                  ILogger<BaseRepository<Auditory>> loggerBase) : base(context, loggerBase)
        {
            _context = context;
            _logger = logger;
            _auditoryValidator = auditoryValidator;

        }

        public override async Task<OperationResult<Auditory>> SaveAsync(Auditory entity)
        {
            if (!_auditoryValidator.Validate(entity, out string errorMessage))
            {
                _logger.LogWarning("Fallo al guardar el Auditorio: {fail}", errorMessage);
                return OperationResult<Auditory>.Fail(errorMessage);
            }

            var result = await base.SaveAsync(entity);

            if (result.Success)
                _logger.LogInformation("Auditoría creada correctamente (ID {Id}, Operación {Operacion})", result.Data?.Id, entity.Operacion);
            else
                _logger.LogWarning("No se pudo crear auditoría (Operación {Operacion}). Mensaje: {Message}", entity.Operacion, result.Message);

            return result;
        }
        public override async Task<OperationResult<Auditory>> UpdateAsync(Auditory entity)
        {
            if (!_auditoryValidator.Validate(entity, out string errorMessage))
            {
                _logger.LogWarning("Fallo al actualizar el Auditorio: {fail}", errorMessage);
                return OperationResult<Auditory>.Fail(errorMessage);
            }

            var result = await base.UpdateAsync(entity);

            if (result.Success)
                _logger.LogInformation("Auditoría actualizada correctamente (ID {Id})", result.Data?.Id);
            else
                _logger.LogWarning("No se pudo actualizar auditoría (ID {Id}). Mensaje: {Message}", entity.Id, result.Message);

            return result;
        }
        public override async Task<OperationResult<Auditory>> DeleteAsync(Auditory entity)
        {
            var result = await base.DeleteAsync(entity);

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
