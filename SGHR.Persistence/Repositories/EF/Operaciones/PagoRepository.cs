using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SchoolPoliApp.Persistence.Base;
using SGHR.Domain.Base;
using SGHR.Domain.Entities.Configuration.Operaciones;
using SGHR.Domain.Validators.Operaciones;
using SGHR.Domain.Validators.Reservas;
using SGHR.Persistence.Contex;
using SGHR.Persistence.Interfaces.Reportes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGHR.Persistence.Repositories.EF.Operaciones
{
    public sealed class PagoRepository : BaseRepository<Pago>, IPagoRepository
    {
        private readonly SGHRContext _context;
        private readonly ILogger<PagoRepository> _logger;
        private readonly IConfiguration _configuration;

        public PagoRepository(SGHRContext context,
                              ILogger<PagoRepository> logger,
                              IConfiguration configuration) : base(context)
        {
            _context = context;
            _logger = logger;
            _configuration = configuration;
        }

        public override async Task<OperationResult<Pago>> Save(Pago entity)
        {
            var result = PagoValidator.Validate(entity);
            if (!result.Success)
            {
                return result;
            }

            if (result.Success)
                _logger.LogInformation("Pago registrado: {Id} - Monto {Monto}", entity.Id, entity.Monto);
            else
                _logger.LogError("Error al registrar Pago: {Message}", result.Message);

            return result;
        }

        public override async Task<OperationResult<Pago>> Update(Pago entity)
        {
            var result = PagoValidator.Validate(entity);
            if (!result.Success)
            {
                return result;
            }

            if (result.Success)
                _logger.LogInformation("Pago actualizado: {Id}", entity.Id);
            else
                _logger.LogError("Error al actualizar Pago {Id}: {Message}", entity.Id, result.Message);

            return result;
        }

        public override async Task<OperationResult<Pago>> Delete(Pago entity)
        {
            var result = PagoValidator.Validate(entity);
            if (!result.Success)
            {
                return result;
            }

            if (result.Success)
                _logger.LogInformation("Pago eliminado correctamente: {Id}", entity.Id);
            else
                _logger.LogError("Error al eliminar Pago {Id}: {Message}", entity.Id, result.Message);

            return result;
        }

        public override async Task<OperationResult<Pago>> GetById(int id)
        {
            try
            {
                var entity = await _context.Pagos
                    .Include(p => p.Reserva) // relación con Reserva
                    .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);

                if (entity == null)
                    return OperationResult<Pago>.Fail("Pago no encontrado");

                _logger.LogInformation("Pago encontrado: {Id}", id);
                return OperationResult<Pago>.Ok(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener pago por Id {Id}", id);
                return OperationResult<Pago>.Fail($"Error: {ex.Message}");
            }
        }

        public async Task<OperationResult<List<Pago>>> GetByReservaAsync(int reservaId)
        {
            try
            {
                var pagos = await _context.Pagos
                    .Where(p => p.IdReserva == reservaId && !p.IsDeleted)
                    .ToListAsync();

                if (!pagos.Any())
                    return OperationResult<List<Pago>>.Fail("No se encontraron pagos para esta reserva");

                return OperationResult<List<Pago>>.Ok(pagos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener pagos para la reserva {ReservaId}", reservaId);
                return OperationResult<List<Pago>>.Fail($"Error: {ex.Message}");
            }
        }

        public async Task<OperationResult<List<Pago>>> GetByFechasAsync(DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                var pagos = await _context.Pagos
                    .Where(p => p.FechaPago >= fechaInicio && p.FechaPago <= fechaFin && !p.IsDeleted)
                    .ToListAsync();

                if (!pagos.Any())
                    return OperationResult<List<Pago>>.Fail("No se encontraron pagos en este rango de fechas");

                return OperationResult<List<Pago>>.Ok(pagos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener pagos entre {FechaInicio} y {FechaFin}", fechaInicio, fechaFin);
                return OperationResult<List<Pago>>.Fail($"Error: {ex.Message}");
            }
        }

        public async Task<OperationResult<List<Pago>>> GetByMetodoAsync(string metodo)
        {
            try
            {
                var pagos = await _context.Pagos
                    .Where(p => p.MetodoPago == metodo && !p.IsDeleted)
                    .ToListAsync();

                if (!pagos.Any())
                    return OperationResult<List<Pago>>.Fail("No se encontraron pagos con este método");

                return OperationResult<List<Pago>>.Ok(pagos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener pagos por método {Metodo}", metodo);
                return OperationResult<List<Pago>>.Fail($"Error: {ex.Message}");
            }
        }
    }
}
