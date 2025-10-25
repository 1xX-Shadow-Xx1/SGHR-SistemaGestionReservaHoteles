using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolPoliApp.Persistence.Base;
using SGHR.Domain.Base;
using SGHR.Domain.Entities.Configuration.Reservas;
using SGHR.Domain.Enum.Reservas;
using SGHR.Domain.Repository;
using SGHR.Persistence.Contex;


namespace SGHR.Persistence.Repositories.EF.Reservas
{
    public sealed class ReservaRepository : BaseRepository<Reserva>, IReservaRepository
    {
        private readonly SGHRContext _context;
        private readonly ILogger<ReservaRepository> _logger;

        public ReservaRepository(SGHRContext context,
                                 ILogger<ReservaRepository> logger,
                                 ILogger<BaseRepository<Reserva>> loggerBase) : base(context, loggerBase)
        {
            _context = context;
            _logger = logger;
        }

        public override async Task<OperationResult<Reserva>> SaveAsync(Reserva entity, int? sesionId = null)
        {
            try
            {
                var result = await base.SaveAsync(entity, sesionId);
                _logger.LogInformation("Reserva del cliente {ClienteId} guardada exitosamente", entity.IdCliente);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error guardando la reserva del cliente {ClienteId}", entity.IdCliente);
                return OperationResult<Reserva>.Fail("Error guardando la reserva");
            }
        }


        public override async Task<OperationResult<Reserva>> UpdateAsync(Reserva entity, int? sesionId = null)
        {
            try
            {
                var result = await base.UpdateAsync(entity, sesionId);
                _logger.LogInformation("Reserva del cliente {ClienteId} actualizada exitosamente", entity.IdCliente);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error actualizando la reserva del cliente {ClienteId}", entity.IdCliente);
                return OperationResult<Reserva>.Fail("Error actualizando la reserva");
            }
        }

        public override async Task<OperationResult<Reserva>> DeleteAsync(Reserva entity, int? sesionId = null)
        {
            try
            {
                var result = await base.DeleteAsync(entity, sesionId);
                _logger.LogInformation("Reserva del cliente {ClienteId} eliminada exitosamente", entity.IdCliente);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error eliminando la reserva del cliente {ClienteId}", entity.IdCliente);
                return OperationResult<Reserva>.Fail("Error eliminando la reserva");
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
