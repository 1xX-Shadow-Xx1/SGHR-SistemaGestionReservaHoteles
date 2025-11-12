using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolPoliApp.Persistence.Base;
using SGHR.Domain.Base;
using SGHR.Domain.Entities.Configuration.Sesiones;
using SGHR.Persistence.Context;
using SGHR.Persistence.Interfaces.Sesiones;
using System.Runtime.InteropServices;
using System.Threading.Channels;

namespace SGHR.Persistence.Repositories.EF.Sesiones
{
    public class SesionRepository : BaseRepository<Sesion>, ISesionRepository
    {
        private readonly ILogger<SesionRepository> _logger;
        private readonly SGHRContext _context;

        public SesionRepository(SGHRContext context,
                                ILogger<SesionRepository> logger,
                                ILogger<BaseRepository<Sesion>> loggerBase) : base(context,loggerBase)
        {
            _context = context;
            _logger = logger;
        }

        public override async Task<OperationResult<Sesion>> SaveAsync(Sesion entity)
        {
            try
            {


                var result = await base.SaveAsync(entity);

                if (result.Success)
                    _logger.LogInformation("Sesion creada correctamente con Id {Id}", result.Data.Id);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear la sesión");
                return OperationResult<Sesion>.Fail("Ocurrió un error al crear la sesión");
            }
        }
        public override async Task<OperationResult<Sesion>> UpdateAsync(Sesion entity)
        {
            try
            {


                var result = await base.UpdateAsync(entity);

                if (result.Success)
                    _logger.LogInformation("Sesion actualizada correctamente con Id {Id}", result.Data.Id);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar la sesión");
                return OperationResult<Sesion>.Fail("Ocurrió un error al actualizar la sesión");
            }
        }
        public override async Task<OperationResult<Sesion>> DeleteAsync(Sesion entity)
        {
            try
            {
                var result = await base.DeleteAsync(entity);

                if (result.Success)
                    _logger.LogInformation("Sesion eliminada correctamente con Id {Id}", entity.Id);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar la sesión");
                return OperationResult<Sesion>.Fail("Ocurrió un error al eliminar la sesión");
            }
        }
        public async Task<OperationResult<List<Sesion>>> GetByUsuarioAsync(int usuarioId)
        {
            try
            {
                var sesiones = await _context.Sesiones
                    .Where(s => s.IdUsuario == usuarioId && !s.IsDeleted)
                    .ToListAsync();

                _logger.LogInformation("Se obtuvieron {Count} sesiones para el usuario {UsuarioId}", sesiones.Count, usuarioId);
                return OperationResult<List<Sesion>>.Ok(sesiones);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo sesiones para el usuario {UsuarioId}", usuarioId);
                return OperationResult<List<Sesion>>.Fail("Ocurrió un error al obtener las sesiones del usuario");
            }
        }
        public async Task<OperationResult<List<Sesion>>> GetActiveSessionsAsync()
        {
            try
            {
                var sesiones = await _context.Sesiones
                    .Where(s => s.Estado && !s.IsDeleted) 
                    .ToListAsync();

                _logger.LogInformation("Se obtuvieron {Count} sesiones activas", sesiones.Count);
                return OperationResult<List<Sesion>>.Ok(sesiones);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo sesiones activas");
                return OperationResult<List<Sesion>>.Fail("Ocurrió un error al obtener las sesiones activas");
            }
        }
        public async Task<OperationResult<Sesion>> GetActiveSesionByUserAsync(int usuarioId)
        {
            try
            {
                var sesiones = await _context.Sesiones
                    .Where(s => s.IdUsuario == usuarioId && !s.IsDeleted)
                    .ToListAsync();

                if (!sesiones.Any())
                {
                    _logger.LogWarning("No se encontraron sesiones para el usuario {UsuarioId}", usuarioId);
                    return OperationResult<Sesion>.Fail("No se encontraron sesiones para el usuario");
                }

                // Filtrar solo las sesiones activas
                var activeSesiones = sesiones.Where(s => s.Estado).ToList();

                if (!activeSesiones.Any())
                {
                    _logger.LogWarning("No se encontraron sesiones activas para el usuario {UsuarioId}", usuarioId);
                    return OperationResult<Sesion>.Fail("No se encontraron sesiones activas para el usuario");
                }

                // Obtener la sesión más reciente
                var ultimaSesion = activeSesiones.MaxBy(s => s.Id);

                // Cerrar todas las sesiones excepto la última
                foreach (var s in activeSesiones)
                {
                    if (s.Id != ultimaSesion.Id)
                    {
                        s.Estado = false;
                        s.FechaFin = DateTime.Now;
                        s.FechaActualizacion = DateTime.Now;
                    }
                }

                // Guardar los cambios en la BD
                _context.Sesiones.UpdateRange(activeSesiones);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Se encontraron {Count} sesiones activas para el usuario {UsuarioId}", activeSesiones.Count, usuarioId);
                return OperationResult<Sesion>.Ok(ultimaSesion, "Se obtuvo la sesión activa correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo todas las sesiones");
                return OperationResult<Sesion>.Fail("Ocurrió un error al obtener la sesión del usuario");
            }

        }

    }
}
