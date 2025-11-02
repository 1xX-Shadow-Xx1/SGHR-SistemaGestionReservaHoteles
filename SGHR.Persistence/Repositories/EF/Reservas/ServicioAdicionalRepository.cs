using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolPoliApp.Persistence.Base;
using SGHR.Domain.Base;
using SGHR.Domain.Entities.Configuration.Reservas;
using SGHR.Domain.Validators.ConfigurationRules.Reservas;
using SGHR.Persistence.Context;
using SGHR.Persistence.Interfaces.Reservas;

namespace SGHR.Persistence.Repositories.EF.Reservas
{
    public sealed class ServicioAdicionalRepository : BaseRepository<ServicioAdicional>, IServicioAdicionalRepository
    {
        private readonly SGHRContext _context;
        private readonly ILogger<ServicioAdicionalRepository> _logger;
        private readonly ServicioAdicionalValidator _validator;

        public ServicioAdicionalRepository(SGHRContext context,
                                           ServicioAdicionalValidator servicioAdicionalvalidator,
                                           ILogger<ServicioAdicionalRepository> logger,
                                           ILogger<BaseRepository<ServicioAdicional>> loggerBase) : base(context, loggerBase)
        {
            _context = context;
            _logger = logger;
            _validator = servicioAdicionalvalidator;
        }

        public override async Task<OperationResult<ServicioAdicional>> SaveAsync(ServicioAdicional entity)
        {
            if (!_validator.Validate(entity, out string errorMessage))
            {
                _logger.LogWarning("Fallo al guardar el ServicioAdicional: {fail}", errorMessage);
                return OperationResult<ServicioAdicional>.Fail(errorMessage);
            }
            try
            {
                var result = await base.SaveAsync(entity);
                _logger.LogInformation("Servicio adicional {Nombre} guardado exitosamente", entity.Nombre);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error guardando servicio adicional {Nombre}", entity.Nombre);
                return OperationResult<ServicioAdicional>.Fail("Error guardando el servicio adicional");
            }
        }
        public override async Task<OperationResult<ServicioAdicional>> UpdateAsync(ServicioAdicional entity)
        {
            if (!_validator.Validate(entity, out string errorMessage))
            {
                _logger.LogWarning("Fallo al actualizar el ServicioAdicional: {fail}", errorMessage);
                return OperationResult<ServicioAdicional>.Fail(errorMessage);
            }

            try
            {
                var result = await base.UpdateAsync(entity);
                _logger.LogInformation("Servicio adicional {Nombre} actualizado exitosamente", entity.Nombre);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error actualizando servicio adicional {Nombre}", entity.Nombre);
                return OperationResult<ServicioAdicional>.Fail("Error actualizando el servicio adicional");
            }
        }
        public override async Task<OperationResult<ServicioAdicional>> DeleteAsync(ServicioAdicional entity)
        {
            try
            {
                var result = await base.DeleteAsync(entity);
                _logger.LogInformation("Servicio adicional {Nombre} eliminado exitosamente", entity.Nombre);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error eliminando servicio adicional {Nombre}", entity.Nombre);
                return OperationResult<ServicioAdicional>.Fail("Error eliminando el servicio adicional");
            }
        }
        public async Task<OperationResult<ServicioAdicional>> GetByNombreAsync(string nombre)
        {
            try
            {
                var servicio = await _context.ServicioAdicional
                    .FirstOrDefaultAsync(s => s.Nombre == nombre && !s.IsDeleted);

                if (servicio == null)
                {
                    _logger.LogWarning("Servicio adicional con nombre {Nombre} no encontrado", nombre);
                    return OperationResult<ServicioAdicional>.Fail("Servicio adicional no encontrado");
                }

                _logger.LogInformation("Servicio adicional obtenido: {Nombre}", nombre);
                return OperationResult<ServicioAdicional>.Ok(servicio);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo servicio adicional por nombre {Nombre}", nombre);
                return OperationResult<ServicioAdicional>.Fail("Ocurrió un error al obtener el servicio adicional");
            }
        }

    }
}
