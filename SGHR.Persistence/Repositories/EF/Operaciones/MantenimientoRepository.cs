using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SchoolPoliApp.Persistence.Base;
using SGHR.Domain.Base;
using SGHR.Domain.Entities.Configuration.Operaciones;
using SGHR.Domain.Validators.Operaciones;
using SGHR.Persistence.Contex;
using SGHR.Persistence.Interfaces.Operaciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGHR.Persistence.Repositories.EF.Operaciones
{
    public sealed class MantenimientoRepository : BaseRepository<Mantenimiento>, IMantenimientoRepository
    {
        private readonly SGHRContext _context;
        private readonly ILogger<MantenimientoRepository> _logger;
        private readonly IConfiguration _configuration;

        public MantenimientoRepository(SGHRContext context,
                                       ILogger<MantenimientoRepository> logger,
                                       IConfiguration configuration) : base(context)
        {
            _context = context;
            _logger = logger;
            _configuration = configuration;
        }

        public override async Task<OperationResult<Mantenimiento>> Save(Mantenimiento entity)
        {
            var result = MantenimientoValidator.Validate(entity);
            if (!result.Success)
            {
                return result;
            }

            if (result.Success)
                _logger.LogInformation("Mantenimiento registrado: {Id} - Habitación {HabitacionId}", entity.Id, entity.IdHabitacion);
            else
                _logger.LogError("Error al registrar mantenimiento: {Message}", result.Message);

            return result;
        }

        public override async Task<OperationResult<Mantenimiento>> Update(Mantenimiento entity)
        {
            var result = MantenimientoValidator.Validate(entity);
            if (!result.Success)
            {
                return result;
            }

            if (result.Success)
                _logger.LogInformation("Mantenimiento actualizado: {Id}", entity.Id);
            else
                _logger.LogError("Error al actualizar mantenimiento {Id}: {Message}", entity.Id, result.Message);

            return result;
        }

        public override async Task<OperationResult<Mantenimiento>> Delete(Mantenimiento entity)
        {
            var result = MantenimientoValidator.Validate(entity);
            if (!result.Success)
            {
                return result;
            }

            if (result.Success)
                _logger.LogInformation("Mantenimiento eliminado correctamente: {Id}", entity.Id);
            else
                _logger.LogError("Error al eliminar mantenimiento {Id}: {Message}", entity.Id, result.Message);

            return result;
        }

        public override async Task<OperationResult<Mantenimiento>> GetById(int id)
        {
            try
            {
                var entity = await _context.Mantenimiento
                    .Include(m => m.Habitacion) // relación con habitación
                    .FirstOrDefaultAsync(m => m.Id == id && !m.IsDeleted);

                if (entity == null)
                    return OperationResult<Mantenimiento>.Fail("Mantenimiento no encontrado");

                _logger.LogInformation("Mantenimiento encontrado: {Id}", id);
                return OperationResult<Mantenimiento>.Ok(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener mantenimiento por Id {Id}", id);
                return OperationResult<Mantenimiento>.Fail($"Error: {ex.Message}");
            }
        }

        public async Task<OperationResult<List<Mantenimiento>>> GetByHabitacionAsync(int habitacionId)
        {
            try
            {
                var mantenimientos = await _context.Mantenimiento
                    .Where(m => m.IdHabitacion == habitacionId && !m.IsDeleted)
                    .ToListAsync();

                if (!mantenimientos.Any())
                    return OperationResult<List<Mantenimiento>>.Fail("No se encontraron mantenimientos para esta habitación");

                return OperationResult<List<Mantenimiento>>.Ok(mantenimientos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener mantenimientos de habitación {HabitacionId}", habitacionId);
                return OperationResult<List<Mantenimiento>>.Fail($"Error: {ex.Message}");
            }
        }

        public async Task<OperationResult<List<Mantenimiento>>> GetByFechasAsync(DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                var mantenimientos = await _context.Mantenimiento
                    .Where(m => m.FechaInicio >= fechaInicio && m.FechaFin <= fechaFin && !m.IsDeleted)
                    .ToListAsync();

                if (!mantenimientos.Any())
                    return OperationResult<List<Mantenimiento>>.Fail("No se encontraron mantenimientos en este rango de fechas");

                return OperationResult<List<Mantenimiento>>.Ok(mantenimientos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener mantenimientos entre {FechaInicio} y {FechaFin}", fechaInicio, fechaFin);
                return OperationResult<List<Mantenimiento>>.Fail($"Error: {ex.Message}");
            }
        }

        public async Task<OperationResult<List<Mantenimiento>>> GetByEstadoAsync(string estado)
        {
            try
            {
                var mantenimientos = await _context.Mantenimiento
                    .Where(m => m.Estado == estado && !m.IsDeleted)
                    .ToListAsync();

                if (!mantenimientos.Any())
                    return OperationResult<List<Mantenimiento>>.Fail("No se encontraron mantenimientos con este estado");

                return OperationResult<List<Mantenimiento>>.Ok(mantenimientos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener mantenimientos con estado {Estado}", estado);
                return OperationResult<List<Mantenimiento>>.Fail($"Error: {ex.Message}");
            }
        }
    }
}
