using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolPoliApp.Persistence.Base;
using SGHR.Domain.Base;
using SGHR.Domain.Entities.Configuration.Operaciones;
using SGHR.Domain.Entities.Configuration.Reservas;
using SGHR.Domain.Validators.ConfigurationRules.Operaciones;
using SGHR.Domain.Validators.Reservas;
using SGHR.Persistence.Contex;
using SGHR.Persistence.Interfaces.Reportes;
using System.Linq.Expressions;

namespace SGHR.Persistence.Repositories.EF.Operaciones
{
    public sealed class PagoRepository : BaseRepository<Pago>, IPagoRepository
    {
        private readonly SGHRContext _context;
        private readonly ILogger<PagoRepository> _logger;
        private readonly PagoValidator _validator;

        public PagoRepository(SGHRContext context,
                              PagoValidator validator,
                              ILogger<PagoRepository> logger,
                              ILogger<BaseRepository<Pago>> LoggerBase) : base(context, LoggerBase)
        {
            _context = context;
            _logger = logger;
            _validator = validator;
        }

        public override async Task<OperationResult<Pago>> SaveAsync(Pago entity)
        {
            if (!_validator.Validate(entity, out string errorMessage))
            {
                _logger.LogWarning("Fallo al guardar el Pago: {fail}", errorMessage);
                return OperationResult<Pago>.Fail(errorMessage);
            }

            var result = await base.SaveAsync(entity);

            if (result.Success)
                _logger.LogInformation("Pago {Id} creado correctamente", result.Data.Id);
            else
                _logger.LogWarning("Error creando el pago");

            return result;
        }
        public override async Task<OperationResult<Pago>> UpdateAsync(Pago entity)
        {
            if (!_validator.Validate(entity, out string errorMessage))
            {
                _logger.LogWarning("Fallo al actualizar el Pago: {fail}", errorMessage);
                return OperationResult<Pago>.Fail(errorMessage);
            }

            var result = await base.UpdateAsync(entity);

            if (result.Success)
                _logger.LogInformation("Pago {Id} actualizado correctamente", result.Data.Id);
            else
                _logger.LogWarning("Error actualizando el pago {Id}", entity.Id);

            return result;
        }
        public override async Task<OperationResult<Pago>> DeleteAsync(Pago entity)
        {
            var result = await base.DeleteAsync(entity);

            if (result.Success)
                _logger.LogInformation("Pago {Id} eliminado correctamente", entity.Id);
            else
                _logger.LogWarning("Error eliminando el pago {Id}", entity.Id);

            return result;
        }
        public override async Task<OperationResult<Pago>> GetByIdAsync(int id, bool includeDeleted = false)
        {
            var result = await base.GetByIdAsync(id, includeDeleted);

            if (result.Success)
                _logger.LogInformation("Pago {Id} obtenido correctamente", id);
            else
                _logger.LogWarning("Pago {Id} no encontrado", id);

            return result;
        }
        public override async Task<OperationResult<List<Pago>>> GetAllAsync(bool includeDeleted = false)
        {
            var result = await base.GetAllAsync(includeDeleted);

            if (result.Success)
                _logger.LogInformation("Todos los pagos obtenidos correctamente, cantidad: {Count}", result.Data.Count);
            else
                _logger.LogWarning("Error obteniendo los pagos");

            return result;
        }
        public async Task<OperationResult<List<Pago>>> GetByReservaAsync(int idReserva)
        {
            try
            {
                var pagos = await _context.Pagos
                    .Where(p => p.IdReserva == idReserva && !p.IsDeleted)
                    .ToListAsync();

                _logger.LogInformation("Se obtuvieron {Cantidad} pagos de la reserva {IdReserva}", pagos.Count, idReserva);
                return OperationResult<List<Pago>>.Ok(pagos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo pagos de la reserva {IdReserva}", idReserva);
                return OperationResult<List<Pago>>.Fail("Error obteniendo los pagos de la reserva");
            }
        }
        public async Task<OperationResult<List<Pago>>> GetByFechaAsync(DateTime fecha)
        {
            try
            {
                var pagos = await _context.Pagos
                    .Where(p => p.FechaPago.Date == fecha.Date && !p.IsDeleted)
                    .ToListAsync();

                _logger.LogInformation("Se obtuvieron {Cantidad} pagos realizados en {Fecha}", pagos.Count, fecha.ToShortDateString());
                return OperationResult<List<Pago>>.Ok(pagos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo pagos de la fecha {Fecha}", fecha.ToShortDateString());
                return OperationResult<List<Pago>>.Fail("Error obteniendo pagos de la fecha especificada");
            }
        }
    }
}
