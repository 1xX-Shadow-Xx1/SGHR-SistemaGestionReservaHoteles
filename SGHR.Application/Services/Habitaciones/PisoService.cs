using Microsoft.Extensions.Logging;
using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Habitaciones.Piso;
using SGHR.Application.Interfaces.Habitaciones;
using SGHR.Domain.Entities.Configuration.Habitaciones;
using SGHR.Domain.Enum.Habitaciones;
using SGHR.Persistence.Interfaces.Habitaciones;

namespace SGHR.Application.Services.Categorias
{
    public class PisoService : IPisoService
    {
        public readonly ILogger<PisoService> _logger;
        public readonly IPisoRepository _pisoRepository;

        public PisoService(ILogger<PisoService> logger, IPisoRepository pisoRepository)
        {
            _logger = logger;
            _pisoRepository = pisoRepository;
        }

        public async Task<ServiceResult> GetAllAsync()
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando obtención de todos los pisos.");

            try
            {
                var opResult = await _pisoRepository.GetAllAsync();
                if (opResult.Success)
                {
                    _logger.LogInformation("Se obtuvieron {count} Pisos.", opResult.Data.Count);
                    result.Success = true;
                    result.Data = opResult.Data;
                    result.Message = "Pisos obtenidos correctamente.";
                }
                else
                {
                    result.Success = false;
                    result.Message = "Error al obtener los pisos.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los pisos.");
                result.Success = false;
                result.Message = "Error al obtener los pisos.";
            }
            return result;
        }
        public async Task<ServiceResult> GetByIdAsync(int id)
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando obtención de piso por ID: {Id}", id);

            try
            {
                var opResult = await _pisoRepository.GetByIdAsync(id);
                if (opResult.Success)
                {
                    _logger.LogInformation("Se obtuvo un piso con Id {id} correctamente.", id);
                    result.Success = true;
                    result.Data = opResult.Data;
                    result.Message = "Piso obtenido correctamente.";
                }
                else
                {
                    result.Success = false;
                    result.Message = opResult.Message;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el piso.");
                result.Success = false;
                result.Message = "Error al obtener el piso.";
            }
            return result;
        }
        public async Task<ServiceResult> DeleteAsync(int id, int? idsesion = null)
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando eliminación de piso con ID: {Id}", id);

            try
            {
                if (id < 0)
                {
                    result.Success = false;
                    result.Message = "El ID del piso no es válido.";
                    return result;
                }

                var PisoExist = await _pisoRepository.GetByIdAsync(id);
                if (!PisoExist.Success)
                {
                    result.Success = false;
                    result.Message = PisoExist.Message;
                    return result;
                }

                var opResult = await _pisoRepository.DeleteAsync(PisoExist.Data, idsesion);
                if (opResult.Success)
                {
                    _logger.LogInformation("Se a eliminado un Piso con Id {id} correctamente.", id);
                    result.Success = true;
                    result.Message = "Piso eliminado correctamente.";
                }
                else
                {
                    result.Success = false;
                    result.Message = "Error al eliminar el piso.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el piso.");
                result.Success = false;
                result.Message = "Error al eliminar el piso.";
            }
            return result;
        }
        public async Task<ServiceResult> CreateAsync(CreatePisoDto createPisoDto, int? idsesion = null)
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando creación de nuevo piso.", createPisoDto);

            try
            {

                Piso piso = new Piso
                {
                    NumeroPiso = createPisoDto.NumeroPiso,
                    Descripcion = createPisoDto.Descripcion
                };


                var opResult = await _pisoRepository.SaveAsync(piso, idsesion);
                if (opResult.Success)
                {
                    _logger.LogInformation("Se a creado un nuevo Piso numero {num} correctamente.", createPisoDto.NumeroPiso);
                    result.Success = true;
                    result.Data = opResult.Data;
                    result.Message = "Piso creado correctamente.";
                }
                else
                {
                    result.Success = false;
                    result.Message = opResult.Message;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el piso.");
                result.Success = false;
                result.Message = "Error al crear el piso.";
            }
            return result;
        }
        public async Task<ServiceResult> UpdateAsync(UpdatePisoDto updatePisoDto, int? idsesion = null)
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando actualización de piso con ID: {Id}", updatePisoDto.Id);

            try
            {
                var existingPisoResult = await _pisoRepository.GetByIdAsync(updatePisoDto.Id);
                if (existingPisoResult.Data == null)
                {
                    result.Success = false;
                    result.Message = existingPisoResult.Message;
                    return result;
                }

                var pisoToUpdate = existingPisoResult.Data;
                pisoToUpdate.NumeroPiso = updatePisoDto.NumeroPiso;
                pisoToUpdate.Descripcion = updatePisoDto.Descripcion;
                if (!string.IsNullOrWhiteSpace(updatePisoDto.Estado))
                {
                    if (Enum.TryParse(updatePisoDto.Estado, out EstadoPiso estado))
                    {
                        pisoToUpdate.Estado = estado;
                    }
                }

                var opResult = await _pisoRepository.UpdateAsync(pisoToUpdate, idsesion);
                if (opResult.Success)
                {
                    _logger.LogInformation("Se a actualizado un piso con Id {id} correctamente.", updatePisoDto.Id);
                    result.Success = true;
                    result.Data = opResult.Data;
                    result.Message = "Piso actualizado correctamente.";
                }
                else
                {
                    result.Success = false;
                    result.Message = "Error al actualizar el piso.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el piso.");
                result.Success = false;
                result.Message = "Error al actualizar el piso.";
            }
            return result;
        }
    }
}
