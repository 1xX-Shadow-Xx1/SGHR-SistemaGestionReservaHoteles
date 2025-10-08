using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SchoolPoliApp.Persistence.Base;
using SGHR.Domain.Base;
using SGHR.Domain.Entities.Configuration.Operaciones;
using SGHR.Persistence.Contex;
using SGHR.Persistence.Interfaces.Operaciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGHR.Persistence.Repositories.EF.Operaciones
{
    public sealed class CheckInOutRepository : BaseRepository<CheckInOut>, ICheckInOutRepository
    {
        private readonly SGHRContext _context;
        private readonly ILogger<CheckInOutRepository> _logger;
        private readonly IConfiguration _configuration;

        public CheckInOutRepository(SGHRContext context,
                                    ILogger<CheckInOutRepository> logger,
                                    IConfiguration configuration) : base(context)
        {
            _context = context;
            _logger = logger;
            _configuration = configuration;
        }

        public override async Task<OperationResult<CheckInOut>> Save(CheckInOut entity)
        {
            var result = await base.Save(entity);

            if (result.Success)
                _logger.LogInformation("CheckInOut registrado: {Id} - Reserva {ReservaId}", entity.ID, entity.IdReserva);
            else
                _logger.LogError("Error al registrar CheckInOut: {Message}", result.Message);

            return result;
        }

        public override async Task<OperationResult<CheckInOut>> Update(CheckInOut entity)
        {
            var result = await base.Update(entity);

            if (result.Success)
                _logger.LogInformation("CheckInOut actualizado: {Id}", entity.ID);
            else
                _logger.LogError("Error al actualizar CheckInOut {Id}: {Message}", entity.ID, result.Message);

            return result;
        }

        public override async Task<OperationResult<CheckInOut>> Delete(CheckInOut entity)
        {
            var result = await base.Delete(entity);

            if (result.Success)
                _logger.LogInformation("CheckInOut eliminado (soft delete): {Id}", entity.ID);
            else
                _logger.LogError("Error al eliminar CheckInOut {Id}: {Message}", entity.ID, result.Message);

            return result;
        }

        public override async Task<OperationResult<CheckInOut>> GetById(int id)
        {
            try
            {
                var entity = await _context.CheckInOut
                    .Include(c => c.Reserva)
                        .ThenInclude(r => r.Usuario) 
                    .Include(c => c.Reserva.Habitacion) 
                    .FirstOrDefaultAsync(c => c.ID == id && !c.is_deleted);

                if (entity == null)
                    return OperationResult<CheckInOut>.Fail("CheckInOut no encontrado");

                _logger.LogInformation("CheckInOut encontrado: {Id}", id);
                return OperationResult<CheckInOut>.Ok(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener CheckInOut por Id {Id}", id);
                return OperationResult<CheckInOut>.Fail($"Error: {ex.Message}");
            }
        }

        public async Task<OperationResult<CheckInOut>> GetByReservaAsync(int reservaId)
        {
            try
            {
                var entity = await _context.CheckInOut
                    .Include(c => c.Reserva)
                        .ThenInclude(r => r.Usuario)
                    .Include(c => c.Reserva.Habitacion)
                    .FirstOrDefaultAsync(c => c.IdReserva == reservaId && !c.is_deleted);

                if (entity == null)
                    return OperationResult<CheckInOut>.Fail("No se encontró CheckInOut para esta reserva");

                return OperationResult<CheckInOut>.Ok(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener CheckInOut por Reserva {ReservaId}", reservaId);
                return OperationResult<CheckInOut>.Fail($"Error: {ex.Message}");
            }
        }

        public async Task<OperationResult<List<CheckInOut>>> GetCheckInsActivosAsync()
        {
            try
            {
                var lista = await _context.CheckInOut
                    .Include(c => c.Reserva)
                    .Where(c => c.FechaCheckOut == null && !c.is_deleted)
                    .ToListAsync();

                if (!lista.Any())
                    return OperationResult<List<CheckInOut>>.Fail("No hay CheckIns activos");

                return OperationResult<List<CheckInOut>>.Ok(lista);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener CheckIns activos");
                return OperationResult<List<CheckInOut>>.Fail($"Error: {ex.Message}");
            }
        }

        // Obtener CheckOuts en un rango de fechas
        public async Task<OperationResult<List<CheckInOut>>> GetCheckOutsByFechaAsync(DateTime inicio, DateTime fin)
        {
            try
            {
                var lista = await _context.CheckInOut
                    .Include(c => c.Reserva)
                    .Where(c => c.FechaCheckOut >= inicio && c.FechaCheckOut <= fin && !c.is_deleted)
                    .ToListAsync();

                if (!lista.Any())
                    return OperationResult<List<CheckInOut>>.Fail("No se encontraron CheckOuts en este rango de fechas");

                return OperationResult<List<CheckInOut>>.Ok(lista);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener CheckOuts entre {Inicio} y {Fin}", inicio, fin);
                return OperationResult<List<CheckInOut>>.Fail($"Error: {ex.Message}");
            }
        }

        public async Task<OperationResult<List<CheckInOut>>> GetHistorialByUsuarioAsync(int usuarioId)
        {
            try
            {
                var lista = await _context.CheckInOut
                    .Include(c => c.Reserva)
                        .ThenInclude(r => r.Habitacion)
                    .Where(c => c.Reserva.IdUsuario == usuarioId && !c.is_deleted)
                    .ToListAsync();

                if (!lista.Any())
                    return OperationResult<List<CheckInOut>>.Fail("El usuario no tiene historial de CheckInOut");

                return OperationResult<List<CheckInOut>>.Ok(lista);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener historial de CheckInOut del usuario {UsuarioId}", usuarioId);
                return OperationResult<List<CheckInOut>>.Fail($"Error: {ex.Message}");
            }
        }
    }
}
