using Microsoft.Extensions.Logging;
using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Habitaciones.Habitacion;
using SGHR.Application.Dtos.Configuration.Reservas.Reserva;
using SGHR.Application.Interfaces.Habitaciones;
using SGHR.Domain.Entities.Configuration.Habitaciones;
using SGHR.Domain.Enum.Habitacion;
using SGHR.Domain.Enum.Reservas;
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

        public async Task<ServiceResult> GetAllAsync()
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando obtención de todas las habitaciones.");

            try
            {
                var opResult = await _habitacionRepository.GetAllAsync();
                if (opResult.Success)
                {
                    _logger.LogInformation("Se obtuvieron {count} habitaciones correctamente.",opResult.Data.Count);
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
        public async Task<ServiceResult> GetByIdAsync(int id)
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando obtención de habitación por ID: {Id}", id);

            try
            {
                var opResult = await _habitacionRepository.GetByIdAsync(id);
                if (opResult.Success)
                {
                    _logger.LogInformation("Se obtuvo una habitacion con Id {id} correctamente.", id);
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
        public async Task<ServiceResult> DeleteAsync(int id, int? idsesion = null)
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando eliminación de habitación con ID: {Id}", id);

            try
            {
                if(id < 0)
                {
                    result.Success = false;
                    result.Message = "ID de habitación inválido.";
                    return result;
                }

                var HabitacionExist = await _habitacionRepository.GetByIdAsync(id);
                if (!HabitacionExist.Success)
                {
                    result.Success = false;
                    result.Message = HabitacionExist.Message;
                    return result;
                }

                var opResult = await _habitacionRepository.DeleteAsync(HabitacionExist.Data, idsesion);
                if (opResult.Success)
                {
                    _logger.LogInformation("Se a eliminado una habitacion con Id {id} correctamente.", id);
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
        public async Task<ServiceResult> CreateAsync(CreateHabitacionDto createHabitacionDto, int? idsesion = null)
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando creación de nueva habitación.", createHabitacionDto);
            
            try
            {
                Habitacion habitacion = new Habitacion
                {
                    IdPiso = createHabitacionDto.IdPiso,
                    IdCategoria = createHabitacionDto.IdCategoria,
                    IdAmenity = createHabitacionDto.IdAmenity,
                    Numero = createHabitacionDto.Numero,
                    Capacidad = createHabitacionDto.Capacidad
                };

                var opResult = await _habitacionRepository.SaveAsync(habitacion, idsesion);
                if (opResult.Success)
                {
                    _logger.LogInformation("Se a creado una nueva habitacion en la categoria con id {id} correctamente.", habitacion.IdCategoria);
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
        public async Task<ServiceResult> UpdateAsync(UpdateHabitacionDto updateHabitacionDto, int? idsesion = null)
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando actualización de habitación con ID: {Id}", updateHabitacionDto.Id);

            try
            {
                var existingHabitacionResult = await _habitacionRepository.GetByIdAsync(updateHabitacionDto.Id);
                if (!existingHabitacionResult.Success)
                {
                    result.Success = false;
                    result.Message = existingHabitacionResult.Message;
                    return result;
                }

                var habitacion = existingHabitacionResult.Data;
                habitacion.IdPiso = updateHabitacionDto.IdPiso;
                habitacion.Numero = updateHabitacionDto.Numero;
                habitacion.IdAmenity = updateHabitacionDto.IdAmenity;
                habitacion.IdCategoria = updateHabitacionDto.IdCategoria;
                habitacion.Capacidad = updateHabitacionDto.Capacidad;
                if (!string.IsNullOrWhiteSpace(updateHabitacionDto.Estado))
                {
                    if (Enum.TryParse(updateHabitacionDto.Estado, out EstadoHabitacion estado))
                    {
                        habitacion.Estado = estado;
                    }
                }

                var opResult = await _habitacionRepository.UpdateAsync(habitacion, idsesion);
                if (opResult.Success)
                {
                    _logger.LogInformation("Se a actualizado una habitacion con id {id} correctamente.", habitacion.Id);
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
