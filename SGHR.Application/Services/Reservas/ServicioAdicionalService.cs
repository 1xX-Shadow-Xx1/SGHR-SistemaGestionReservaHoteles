
using Microsoft.Extensions.Logging;
using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Reservas.ServicioAdicional;
using SGHR.Application.Interfaces.Reservas;
using SGHR.Domain.Entities.Configuration.Reservas;
using SGHR.Persistence.Interfaces.Reservas;

namespace SGHR.Application.Services.Reservas
{
    public class ServicioAdicionalService : IServicioAdicionalService
    {
        public readonly IServicioAdicionalRepository _servicioAdicionalRepository;
        public readonly ILogger<ServicioAdicionalService> _logger;

        public ServicioAdicionalService(IServicioAdicionalRepository servicioAdicionalRepository,
                                        ILogger<ServicioAdicionalService> logger)
        {
            _servicioAdicionalRepository = servicioAdicionalRepository;
            _logger = logger;
        }

        public async Task<ServiceResult> GetAll()
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando obtención de todos los servicios adicionales.");

            try
            {
                var opResult = await _servicioAdicionalRepository.GetAll();
                if (opResult.Success)
                {
                    result.Success = true;
                    result.Data = opResult.Data;
                    result.Message = "Servicios adicionales obtenidos correctamente.";
                }
                else
                {
                    result.Success = false;
                    result.Message = "Error al obtener los servicios adicionales.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los servicios adicionales.");
                result.Success = false;
                result.Message = "Error al obtener los servicios adicionales.";
            }
            return result;
        }

        public async Task<ServiceResult> GetById(int id)
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando obtención de servicio adicional por ID: {Id}", id);

            try
            {
                var opResult = await _servicioAdicionalRepository.GetById(id);
                if (opResult.Success)
                {
                    result.Success = true;
                    result.Data = opResult.Data;
                    result.Message = "Servicio adicional obtenido correctamente.";
                }
                else
                {
                    result.Success = false;
                    result.Message = "Error al obtener el servicio adicional.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el servicio adicional.");
                result.Success = false;
                result.Message = "Error al obtener el servicio adicional.";
            }
            return result;
        }

        public async Task<ServiceResult> Remove(DeleteServicioAdicionalDto deleteServicioAdicionalDto)
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando eliminación de servicio adicional.", deleteServicioAdicionalDto);

            try
            {
                if (!result.Success)
                {
                    result.Success = false;
                    result.Message = "Error: el servicio adicional no es válido.";
                    return result;
                }

                var existingServicioAdicional = await _servicioAdicionalRepository.GetById(deleteServicioAdicionalDto.Id);
                if (!existingServicioAdicional.Success || existingServicioAdicional.Data == null)
                {
                    result.Success = false;
                    result.Message = "Error: el servicio adicional no existe.";
                    return result;
                }

                var opResult = await _servicioAdicionalRepository.Delete(existingServicioAdicional.Data);
                if (opResult.Success)
                {
                    result.Success = true;
                    result.Data = opResult.Data;
                    result.Message = "Servicio adicional eliminado correctamente.";
                }
                else
                {
                    result.Success = false;
                    result.Message = "Error al eliminar el servicio adicional.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el servicio adicional.");
                result.Success = false;
                result.Message = "Error al eliminar el servicio adicional.";
            }
            return result;
        }

        public async Task<ServiceResult> Save(CreateServicioAdicionalDto createServicioAdicionalDto)
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando creación de servicio adicional.", createServicioAdicionalDto);

            try
            {
                if(!result.Success)
                {
                    result.Success = false;
                    result.Message = "Error: el servicio adicional no es válido.";
                    return result;
                }
                ServicioAdicional servicioAdicional = new ServicioAdicional
                {
                    Nombre = createServicioAdicionalDto.Nombre,
                    Descripcion = createServicioAdicionalDto.Descripcion,
                    Precio = createServicioAdicionalDto.Precio
                };
                var opResult = await _servicioAdicionalRepository.Save(servicioAdicional);
                if (opResult.Success)
                {
                    result.Success = true;
                    result.Data = opResult.Data;
                    result.Message = "Servicio adicional creado correctamente.";
                }
                else
                {
                    result.Success = false;
                    result.Message = "Error al crear el servicio adicional.";
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el servicio adicional.");
                result.Success = false;
                result.Message = "Error al crear el servicio adicional.";
            }
            return result;
        }

        public async Task<ServiceResult> Update(UpdateServicioAdicionalDto updateServicioAdicionalDto)
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando actualización de servicio adicional.", updateServicioAdicionalDto);

            try
            {
                if (!result.Success)
                {
                    result.Success = false;
                    result.Message = "Error: el servicio adicional no es válido.";
                    return result;
                }

                var existingServicioAdicional = await _servicioAdicionalRepository.GetById(updateServicioAdicionalDto.Id);
                if (!existingServicioAdicional.Success || existingServicioAdicional.Data == null)
                {
                    result.Success = false;
                    result.Message = "Error: el servicio adicional no existe.";
                    return result;
                }

                var servicioAdicional = existingServicioAdicional.Data;

                servicioAdicional.Nombre = updateServicioAdicionalDto.Nombre;
                servicioAdicional.Descripcion = updateServicioAdicionalDto.Descripcion;
                servicioAdicional.Precio = updateServicioAdicionalDto.Precio;

                var opResult = await _servicioAdicionalRepository.Update(servicioAdicional);
                if (opResult.Success)
                {
                    result.Success = true;
                    result.Data = opResult.Data;
                    result.Message = "Servicio adicional actualizado correctamente.";
                }
                else
                {
                    result.Success = false;
                    result.Message = "Error al actualizar el servicio adicional.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el servicio adicional.");
                result.Success = false;
                result.Message = "Error al actualizar el servicio adicional.";
            }
            return result;
        }
    }
}
