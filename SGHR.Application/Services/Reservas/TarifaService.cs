
using Microsoft.Extensions.Logging;
using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Reservas.Tarifa;
using SGHR.Application.Interfaces.Reservas;
using SGHR.Domain.Entities.Configuration.Reservas;
using SGHR.Persistence.Interfaces.Reservas;

namespace SGHR.Application.Services.Reservas
{
    public class TarifaService : ITarifaService
    {
        public readonly ILogger<TarifaService> _logger;
        public readonly ITarifaRepository _tarifaRepository;
        public TarifaService(ITarifaRepository tarifaRepository,
                             ILogger<TarifaService> logger)
        {
            _tarifaRepository = tarifaRepository;
            _logger = logger;
        }

        public async Task<ServiceResult> CreateAsync(CreateTarifaDto CreateDto, int? sesionId = null)
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando creación de nueva tarifa.", CreateDto);

            try
            {
                _logger.LogInformation("Verificando que no exista una tarifa igual.");
                var exitsTarifa = await _tarifaRepository.ExistsAsync(T => T.IdCategoria == CreateDto.IdCategoria &&
                                                                      T.Temporada == CreateDto.Temporada &&
                                                                      T.Precio == CreateDto.Precio);
                if (!exitsTarifa.Success)
                {
                    _logger.LogWarning("Ya existe una tarifa igual.");
                    result.Success = false;
                    result.Message = ($"Ya existe una tarifa con esos parametros.");
                    return result;
                }

                Tarifa tarifa = new Tarifa
                {
                    IdCategoria = CreateDto.IdCategoria,
                    Temporada = CreateDto.Temporada,
                    Precio = CreateDto.Precio
                };


                var opResult = await _tarifaRepository.SaveAsync(tarifa, sesionId);
                if (opResult.Success)
                {
                    result.Success = true;
                    result.Data = opResult.Data;
                    result.Message = "Tarifa creada correctamente.";
                    _logger.LogInformation("Tarifa creada correctamente: {Id} - {Precio}", opResult.Data.Id, tarifa.Precio);
                }
                else
                {
                    result.Success = false;
                    result.Message = opResult.Message;
                    _logger.LogError("Error al crear la tarifa: {Message}", opResult.Message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear la tarifa.");
                result.Success = false;
                result.Message = "Error al crear la tarifa.";
            }
            return result;
        }
        public async Task<ServiceResult> DeleteAsync(int id, int? sesionId = null)
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando eliminación de tarifa ID: {Id}", id);

            try
            {
                if (id < 0)
                {
                    result.Success = false;
                    result.Message = "El ID de la tarifa no es válido.";
                    return result;
                }
                var TarifaExist = await _tarifaRepository.GetByIdAsync(id);
                if (!TarifaExist.Success)
                {
                    result.Success = false;
                    result.Message = TarifaExist.Message;
                    return result;
                }

                var deleteResult = await _tarifaRepository.DeleteAsync(TarifaExist.Data, sesionId);
                if (deleteResult.Success)
                {
                    result.Success = true;
                    result.Message = "Tarifa eliminada correctamente.";
                    _logger.LogInformation("Tarifa eliminada correctamente: {Id}", id);
                }
                else
                {
                    result.Success = false;
                    result.Message = ($"Error al eliminar la tarifa: {deleteResult.Message}");
                    _logger.LogError("Error al eliminar la tarifa {Id}: {Message}", id, deleteResult.Message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar la tarifa.");
                result.Success = false;
                result.Message = "Error al eliminar la tarifa.";
            }
            return result;
        }
        public async Task<ServiceResult> GetAllAsync()
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando obtención de todas las tarifas.");

            try
            {
                var opResult = await _tarifaRepository.GetAllAsync();
                if (opResult.Success)
                {
                    result.Success = true;
                    result.Data = opResult.Data;
                    result.Message = "Tarifas obtenidas exitosamente.";
                }
                else
                {
                    result.Success = false;
                    result.Message = "Error al obtener las tarifas.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener las tarifas.");
                result.Success = false;
                result.Message = "Error al obtener las tarifas.";
            }
            return result;
        }
        public async Task<ServiceResult> GetByIdAsync(int id)
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando obtención de tarifa por ID: {Id}", id);

            try
            {
                var opResult = await _tarifaRepository.GetByIdAsync(id);
                if (opResult.Success)
                {
                    result.Success = true;
                    result.Data = opResult.Data;
                    result.Message = "Tarifa obtenida correctamente.";
                }
                else
                {
                    result.Success = false;
                    result.Message = opResult.Message;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la tarifa.");
                result.Success = false;
                result.Message = "Error al obtener la tarifa.";
            }
            return result;
        }
        public async Task<ServiceResult> UpdateAsync(UpdateTarifaDto dto, int? sesionId = null)
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando actualización de tarifa ID: {Id}", dto.Id);

            try
            {
                var existingTarifaResult = await _tarifaRepository.GetByIdAsync(dto.Id);
                if (!existingTarifaResult.Success)
                {
                    result.Success = false;
                    result.Message = existingTarifaResult.Message;
                    return result;
                }

                var existingTarifa = existingTarifaResult.Data;
                existingTarifa.IdCategoria = dto.IdCategoria;
                existingTarifa.Temporada = dto.Temporada;
                existingTarifa.Precio = dto.Precio;

                var updateResult = await _tarifaRepository.UpdateAsync(existingTarifa, sesionId);
                if (updateResult.Success)
                {
                    result.Success = true;
                    result.Data = updateResult.Data;
                    result.Message = "Tarifa actualizada correctamente.";
                    _logger.LogInformation("Tarifa actualizada correctamente: {Id}", dto.Id);
                }
                else
                {
                    result.Success = false;
                    result.Message = "Error al actualizar la tarifa.";
                    _logger.LogError("Error al actualizar la tarifa {Id}: {Message}", dto.Id, updateResult.Message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar la tarifa.");
                result.Success = false;
                result.Message = "Error al actualizar la tarifa.";
            }
            return result;
        }
    }
}
