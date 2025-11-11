using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolPoliApp.Persistence.Base;
using SGHR.Domain.Base;
using SGHR.Domain.Entities.Configuration.Reservas;
using SGHR.Domain.Enum.Reservas;
using SGHR.Domain.Repository;
using SGHR.Domain.Validators.ConfigurationRules.Reservas;
using SGHR.Persistence.Context;


namespace SGHR.Persistence.Repositories.EF.Reservas
{
    public sealed class ReservaRepository : BaseRepository<Reserva>, IReservaRepository
    {
        private readonly SGHRContext _context;
        private readonly ILogger<ReservaRepository> _logger;
        private readonly ReservaValidator _validator;

        public ReservaRepository(SGHRContext context,
                                 ReservaValidator validator,
                                 ILogger<ReservaRepository> logger) : base(context)
        {
            _context = context;
            _logger = logger;
            _validator = validator;
        }

        public override async Task<OperationResult<Reserva>> SaveAsync(Reserva entity)
        {
            if (!_validator.Validate(entity, out string errorMessage))
            {
                _logger.LogWarning("Fallo al guardar la Reserva: {fail}", errorMessage);
                return OperationResult<Reserva>.Fail(errorMessage);
            }

            try
            {
                var result = await base.SaveAsync(entity);
                _logger.LogInformation("Reserva del cliente {ClienteId} guardada exitosamente", entity.IdCliente);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error guardando la reserva del cliente {ClienteId}", entity.IdCliente);
                return OperationResult<Reserva>.Fail("Error guardando la reserva");
            }
        }
        public override async Task<OperationResult<Reserva>> UpdateAsync(Reserva entity)
        {
            if (!_validator.Validate(entity, out string errorMessage))
            {
                _logger.LogWarning("Fallo al actualizar la Reserva: {fail}", errorMessage);
                return OperationResult<Reserva>.Fail(errorMessage);
            }

            try
            {
                var result = await base.UpdateAsync(entity);
                _logger.LogInformation("Reserva del cliente {ClienteId} actualizada exitosamente", entity.IdCliente);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error actualizando la reserva del cliente {ClienteId}", entity.IdCliente);
                return OperationResult<Reserva>.Fail("Error actualizando la reserva");
            }
        }
        public override async Task<OperationResult<Reserva>> DeleteAsync(Reserva entity)
        {
            try
            {
                entity.Estado = EstadoReserva.Cancelada;
                var result = await base.DeleteAsync(entity);
                _logger.LogInformation("Reserva del cliente {ClienteId} eliminada exitosamente", entity.IdCliente);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error eliminando la reserva del cliente {ClienteId}", entity.IdCliente);
                return OperationResult<Reserva>.Fail("Error eliminando la reserva");
            }
        }
        public override async Task<OperationResult<Reserva>> GetByIdAsync(int id, bool includeDeleted = false)
        {
            try
            {
                var result = await _context.Reservas
                    .Include(r => r.Servicios)
                    .FirstOrDefaultAsync(r => r.Id == id && 
                    r.IsDeleted == includeDeleted);
                if(result == null)
                {
                    _logger.LogInformation("Reserva con id {id} obtenida correctamente.", id);
                    return OperationResult<Reserva>.Fail("Reserva no encontrada.");
                }
                _logger.LogInformation("Reserva con id {id} obtenida correctamente.", id);
                return OperationResult<Reserva>.Ok(result,"Reserva encontrada correctamente."); ;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error pbteniendo la reserva con Id {id}", id);
                return OperationResult<Reserva>.Fail("Error al tratar de obtener la reserva.");
            }
        }
        public async Task<OperationResult<List<Reserva>>> GetActiveReservationsAsync()
        {
            try
            {
                var reservas = await _context.Reservas
                    .Where(r => r.Estado == EstadoReserva.Activa && !r.IsDeleted)
                    .ToListAsync();

                _logger.LogInformation("Se obtuvieron {Cantidad} reservas activas", reservas.Count);
                return OperationResult<List<Reserva>>.Ok(reservas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo reservas activas");
                return OperationResult<List<Reserva>>.Fail("Error obteniendo reservas activas");
            }
        }
        public async Task<OperationResult<List<Reserva>>> GetByClienteAsync(int idCliente)
        {
            try
            {
                var reservas = await _context.Reservas
                    .Where(r => r.IdCliente == idCliente && !r.IsDeleted)
                    .ToListAsync();

                _logger.LogInformation("Se obtuvieron {Cantidad} reservas del cliente {ClienteId}", reservas.Count, idCliente);
                return OperationResult<List<Reserva>>.Ok(reservas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo reservas del cliente {ClienteId}", idCliente);
                return OperationResult<List<Reserva>>.Fail("Error obteniendo reservas del cliente");
            }
        }
        public async Task<OperationResult<List<Reserva>>> GetByHabitacionAsync(int idHabitacion)
        {
            try
            {
                var reservas = await _context.Reservas
                    .Where(r => r.IdHabitacion == idHabitacion && !r.IsDeleted)
                    .ToListAsync();

                _logger.LogInformation("Se obtuvieron {Cantidad} reservas para la habitacion {HabitacionId}", reservas.Count, idHabitacion);
                return OperationResult<List<Reserva>>.Ok(reservas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo reservas para la habitacion {HabitacionId}", idHabitacion);
                return OperationResult<List<Reserva>>.Fail("Error obteniendo reservas de la habitacion");
            }
        }
    }
}
