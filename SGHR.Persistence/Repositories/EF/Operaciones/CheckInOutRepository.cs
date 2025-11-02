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
    public sealed class CheckInOutRepository : BaseRepository<CheckInOut>, ICheckInOutRepository
    {
        private readonly SGHRContext _context;
        private readonly ILogger<CheckInOutRepository> _logger;
        private readonly CheckInOutValidator _checkInOutValidator;

        public CheckInOutRepository(SGHRContext context,
                                    CheckInOutValidator checkInOutValidator,
                                    ILogger<CheckInOutRepository> logger,
                                    ILogger<BaseRepository<CheckInOut>> loggerBase) : base(context, loggerBase)
        {
            _context = context;
            _logger = logger;
            _checkInOutValidator = checkInOutValidator;
        }

        public override async Task<OperationResult<CheckInOut>> SaveAsync(CheckInOut entity)
        {
            if (!_checkInOutValidator.Validate(entity, out string errorMessage))
            {
                _logger.LogWarning("Fallo al guardar el CheckInOut: {fail}", errorMessage);
                return OperationResult<CheckInOut>.Fail(errorMessage);
            }

            try
            {
                var result = await base.SaveAsync(entity);
                _logger.LogInformation("CheckInOut creado con Id {Id}", entity.Id);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando CheckInOut");
                return OperationResult<CheckInOut>.Fail("Ocurrió un error al crear el registro");
            }
        }
        public override async Task<OperationResult<CheckInOut>> UpdateAsync(CheckInOut entity)
        {
            if (!_checkInOutValidator.Validate(entity, out string errorMessage))
            {
                _logger.LogWarning("Fallo al actualizar el CheckInOut: {fail}", errorMessage);
                return OperationResult<CheckInOut>.Fail(errorMessage);
            }

            try
            {
                var result = await base.UpdateAsync(entity);
                _logger.LogInformation("CheckInOut actualizado con Id {Id}", entity.Id);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error actualizando CheckInOut");
                return OperationResult<CheckInOut>.Fail("Ocurrió un error al actualizar el registro");
            }
        }
        public override async Task<OperationResult<CheckInOut>> DeleteAsync(CheckInOut entity)
        {
            try
            {
                var result = await base.DeleteAsync(entity);
                _logger.LogInformation("CheckInOut eliminado con Id {Id}", entity.Id);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error eliminando CheckInOut");
                return OperationResult<CheckInOut>.Fail("Ocurrió un error al eliminar el registro");
            }
        }
        public async Task<OperationResult<CheckInOut>> GetByReservaAsync(int idReserva)
        {
            try
            {
                var check = await _context.CheckInOut
                    .FirstOrDefaultAsync(c => c.IdReserva == idReserva && !c.IsDeleted);

                if (check == null)
                {
                    _logger.LogWarning("CheckInOut con reserva {IdReserva} no encontrado", idReserva);
                    return OperationResult<CheckInOut>.Fail("CheckInOut no encontrado");
                }

                _logger.LogInformation("CheckInOut con reserva {IdReserva} encontrado", idReserva);
                return OperationResult<CheckInOut>.Ok(check);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo CheckInOut por reserva {IdReserva}", idReserva);
                return OperationResult<CheckInOut>.Fail("Ocurrió un error al obtener CheckInOut");
            }
        }
        public async Task<OperationResult<List<CheckInOut>>> GetByFechaAsync(DateTime fecha)
        {
            try
            {
                var checks = await _context.CheckInOut
                    .Where(c => c.FechaCheckIn.HasValue && c.FechaCheckIn.Value.Date == fecha.Date && !c.IsDeleted)
                    .ToListAsync();

                _logger.LogInformation("Se obtuvieron {count} CheckInOut con fecha {Fecha}.", checks.Count, fecha);
                return OperationResult<List<CheckInOut>>.Ok(checks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo CheckInOut por fecha {Fecha}", fecha);
                return OperationResult<List<CheckInOut>>.Fail("Ocurrió un error al obtener CheckInOut");
            }
        }
        public async Task<OperationResult<List<CheckInOut>>> GetCheckInsPendientesAsync()
        {
            try
            {
                var checks = await _context.CheckInOut
                    .Where(c => !c.FechaCheckIn.HasValue && !c.IsDeleted)
                    .ToListAsync();

                _logger.LogInformation("Se obtuvieron {count} CheckInOut pendientes.", checks.Count);
                return OperationResult<List<CheckInOut>>.Ok(checks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo CheckIns pendientes");
                return OperationResult<List<CheckInOut>>.Fail("Ocurrió un error al obtener CheckIns pendientes");
            }
        }
        public async Task<OperationResult<List<CheckInOut>>> GetByDateRangeAsync(DateTime inicio, DateTime fin)
        {
            try
            {
                var checks = await _context.CheckInOut
                    .Where(c => c.FechaCheckIn.HasValue && c.FechaCheckIn.Value.Date >= inicio.Date
                                && c.FechaCheckIn.Value.Date <= fin.Date && !c.IsDeleted)
                    .ToListAsync();

                _logger.LogInformation("Se obtuvieron {count} CheckInOut desde el {fechainicio} hasta {fechafin}.", inicio, fin);
                return OperationResult<List<CheckInOut>>.Ok(checks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo CheckInOut entre {Inicio} y {Fin}", inicio, fin);
                return OperationResult<List<CheckInOut>>.Fail("Ocurrió un error al obtener CheckInOut");
            }
        }

    }
}
