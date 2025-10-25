using Microsoft.Extensions.Logging;
using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Operaciones.Mantenimiento;
using SGHR.Application.Interfaces.Operaciones;
using SGHR.Domain.Entities.Configuration.Operaciones;
using SGHR.Domain.Enum.Operaciones;
using SGHR.Persistence.Interfaces.Operaciones;

namespace SGHR.Application.Services.Operaciones
{
    public class MantenimientoService : IMantenimientoService
    {
        public readonly ILogger<MantenimientoService> _logger;
        public readonly IMantenimientoRepository _mantenimientoRepository;

        public MantenimientoService(ILogger<MantenimientoService> logger, IMantenimientoRepository mantenimientoRepository)
        {
            _logger = logger;
            _mantenimientoRepository = mantenimientoRepository;
        }

        public async Task<ServiceResult> GetAllAsync()
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando obtención de todos los mantenimientos.");

            try
            {
                var opResult = await _mantenimientoRepository.GetAllAsync();
                if (opResult.Success)
                {
                    _logger.LogInformation("Se obtuvieron {count} mantenimientos.", opResult.Data.Count);
                    result.Success = true;
                    result.Data = opResult.Data;
                    result.Message = "Mantenimientos obtenidos correctamente.";
                }
                else
                {
                    result.Success = false;
                    result.Message = "Error al obtener los mantenimientos.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los mantenimientos.");
                result.Success = false;
                result.Message = "Error al obtener los mantenimientos.";
            }
            return result;
        }
        public async Task<ServiceResult> GetByIdAsync(int id)
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando obtención de mantenimiento por ID: {Id}", id);

            try
            {
                var opResult = await _mantenimientoRepository.GetByIdAsync(id);
                if (opResult.Success)
                {
                    _logger.LogInformation("Se obtuvo una mantenimiento con Id {id}.",id);
                    result.Success = true;
                    result.Data = opResult.Data;
                    result.Message = "Mantenimiento obtenido correctamente.";
                }
                else
                {
                    result.Success = false;
                    result.Message = opResult.Message;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el mantenimiento.");
                result.Success = false;
                result.Message = "Error al obtener el mantenimiento.";
            }
            return result;
        }
        public async Task<ServiceResult> DeleteAsync(int id, int? idsesion = null)
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando eliminación de mantenimiento con ID: {Id}", id);

            try
            {
                if (id < 0)
                {
                    result.Success = false;
                    result.Message = "El ID de mantenimiento no es válido.";
                    return result;
                }

                var MantenimientoExist = await  _mantenimientoRepository.GetByIdAsync(id);
                if (!MantenimientoExist.Success)
                {
                    result.Success = false;
                    result.Message = MantenimientoExist.Message;
                    return result;
                }

                var opResult =  _mantenimientoRepository.DeleteAsync(MantenimientoExist.Data, idsesion);
                if (opResult.Result.Success)
                {
                    _logger.LogInformation("Se elimino una mantenimiento con Id {id}.", id);
                    result.Success = true;
                    result.Message = "Mantenimiento eliminado correctamente.";
                }
                else
                {
                    result.Success = false;
                    result.Message = "Error al eliminar el mantenimiento.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el mantenimiento.");
                result.Success = false;
                result.Message = "Error al eliminar el mantenimiento.";
            }
            return result;
        }
        public async Task<ServiceResult> CreateAsync(CreateMantenimientoDto createMantenimientoDto, int? idsesion = null)
        {
            ServiceResult   result = new ServiceResult();
            _logger.LogInformation("Iniciando creación de nuevo mantenimiento.", createMantenimientoDto);

            try
            {
                Mantenimiento mantenimiento = new Mantenimiento
                {
                    IdPiso = createMantenimientoDto.IdPiso,
                    IdHabitacion = createMantenimientoDto.IdHabitacion,
                    Descripcion = createMantenimientoDto.Descripcion,
                    RealizadoPor = createMantenimientoDto.RealizadoPor,
                    FechaInicio = createMantenimientoDto.FechaInicio
                };

                var opResult = await _mantenimientoRepository.SaveAsync(mantenimiento);
                if (opResult.Success)
                {
                    result.Success = true;
                    result.Data = opResult.Data;
                    result.Message = "Mantenimiento creado correctamente.";
                }
                else
                {
                    result.Success = false;
                    result.Message = opResult.Message;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el mantenimiento.");
                result.Success = false;
                result.Message = "Error al crear el mantenimiento.";
            }
            return result;
        }
        public async Task<ServiceResult> UpdateAsync(UpdateMantenimientoDto updateMantenimientoDto, int? idsesion = null)
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando actualización de mantenimiento con ID: {Id}", updateMantenimientoDto.Id);

            try
            {
                var existingMantenimientoResult = await _mantenimientoRepository.GetByIdAsync(updateMantenimientoDto.Id);
                if (existingMantenimientoResult == null || !existingMantenimientoResult.Success)
                {
                    result.Success = false;
                    result.Message = existingMantenimientoResult.Message;
                    return result;
                }

                var existingMantenimiento = existingMantenimientoResult.Data;
                existingMantenimiento.IdPiso = updateMantenimientoDto.IdPiso;
                existingMantenimiento.IdHabitacion = updateMantenimientoDto.IdHabitacion;
                existingMantenimiento.Descripcion = updateMantenimientoDto.Descripcion;
                existingMantenimiento.FechaInicio = updateMantenimientoDto.FechaInicio;
                existingMantenimiento.FechaFin = updateMantenimientoDto.FechaFin;
                existingMantenimiento.RealizadoPor = updateMantenimientoDto.RealizadoPor;
                if (!string.IsNullOrWhiteSpace(updateMantenimientoDto.Estado))
                {
                    if (Enum.TryParse(updateMantenimientoDto.Estado, out EstadoMantenimiento estado))
                    {
                        existingMantenimiento.Estado = estado;
                    }
                }

                var opResult = await _mantenimientoRepository.UpdateAsync(existingMantenimiento, idsesion);
                if (opResult.Success)
                {
                    _logger.LogInformation("Se actualizado el mantenimiento con Id {id}.", updateMantenimientoDto.Id);
                    result.Success = true;
                    result.Data = opResult.Data;
                    result.Message = "Mantenimiento actualizado correctamente.";
                }
                else
                {
                    result.Success = false;
                    result.Message = "Error al actualizar el mantenimiento.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el mantenimiento.");
                result.Success = false;
                result.Message = "Error al actualizar el mantenimiento.";
            }
            return result;
        }
    }
}
