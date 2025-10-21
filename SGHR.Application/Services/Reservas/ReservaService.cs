
using Microsoft.Extensions.Logging;
using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Reservas.Reserva;
using SGHR.Application.Interfaces.Reservas;
using SGHR.Domain.Entities.Configuration.Reservas;
using SGHR.Domain.Repository;

namespace SGHR.Application.Services.Reservas
{
    public class ReservaService : IReservaService
    {
        private readonly IReservaRepository _reservaRepository;
        private readonly ILogger<ReservaService> _logger;

        public ReservaService(IReservaRepository reservaRepository, 
                              ILogger<ReservaService> logger)
        {
            _reservaRepository = reservaRepository;
            _logger = logger;
        }

        public async Task<ServiceResult> GetAll()
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando obtención de todas las reservas.");

            try
            {
                var opResult = await _reservaRepository.GetAll();
                if (opResult.Success)
                {
                    result.Success = true;
                    result.Data = opResult.Data;
                    result.Message = "Reservas obtenidas correctamente.";
                }
                else
                {
                    result.Success = false;
                    result.Message = "Error al obtener las reservas.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener las reservas.");
                result.Success = false;
                result.Message = "Error al obtener las reservas.";
            }
            return result;
        }

        public async Task<ServiceResult> GetById(int id)
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando obtención de reserva por ID: {Id}", id);

            try
            {
                var opResult = await _reservaRepository.GetById(id);
                if (opResult.Success)
                {
                    result.Success = true;
                    result.Data = opResult.Data;
                    result.Message = "Reserva obtenida correctamente.";
                }
                else
                {
                    result.Success = false;
                    result.Message = opResult.Message;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la reserva.");
                result.Success = false;
                result.Message = "Error al obtener la reserva.";
            }
            return result;
        }

        public async Task<ServiceResult> Remove(int id)
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando eliminación de reserva con ID: {Id}", id);

            try
            {
                if(id <= 0)
                {
                    result.Success = false;
                    result.Message = "El ID de la reserva no es válido.";
                    return result;
                }
                var reservaExist = await _reservaRepository.GetById(id);
                if (!reservaExist.Success)
                {
                    result.Success = false;
                    result.Message = reservaExist.Message;
                    return result;
                }

                var opResult = await _reservaRepository.Delete(reservaExist.Data);

                if (opResult.Success)
                {
                    result.Success = true;
                    result.Data = opResult.Data;
                    result.Message = "Reserva eliminada correctamente.";
                }
                else
                {
                    result.Success = false;
                    result.Message = "Error al eliminar la reserva.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar la reserva.");
                result.Success = false;
                result.Message = "Error al eliminar la reserva.";
            }

            return result;
        }

        public async Task<ServiceResult> Save(CreateReservaDto createReservaDto)
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando creación de reserva.", createReservaDto);

            try
            {
                Reserva reserva = new Reserva
                {
                    FechaInicio = createReservaDto.FechaInicio,
                    FechaFin = createReservaDto.FechaFin,
                    IdHabitacion = createReservaDto.IdHabitacion,
                    IdUsuario = createReservaDto.IdUsuario,
                    CostoTotal = createReservaDto.CostoTotal,
                    Estado = "Pendiente"
                };
                var opResult = await _reservaRepository.Save(reserva);

                if (opResult.Success)
                {
                    result.Success = true;
                    result.Data = opResult.Data;
                    result.Message = opResult.Message;
                }
                else
                {
                    result.Success = false;
                    result.Message = opResult.Message;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear la reserva.");
                result.Success = false;
                result.Message = "Error al crear la reserva.";
            }
            return result;
        }

        public async Task<ServiceResult> Update(UpdateReservaDto updateReservaDto)
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando actualización de reserva.", updateReservaDto);

            try
            {
                var existingReservaResult = await _reservaRepository.GetById(updateReservaDto.Id);
                if (!existingReservaResult.Success)
                {
                    result.Success = false;
                    result.Message = existingReservaResult.Message;
                    return result;
                }

                var existingReserva = existingReservaResult.Data;

                existingReserva.FechaInicio = updateReservaDto.FechaInicio;
                existingReserva.FechaFin = updateReservaDto.FechaFin;
                existingReserva.IdHabitacion = updateReservaDto.IdHabitacion;
                existingReserva.CostoTotal = updateReservaDto.CostoTotal;
                existingReserva.Estado = updateReservaDto.Estado;

                var opResult = await _reservaRepository.Update(existingReserva);
                if (opResult.Success)
                {
                    result.Success = true;
                    result.Message = "Reserva actualizada correctamente.";
                }
                else
                {
                    result.Success = false;
                    result.Message = "Error al actualizar la reserva.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar la reserva.");
                result.Success = false;
                result.Message = "Error al actualizar la reserva.";
            }
            return result;
        }
    }
}
