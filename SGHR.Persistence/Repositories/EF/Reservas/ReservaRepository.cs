using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SchoolPoliApp.Persistence.Base;
using SGHR.Domain.Base;
using SGHR.Domain.Entities.Configuration.Reservas;
using SGHR.Domain.Repository;
using SGHR.Domain.Validators.Reservas;
using SGHR.Persistence.Contex;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGHR.Persistence.Repositories.EF.Reservas
{
    public sealed class ReservaRepository : BaseRepository<Reserva>, IReservaRepository
    {
        private readonly SGHRContext _context;
        private readonly ILogger<ReservaRepository> _logger;
        private readonly IConfiguration _configuration;

        public ReservaRepository(SGHRContext context,
                                 ILogger<ReservaRepository> logger,
                                 IConfiguration configuration) : base(context)
        {
            _context = context;
            _logger = logger;
            _configuration = configuration;
        }

        public override async Task<OperationResult<Reserva>> Save(Reserva entity)
        {
            var result = ReservaValidator.Validate(entity);
            if (!result.Success)
            {
                return result;
            }

            if (result.Success)
                _logger.LogInformation("Reserva creada: {Id} - Cliente {ClienteId} - Habitación {HabitacionId}", entity.Id, entity.IdCliente, entity.IdHabitacion);
            else
                _logger.LogError("Error al crear Reserva: {Message}", result.Message);

            return result;
        }

        public override async Task<OperationResult<Reserva>> Update(Reserva entity)
        {
            var result = ReservaValidator.Validate(entity);
            if (!result.Success)
            {
                return result;
            }

            if (result.Success)
                _logger.LogInformation("Reserva actualizada: {Id}", entity.Id);
            else
                _logger.LogError("Error al actualizar Reserva {Id}: {Message}", entity.Id, result.Message);

            return result;
        }

        public override async Task<OperationResult<Reserva>> Delete(Reserva entity)
        {
            var result = ReservaValidator.Validate(entity);
            if (!result.Success)
            {
                return result;
            }

            if (result.Success)
                _logger.LogInformation("Reserva eliminada correctamente: {Id}", entity.Id);
            else
                _logger.LogError("Error al eliminar Reserva {Id}: {Message}", entity.Id, result.Message);

            return result;
        }

        public override async Task<OperationResult<Reserva>> GetById(int id)
        {
            try
            {
                var entity = await _context.Reservas
                    .Include(r => r.Cliente)
                    .Include(r => r.Habitacion)
                    .Include(r => r.Pagos)
                    .FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted);

                if (entity == null)
                    return OperationResult<Reserva>.Fail("Reserva no encontrada");

                _logger.LogInformation("Reserva encontrada: {Id}", id);
                return OperationResult<Reserva>.Ok(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener reserva por Id {Id}", id);
                return OperationResult<Reserva>.Fail($"Error: {ex.Message}");
            }
        }

        public async Task<OperationResult<List<Reserva>>> GetByClienteAsync(int clienteId)
        {
            try
            {
                var reservas = await _context.Reservas
                    .Where(r => r.IdCliente == clienteId && !r.IsDeleted)
                    .Include(r => r.Habitacion)
                    .ToListAsync();

                if (!reservas.Any())
                    return OperationResult<List<Reserva>>.Fail("No se encontraron reservas para este cliente");

                return OperationResult<List<Reserva>>.Ok(reservas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener reservas del cliente {ClienteId}", clienteId);
                return OperationResult<List<Reserva>>.Fail($"Error: {ex.Message}");
            }
        }

        public async Task<OperationResult<List<Reserva>>> GetByEstadoAsync(string estado)
        {
            try
            {
                var reservas = await _context.Reservas
                    .Where(r => r.Estado == estado && !r.IsDeleted)
                    .Include(r => r.Cliente)
                    .ToListAsync();

                if (!reservas.Any())
                    return OperationResult<List<Reserva>>.Fail("No se encontraron reservas con ese estado");

                return OperationResult<List<Reserva>>.Ok(reservas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener reservas por estado {Estado}", estado);
                return OperationResult<List<Reserva>>.Fail($"Error: {ex.Message}");
            }
        }

        public async Task<OperationResult<List<Reserva>>> GetByFechasAsync(DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                var reservas = await _context.Reservas
                    .Where(r => r.FechaInicio >= fechaInicio && r.FechaFin <= fechaFin && !r.IsDeleted)
                    .Include(r => r.Habitacion)
                    .Include(r => r.Cliente)
                    .ToListAsync();

                if (!reservas.Any())
                    return OperationResult<List<Reserva>>.Fail("No se encontraron reservas en ese rango de fechas");

                return OperationResult<List<Reserva>>.Ok(reservas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener reservas entre {FechaInicio} y {FechaFin}", fechaInicio, fechaFin);
                return OperationResult<List<Reserva>>.Fail($"Error: {ex.Message}");
            }
        }
    }
}
