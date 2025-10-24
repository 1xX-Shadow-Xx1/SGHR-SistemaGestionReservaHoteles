using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolPoliApp.Persistence.Base;
using SGHR.Domain.Base;
using SGHR.Domain.Entities.Configuration.Sesiones;
using SGHR.Persistence.Contex;
using SGHR.Persistence.Interfaces.Sesiones;

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

        public override async Task<OperationResult<Sesion>> SaveAsync(Sesion entity, int? sesionId = null)
        {
            try
            {
                if (sesionId.HasValue)
                    entity.SesionCreacionId = sesionId;

                var result = await base.SaveAsync(entity, sesionId);

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

        public override async Task<OperationResult<Sesion>> UpdateAsync(Sesion entity, int? sesionId = null)
        {
            try
            {
                if (sesionId.HasValue)
                    entity.SesionActualizacionId = sesionId;

                var result = await base.UpdateAsync(entity, sesionId);

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

        public override async Task<OperationResult<Sesion>> DeleteAsync(Sesion entity, int? sesionId = null)
        {
            try
            {
                var result = await base.DeleteAsync(entity, sesionId);

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
                    .Where(s => s.IdUsuario == usuarioId && !s.Eliminado)
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
                    .Where(s => s.Estado && !s.Eliminado) 
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

    }
}
