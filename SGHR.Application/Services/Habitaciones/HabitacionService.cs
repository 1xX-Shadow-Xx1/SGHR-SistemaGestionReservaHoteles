using Microsoft.Extensions.Logging;
using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Habitaciones.Habitacion;
using SGHR.Application.Interfaces.Habitaciones;
using SGHR.Domain.Entities.Configuration.Habitaciones;
using SGHR.Domain.Repository;

namespace SGHR.Application.Services.Categorias
{
    public class HabitacionService : IHabitacionService
    {
        public readonly ILogger<HabitacionService> _logger;
        public readonly IHabitacionRepository _habitacionRepository;

        public HabitacionService(ILogger<HabitacionService> logger, IHabitacionRepository habitacionRepository)
        {
            _logger = logger;
            _habitacionRepository = habitacionRepository;
        }

        public async Task<ServiceResult> GetAll()
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando obtención de todas las habitaciones.");

            try
            {
                var opResult = await _habitacionRepository.GetAll();
                if (opResult.Success)
                {
                    result.Success = true;
                    result.Data = opResult.Data;
                    result.Message = "Habitaciones obtenidas correctamente.";
                }
                else
                {
                    result.Success = false;
                    result.Message = "Error al obtener las habitaciones.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener las habitaciones.");
                result.Success = false;
                result.Message = "Error al obtener las habitaciones.";
            }
            return result;
        }

        public async Task<ServiceResult> GetById(int id)
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando obtención de habitación por ID: {Id}", id);

            try
            {
                var opResult = await _habitacionRepository.GetById(id);
                if (opResult.Success)
                {
                    result.Success = true;
                    result.Data = opResult.Data;
                    result.Message = "Habitación obtenida correctamente.";
                }
                else
                {
                    result.Success = false;
                    result.Message = opResult.Message;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la habitación.");
                result.Success = false;
                result.Message = "Error al obtener la habitación.";
            }
            return result;
        }

        public async Task<ServiceResult> Remove(int id)
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando eliminación de habitación con ID: {Id}", id);

            try
            {
                if(id <= 0)
                {
                    result.Success = false;
                    result.Message = "ID de habitación inválido.";
                    return result;
                }

                var HabitacionExist = await _habitacionRepository.GetById(id);
                if (!HabitacionExist.Success)
                {
                    result.Success = false;
                    result.Message = HabitacionExist.Message;
                    return result;
                }

                var opResult = await _habitacionRepository.Delete(HabitacionExist.Data);
                if (opResult.Success)
                {
                    result.Success = true;
                    result.Message = "Habitación eliminada correctamente.";
                }
                else
                {
                    result.Success = false;
                    result.Message = "Error al eliminar la habitación.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar la habitación.");
                result.Success = false;
                result.Message = "Error al eliminar la habitación.";
            }
            return result;
        }

        public async Task<ServiceResult> Save(CreateHabitacionDto createHabitacionDto)
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando creación de nueva habitación.", createHabitacionDto);
            
            try
            {
                Habitacion habitacion = new Habitacion
                {
                    IdPiso = createHabitacionDto.IdPiso,
                    IdCategoria = createHabitacionDto.IdCategoria,
                    Numero = createHabitacionDto.Numero,
                    Capacidad = createHabitacionDto.Capacidad
                };

                var opResult = await _habitacionRepository.Save(habitacion);
                if (opResult.Success)
                {
                    result.Success = true;
                    result.Data = opResult.Data;
                    result.Message = "Habitación creada correctamente.";
                }
                else
                {
                    result.Success = false;
                    result.Message = opResult.Message;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear la habitación.");
                result.Success = false;
                result.Message = "Error al crear la habitación.";
            }
            return result;
        }

        public async Task<ServiceResult> Update(UpdateHabitacionDto updateHabitacionDto)
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando actualización de habitación con ID: {Id}", updateHabitacionDto.Id);

            try
            {
                var existingHabitacionResult = await _habitacionRepository.GetById(updateHabitacionDto.Id);
                if (!existingHabitacionResult.Success)
                {
                    result.Success = false;
                    result.Message = existingHabitacionResult.Message;
                    return result;
                }

                var habitacion = existingHabitacionResult.Data;
                habitacion.IdPiso = updateHabitacionDto.IdPiso;
                habitacion.Numero = updateHabitacionDto.Numero;
                habitacion.Capacidad = updateHabitacionDto.Capacidad;
                habitacion.Estado = updateHabitacionDto.Estado;

                var opResult = await _habitacionRepository.Update(habitacion);
                if (opResult.Success)
                {
                    result.Success = true;
                    result.Data = opResult.Data;
                    result.Message = "Habitación actualizada correctamente.";
                }
                else
                {
                    result.Success = false;
                    result.Message = "Error al actualizar la habitación.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar la habitación.");
                result.Success = false;
                result.Message = "Error al actualizar la habitación.";
            }
            return result;
        }
    }
}
