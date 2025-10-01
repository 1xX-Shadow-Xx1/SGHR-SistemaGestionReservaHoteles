using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SchoolPoliApp.Persistence.Base;
using SGHR.Domain.Base;
using SGHR.Domain.Entities.Configuration.Reportes;
using SGHR.Persistence.Contex;
using SGHR.Persistence.Interfaces.Reportes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGHR.Persistence.Repositories.EF.Operaciones
{
    public sealed class ReporteRepository : BaseRepository<Reporte>, IReporteRepository
    {

        private readonly SGHRContext _context;
        private readonly ILogger<ReporteRepository> _logger;
        private readonly IConfiguration _configuration;

        public ReporteRepository(SGHRContext context,
                                 ILogger<ReporteRepository> logger,
                                 IConfiguration configuration) : base(context)
        {
            _context = context;
            _logger = logger;
            _configuration = configuration;
        }

        public override async Task<OperationResult<Reporte>> Save(Reporte entity)
        {
            var result = await base.Save(entity);

            if (result.Success)
                _logger.LogInformation("Reporte creado: {Id} - Tipo {Tipo}", entity.Id, entity.TipoReporte);
            else
                _logger.LogError("Error al crear Reporte: {Message}", result.Message);

            return result;
        }

        public override async Task<OperationResult<Reporte>> Update(Reporte entity)
        {
            var result = await base.Update(entity);

            if (result.Success)
                _logger.LogInformation("Reporte actualizado: {Id}", entity.Id);
            else
                _logger.LogError("Error al actualizar Reporte {Id}: {Message}", entity.Id, result.Message);

            return result;
        }

        public override async Task<OperationResult<Reporte>> Delete(Reporte entity)
        {
            var result = await base.Delete(entity);

            if (result.Success)
                _logger.LogInformation("Reporte eliminado correctamente): {Id}", entity.Id);
            else
                _logger.LogError("Error al eliminar Reporte {Id}: {Message}", entity.Id, result.Message);

            return result;
        }

        public override async Task<OperationResult<Reporte>> GetById(int id)
        {
            try
            {
                var entity = await _context.Reportes
                    .Include(r => r.Usuario) 
                    .FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted);

                if (entity == null)
                    return OperationResult<Reporte>.Fail("Reporte no encontrado");

                _logger.LogInformation("Reporte encontrado: {Id}", id);
                return OperationResult<Reporte>.Ok(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener reporte por Id {Id}", id);
                return OperationResult<Reporte>.Fail($"Error: {ex.Message}");
            }
        }

        public async Task<OperationResult<List<Reporte>>> GetByTipoAsync(string tipo)
        {
            try
            {
                var reportes = await _context.Reportes
                    .Where(r => r.TipoReporte == tipo && !r.IsDeleted)
                    .Include(r => r.Usuario)
                    .ToListAsync();

                if (!reportes.Any())
                    return OperationResult<List<Reporte>>.Fail("No se encontraron reportes de este tipo");

                return OperationResult<List<Reporte>>.Ok(reportes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener reportes por tipo {Tipo}", tipo);
                return OperationResult<List<Reporte>>.Fail($"Error: {ex.Message}");
            }
        }

        public async Task<OperationResult<List<Reporte>>> GetByFechasAsync(DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                var reportes = await _context.Reportes
                    .Where(r => r.FechaGeneracion >= fechaInicio && r.FechaGeneracion <= fechaFin && !r.IsDeleted)
                    .Include(r => r.Usuario)
                    .ToListAsync();

                if (!reportes.Any())
                    return OperationResult<List<Reporte>>.Fail("No se encontraron reportes en este rango de fechas");

                return OperationResult<List<Reporte>>.Ok(reportes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener reportes entre {FechaInicio} y {FechaFin}", fechaInicio, fechaFin);
                return OperationResult<List<Reporte>>.Fail($"Error: {ex.Message}");
            }
        }

        // Obtener reportes generados por un usuario específico
        public async Task<OperationResult<List<Reporte>>> GetByUsuarioAsync(int usuarioId)
        {
            try
            {
                var reportes = await _context.Reportes
                    .Where(r => r.GeneradoPor == usuarioId && !r.IsDeleted)
                    .ToListAsync();

                if (!reportes.Any())
                    return OperationResult<List<Reporte>>.Fail("No se encontraron reportes de este usuario");

                return OperationResult<List<Reporte>>.Ok(reportes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener reportes del usuario {UsuarioId}", usuarioId);
                return OperationResult<List<Reporte>>.Fail($"Error: {ex.Message}");
            }
        }
    }
}
