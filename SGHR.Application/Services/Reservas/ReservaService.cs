
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
                    result.Message = "Error al obtener la reserva.";
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

        public async Task<ServiceResult> Remove(DeleteReservaDto deleteReservaDto)
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando eliminación de reserva.", deleteReservaDto);

            try
            {
                var reserva = await _reservaRepository.GetById(deleteReservaDto.Id);
                if (!reserva.Success)
                {
                    result.Success = false;
                    result.Message = "Error: la reserva no existe.";
                    return result;
                }

                var opResult = await _reservaRepository.Delete(reserva.Data);

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
                if (!result.Success)
                {
                    result.Success = false;
                    result.Message = "Error: la reserva no puede ser nula";
                    return result;
                }
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
                if (!result.Success)
                {
                    result.Success = false;
                    result.Message = "Error: la reserva no puede ser nula";
                    return result;
                }

                var existingReservaResult = await _reservaRepository.GetById(updateReservaDto.Id);
                if (!existingReservaResult.Success)
                {
                    result.Success = false;
                    result.Message = "Error: la reserva no existe.";
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
