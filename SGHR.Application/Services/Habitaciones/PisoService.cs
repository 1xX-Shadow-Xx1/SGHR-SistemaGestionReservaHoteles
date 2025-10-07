using Microsoft.Extensions.Logging;
using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Habitaciones.Piso;
using SGHR.Application.Interfaces.Habitaciones;
using SGHR.Domain.Entities.Configuration.Habitaciones;
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

        public async Task<ServiceResult> GetAll()
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando obtención de todos los pisos.");

            try
            {
                var opResult = await _pisoRepository.GetAll();
                if (opResult.Success)
                {
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

        public async Task<ServiceResult> GetById(int id)
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando obtención de piso por ID: {Id}", id);

            try
            {
                var opResult = await _pisoRepository.GetById(id);
                if (opResult.Success)
                {
                    result.Success = true;
                    result.Data = opResult.Data;
                    result.Message = "Piso obtenido correctamente.";
                }
                else
                {
                    result.Success = false;
                    result.Message = "Error al obtener el piso.";
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

        public async Task<ServiceResult> Remove(DeletePisoDto deletePisoDto)
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando eliminación de piso con ID: {Id}", deletePisoDto.Id);

            try
            {
                var piso = await _pisoRepository.GetById(deletePisoDto.Id);
                if (piso.Data == null)
                {
                    result.Success = false;
                    result.Message = "El piso no existe.";
                    return result;
                }

                var opResult = await _pisoRepository.Delete(piso.Data);
                if (opResult.Success)
                {
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

        public async Task<ServiceResult> Save(CreatePisoDto createPisoDto)
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


                var opResult = await _pisoRepository.Save(piso);
                if (opResult.Success)
                {
                    result.Success = true;
                    result.Data = opResult.Data;
                    result.Message = "Piso creado correctamente.";
                }
                else
                {
                    result.Success = false;
                    result.Message = "Error al crear el piso.";
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

        public async Task<ServiceResult> Update(UpdatePisoDto updatePisoDto)
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando actualización de piso con ID: {Id}", updatePisoDto.Id);

            try
            {
                var existingPisoResult = await _pisoRepository.GetById(updatePisoDto.Id);
                if (existingPisoResult.Data == null)
                {
                    result.Success = false;
                    result.Message = "El piso no existe.";
                    return result;
                }

                var pisoToUpdate = existingPisoResult.Data;
                pisoToUpdate.NumeroPiso = updatePisoDto.NumeroPiso;
                pisoToUpdate.Descripcion = updatePisoDto.Descripcion;
                pisoToUpdate.Estado = updatePisoDto.Estado;

                var opResult = await _pisoRepository.Update(pisoToUpdate);
                if (opResult.Success)
                {
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
