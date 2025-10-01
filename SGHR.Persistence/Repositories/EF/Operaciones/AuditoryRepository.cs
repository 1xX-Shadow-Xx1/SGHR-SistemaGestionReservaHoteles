using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
    public sealed class AuditoryRepository : BaseRepository<Auditory>, IAuditoryRepository
    {
        private readonly SGHRContext _context;
        private readonly ILogger<AuditoryRepository> _logger;
        private readonly IConfiguration _configuration;

        public AuditoryRepository(SGHRContext context,
                                  ILogger<AuditoryRepository> logger,
                                  IConfiguration configuration) : base(context)
        {
            _context = context;
            _logger = logger;
            _configuration = configuration;
        }

        public override async Task<OperationResult<Auditory>> Save(Auditory entity)
        {
            var result = await base.Save(entity);

            if (result.Success)
                _logger.LogInformation("Auditoría registrada: {Id} - Acción {Accion}", entity.Id, entity.Detalle);
            else
                _logger.LogError("Error al registrar auditoría: {Message}", result.Message);

            return result;
        }

        public override async Task<OperationResult<Auditory>> Update(Auditory entity)
        {
            var result = await base.Update(entity);

            if (result.Success)
                _logger.LogInformation("Auditoría actualizada: {Id}", entity.Id);
            else
                _logger.LogError("Error al actualizar auditoría {Id}: {Message}", entity.Id, result.Message);

            return result;
        }

        public override async Task<OperationResult<Auditory>> Delete(Auditory entity)
        {
            var result = await base.Delete(entity);

            if (result.Success)
                _logger.LogInformation("Auditoría eliminada correctamente: {Id}", entity.Id);
            else
                _logger.LogError("Error al eliminar auditoría {Id}: {Message}", entity.Id, result.Message);

            return result;
        }

        public override async Task<OperationResult<Auditory>> GetById(int id)
        {
            try
            {
                var entity = await _context.Auditory
                    .Include(a => a.Usuario)
                    .FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted);

                if (entity == null)
                    return OperationResult<Auditory>.Fail("Auditoría no encontrada");

                return OperationResult<Auditory>.Ok(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener auditoría por Id {Id}", id);
                return OperationResult<Auditory>.Fail($"Error: {ex.Message}");
            }
        }

        public async Task<OperationResult<List<Auditory>>> GetByUsuarioAsync(int usuarioId)
        {
            try
            {
                var lista = await _context.Auditory
                    .Where(a => a.IdUsuario == usuarioId && !a.IsDeleted)
                    .OrderByDescending(a => a.Fecha)
                    .ToListAsync();

                if (!lista.Any())
                    return OperationResult<List<Auditory>>.Fail("El usuario no tiene registros de auditoría");

                return OperationResult<List<Auditory>>.Ok(lista);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener auditorías del usuario {UsuarioId}", usuarioId);
                return OperationResult<List<Auditory>>.Fail($"Error: {ex.Message}");
            }
        }
        public async Task<OperationResult<List<Auditory>>> GetByAccionAsync(string accion)
        {
            try
            {
                var lista = await _context.Auditory
                    .Where(a => a.Operacion == accion && !a.IsDeleted)
                    .OrderByDescending(a => a.Fecha)
                    .ToListAsync();

                if (!lista.Any())
                    return OperationResult<List<Auditory>>.Fail("No se encontraron auditorías con esa acción");

                return OperationResult<List<Auditory>>.Ok(lista);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener auditorías por acción {Accion}", accion);
                return OperationResult<List<Auditory>>.Fail($"Error: {ex.Message}");
            }
        }
        public async Task<OperationResult<List<Auditory>>> GetByFechaAsync(DateTime inicio, DateTime fin)
        {
            try
            {
                var lista = await _context.Auditory
                    .Where(a => a.Fecha >= inicio && a.Fecha <= fin && !a.IsDeleted)
                    .OrderByDescending(a => a.Fecha)
                    .ToListAsync();

                if (!lista.Any())
                    return OperationResult<List<Auditory>>.Fail("No se encontraron auditorías en este rango de fechas");

                return OperationResult<List<Auditory>>.Ok(lista);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener auditorías entre {Inicio} y {Fin}", inicio, fin);
                return OperationResult<List<Auditory>>.Fail($"Error: {ex.Message}");
            }
        }

        public async Task<OperationResult<List<Auditory>>> GetUltimasAsync(int cantidad)
        {
            try
            {
                var lista = await _context.Auditory
                    .Where(a => !a.IsDeleted)
                    .OrderByDescending(a => a.Fecha)
                    .Take(cantidad)
                    .ToListAsync();

                if (!lista.Any())
                    return OperationResult<List<Auditory>>.Fail("No se encontraron auditorías recientes");

                return OperationResult<List<Auditory>>.Ok(lista);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener últimas {Cantidad} auditorías", cantidad);
                return OperationResult<List<Auditory>>.Fail($"Error: {ex.Message}");
            }
        }
    }
}
