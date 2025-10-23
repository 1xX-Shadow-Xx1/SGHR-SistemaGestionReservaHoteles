using Microsoft.Extensions.Logging;
using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Operaciones.Auditory;
using SGHR.Application.Interfaces.Operaciones;
using SGHR.Domain.Entities.Configuration.Operaciones;
using SGHR.Persistence.Interfaces.Operaciones;

namespace SGHR.Application.Services.Operaciones
{
    public class AuditoryService : IAuditoryService
    {
        public readonly ILogger<AuditoryService> _logger;
        public readonly IAuditoryRepository _auditoryRepository;

        public AuditoryService(ILogger<AuditoryService> logger,
                               IAuditoryRepository auditoryRepository)
        {
            _logger = logger;
            _auditoryRepository = auditoryRepository;
        }

        public async Task<ServiceResult> GetAll()
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando obtención de todas las auditorías.");

            try
            {
                var opResult = await _auditoryRepository.GetAll();
                if (opResult.Success)
                {
                    result.Success = true;
                    result.Data = opResult.Data;
                    result.Message = "Auditorías obtenidas correctamente.";
                }
                else
                {
                    result.Success = false;
                    result.Message = "Error al obtener las auditorías.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener las auditorías.");
                result.Success = false;
                result.Message = "Error al obtener las auditorías.";
            }   
            return result;
        }

        public async Task<ServiceResult> GetById(int id)
        {
            ServiceResult result= new ServiceResult();
            _logger.LogInformation("Iniciando obtención de auditoría por ID: {Id}", id);

            try
            {
                var opResult = await _auditoryRepository.GetById(id);
                if (opResult.Success)
                {
                    result.Success = true;
                    result.Data = opResult.Data;
                    result.Message = "Auditoría obtenida correctamente.";
                }
                else
                {
                    result.Success = false;
                    result.Message = opResult.Message;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la auditoría.");
                result.Success = false;
                result.Message = "Error al obtener la auditoría.";
            }
            return result;
        }

        public async Task<ServiceResult> Remove(int id)
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando eliminación de auditoría con ID: {Id}", id);

            try
            {
                if (id <= 0)
                {
                    result.Success = false;
                    result.Message = "El ID de la auditoría no es válido.";
                    return result;
                }

                var AuditoryExist = await _auditoryRepository.GetById(id);
                if (!AuditoryExist.Success)
                {
                    result.Success = false;
                    result.Message = AuditoryExist.Message;
                    return result;
                }

                var opResult = await _auditoryRepository.Delete(AuditoryExist.Data);
                if (opResult.Success)
                {
                    result.Success = true;
                    result.Message = "Auditoría eliminada correctamente.";
                }
                else
                {
                    result.Success = false;
                    result.Message = "Error al eliminar la auditoría.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar la auditoría.");
                result.Success = false;
                result.Message = "Error al eliminar la auditoría.";
            }
            return result;
        }

        public async Task<ServiceResult> Save(CreateAuditoryDto createAuditoryDto)
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando creación de auditoría.", createAuditoryDto);

            try
            {
                Auditory auditory = new Auditory
                {
                    IdUsuario = createAuditoryDto.IdUsuario,
                    Operacion = createAuditoryDto.Operacion,
                    Detalle = createAuditoryDto.Detalle
                };

                var opResult = await _auditoryRepository.Save(auditory);
                if (opResult.Success)
                {
                    result.Success = true;
                    result.Data = opResult.Data;
                    result.Message = "Auditoría creada correctamente.";
                }
                else
                {
                    result.Success = false;
                    result.Message = opResult.Message;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear la auditoría.");
                result.Success = false;
                result.Message = "Error al crear la auditoría.";
            }
            return result;
        }

        public async Task<ServiceResult> Update(UpdateAuditoryDto updateAuditoryDto)
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando actualización de auditoría con ID: {Id}", updateAuditoryDto.Id);

            try
            {
                var existingAuditoryResult = await _auditoryRepository.GetById(updateAuditoryDto.Id);
                if (!existingAuditoryResult.Success || existingAuditoryResult.Data == null)
                {
                    result.Success = false;
                    result.Message = existingAuditoryResult.Message;
                    return result;
                }

                var auditory = existingAuditoryResult.Data;
                auditory.IdUsuario = updateAuditoryDto.IdUsuario;
                auditory.Operacion = updateAuditoryDto.Operacion;
                auditory.Fecha = updateAuditoryDto.Fecha;
                auditory.Detalle = updateAuditoryDto.Detalle;

                var opResult = await _auditoryRepository.Update(auditory);
                if (opResult.Success)
                {
                    result.Success = true;
                    result.Data = opResult.Data;
                    result.Message = "Auditoría actualizada correctamente.";
                }
                else
                {
                    result.Success = false;
                    result.Message = "Error al actualizar la auditoría.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar la auditoría.");
                result.Success = false;
                result.Message = "Error al actualizar la auditoría.";
            }
            return result;
        }
    }
}
