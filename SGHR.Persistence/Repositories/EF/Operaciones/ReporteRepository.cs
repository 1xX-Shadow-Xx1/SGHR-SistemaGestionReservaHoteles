using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolPoliApp.Persistence.Base;
using SGHR.Domain.Base;
using SGHR.Domain.Entities.Configuration.Reportes;
using SGHR.Domain.Validators.ConfigurationRules.Operaciones;
using SGHR.Persistence.Context;
using SGHR.Persistence.Interfaces.Reportes;


namespace SGHR.Persistence.Repositories.EF.Operaciones
{
    public sealed class ReporteRepository : BaseRepository<Reporte>, IReporteRepository
    {

        private readonly SGHRContext _context;
        private readonly ILogger<ReporteRepository> _logger;
        private readonly ReporteValidator _reporteValidator;


        public ReporteRepository(SGHRContext context,
                                ReporteValidator reporteValidator,
                                 ILogger<ReporteRepository> logger) : base(context)
        {
            _context = context;
            _logger = logger;
            _reporteValidator = reporteValidator;

        }

        public override async Task<OperationResult<Reporte>> SaveAsync(Reporte entity)
        {
            if (!_reporteValidator.Validate(entity, out string errorMessage))
            {
                _logger.LogWarning("Fallo al guardar el Reporte: {fail}", errorMessage);
                return OperationResult<Reporte>.Fail(errorMessage);
            }

            try
            {
                var result = await base.SaveAsync(entity);
                _logger.LogInformation("Reporte {ReporteId} guardado exitosamente", entity.Id);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error guardando el reporte {ReporteId}", entity.Id);
                return OperationResult<Reporte>.Fail("Error guardando el reporte");
            }
        }
        public override async Task<OperationResult<Reporte>> UpdateAsync(Reporte entity)
        {
            if (!_reporteValidator.Validate(entity, out string errorMessage))
            {
                _logger.LogWarning("Fallo al actualizar el Reporte: {fail}", errorMessage);
                return OperationResult<Reporte>.Fail(errorMessage);
            }

            try
            {
                var result = await base.UpdateAsync(entity);
                _logger.LogInformation("Reporte {ReporteId} actualizado exitosamente", entity.Id);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error actualizando el reporte {ReporteId}", entity.Id);
                return OperationResult<Reporte>.Fail("Error actualizando el reporte");
            }
        }
        public override async Task<OperationResult<Reporte>> DeleteAsync(Reporte entity)
        {
            try
            {
                var result = await base.DeleteAsync(entity);
                _logger.LogInformation("Reporte {ReporteId} eliminado exitosamente", entity.Id);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error eliminando el reporte {ReporteId}", entity.Id);
                return OperationResult<Reporte>.Fail("Error eliminando el reporte");
            }
        }
        public async Task<OperationResult<List<Reporte>>> GetByFechaAsync(DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                var reportes = await _context.Reportes
                    .Where(r => r.FechaGeneracion >= fechaInicio && r.FechaGeneracion <= fechaFin && !r.IsDeleted)
                    .ToListAsync();

                _logger.LogInformation("Se obtuvieron {Cantidad} reportes entre {FechaInicio} y {FechaFin}", reportes.Count, fechaInicio, fechaFin);
                return OperationResult<List<Reporte>>.Ok(reportes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo reportes entre {FechaInicio} y {FechaFin}", fechaInicio, fechaFin);
                return OperationResult<List<Reporte>>.Fail("Error obteniendo reportes");
            }
        }
        public async Task<OperationResult<List<Reporte>>> GetByTipoAsync(string tipoReporte)
        {
            try
            {
                var reportes = await _context.Reportes
                    .Where(r => r.TipoReporte == tipoReporte && !r.IsDeleted)
                    .ToListAsync();

                _logger.LogInformation("Se obtuvieron {Cantidad} reportes del tipo {TipoReporte}", reportes.Count, tipoReporte);
                return OperationResult<List<Reporte>>.Ok(reportes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo reportes del tipo {TipoReporte}", tipoReporte);
                return OperationResult<List<Reporte>>.Fail("Error obteniendo reportes del tipo especificado");
            }
        }
        public async Task<OperationResult<List<Reporte>>> GetByUsuarioAsync(int usuarioId)
        {
            try
            {
                var reportes = await _context.Reportes
                    .Where(r => r.GeneradoPor == usuarioId && !r.IsDeleted)
                    .ToListAsync();

                _logger.LogInformation("Se obtuvieron {Cantidad} reportes para el usuario {UsuarioId}", reportes.Count, usuarioId);
                return OperationResult<List<Reporte>>.Ok(reportes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo reportes para el usuario {UsuarioId}", usuarioId);
                return OperationResult<List<Reporte>>.Fail("Error obteniendo reportes del usuario");
            }
        }
    }
}
