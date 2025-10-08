
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

        public async Task<ServiceResult> GetAll()
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando obtención de todas las tarifas.");

            try
            {
                var opResult = await _tarifaRepository.GetAll();
                if (opResult.Success)
                {
                    result.Success = true;
                    result.Data = opResult.Data;
                    result.Message = "Tarifas obtenidas correctamente.";
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

        public async Task<ServiceResult> GetById(int id)
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando obtención de tarifa por ID: {Id}", id);

            try
            {
                var opResult = await _tarifaRepository.GetById(id);
                if (opResult.Success)
                {
                    result.Success = true;
                    result.Data = opResult.Data;
                    result.Message = "Tarifa obtenida correctamente.";
                }
                else
                {
                    result.Success = false;
                    result.Message = "Error al obtener la tarifa.";
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

        public async Task<ServiceResult> Remove(DeleteTarifaDto dto)
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando eliminación de tarifa ID: {Id}", dto.Id);

            try
            {
                var opResult = await _tarifaRepository.GetById(dto.Id);
                if (!opResult.Success)
                {
                    result.Success = false;
                    result.Message = "Error al obtener la tarifa.";
                    return result;
                }

                var entity = opResult.Data;
                if (entity == null)
                {
                    result.Success = false;
                    result.Message = "La tarifa no existe.";
                    return result;
                }

                var deleteResult = await _tarifaRepository.Delete(entity);
                if (deleteResult.Success)
                {
                    result.Success = true;
                    result.Message = "Tarifa eliminada correctamente.";
                    _logger.LogInformation("Tarifa eliminada correctamente: {Id}", dto.Id);
                }
                else
                {
                    result.Success = false;
                    result.Message = "Error al eliminar la tarifa.";
                    _logger.LogError("Error al eliminar la tarifa {Id}: {Message}", dto.Id, deleteResult.Message);
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

        public async Task<ServiceResult> Save(CreateTarifaDto dto)
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando creación de nueva tarifa.", dto);

            try
            {
                Tarifa tarifa = new Tarifa
                {
                    IdCategoria = dto.IdCategoria,
                    Temporada = dto.Temporada,
                    Precio = dto.Precio
                };


                var opResult = await _tarifaRepository.Save(tarifa);
                if (opResult.Success)
                {
                    result.Success = true;
                    result.Data = opResult.Data;
                    result.Message = "Tarifa creada correctamente.";
                    _logger.LogInformation("Tarifa creada correctamente: {Id} - {Precio}", tarifa.ID, tarifa.Precio);
                }
                else
                {
                    result.Success = false;
                    result.Message = "Error al crear la tarifa.";
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

        public async Task<ServiceResult> Update(UpdateTarifaDto dto)
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando actualización de tarifa ID: {Id}", dto.Id);

            try
            {
                var existingTarifaResult = await _tarifaRepository.GetById(dto.Id);
                if (!existingTarifaResult.Success)
                {
                    result.Success = false;
                    result.Message = "Error al obtener la tarifa.";
                    return result;
                }
                var existingTarifa = existingTarifaResult.Data;
                if (existingTarifa == null)
                {
                    result.Success = false;
                    result.Message = "La tarifa no existe.";
                    return result;
                }


                existingTarifa.IdCategoria = dto.IdCategoria;
                existingTarifa.Temporada = dto.Temporada;
                existingTarifa.Precio = dto.Precio;

                var updateResult = await _tarifaRepository.Update(existingTarifa);
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
