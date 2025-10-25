
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Reservas.Reserva;
using SGHR.Application.Dtos.Configuration.Users.Usuario;
using SGHR.Application.Interfaces.Reservas;
using SGHR.Domain.Entities.Configuration.Reservas;
using SGHR.Domain.Entities.Configuration.Usuers;
using SGHR.Domain.Enum.Habitacion;
using SGHR.Domain.Enum.Reservas;
using SGHR.Domain.Enum.Usuario;
using SGHR.Domain.Repository;

namespace SGHR.Application.Services.Reservas
{
    public class ReservaService : IReservaService
    {
        private readonly IReservaRepository _reservaRepository;
        private readonly ILogger<ReservaService> _logger;
        private readonly IHabitacionRepository _habitacionRepository;

        public ReservaService(IReservaRepository reservaRepository, 
                              ILogger<ReservaService> logger,
                              IHabitacionRepository habitacionRepository)
        {
            _reservaRepository = reservaRepository;
            _logger = logger;
            _habitacionRepository = habitacionRepository;
        }

        public async Task<ServiceResult> GetAllAsync()
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando obtención de todas las reservas.");

            try
            {
                var opResult = await _reservaRepository.GetAllAsync();
                if (opResult.Success)
                {
                    _logger.LogInformation($"Se obtuvieron {opResult.Data.Count} reservas.");
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
        public async Task<ServiceResult> GetByIdAsync(int id)
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando obtención de reserva por ID: {Id}", id);

            try
            {
                var opResult = await _reservaRepository.GetByIdAsync(id);
                if (opResult.Success)
                {
                    _logger.LogInformation("Reserva obtenida correctamente.");
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
        public async Task<ServiceResult> DeleteAsync(int id, int? idsesion)
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando eliminación de reserva con ID: {Id}", id);

            try
            {
                if(id < 0)
                {
                    result.Success = false;
                    result.Message = "El ID de la reserva no es válido.";
                    return result;
                }
                var reservaExist = await _reservaRepository.GetByIdAsync(id);
                if (!reservaExist.Success)
                {
                    result.Success = false;
                    result.Message = reservaExist.Message;
                    return result;
                }
                if(reservaExist.Data.Estado == EstadoReserva.Activa || reservaExist.Data.Estado == EstadoReserva.Confirmada)
                {
                    result.Success =false;
                    result.Message = "No se puede borrar una reserva que ya este confirmada o activa.";
                    return result;
                }

                var opResult = await _reservaRepository.DeleteAsync(reservaExist.Data);

                if (opResult.Success)
                {
                    _logger.LogInformation("Se elimino una reserva correctamente.");
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
        public async Task<ServiceResult> CreateAsync(CreateReservaDto createReservaDto, int? idsesion)
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando creación de reserva.", createReservaDto);
            result.Success = false;

            try
            {
                var OpReservas = await _reservaRepository.GetAllAsync();

                var existReserva = OpReservas.Data.Where(r => r.FechaInicio >= createReservaDto.FechaInicio &&
                                                                   r.FechaFin <= createReservaDto.FechaFin &&
                                                                   r.IdHabitacion == createReservaDto.IdHabitacion)
                                                  .ToList();
                if (existReserva.Count != 0)
                {
                    result.Message = "Ya existe una reserva para esa fecha.";
                    return result;
                }

                var HabitacionReservada = await _habitacionRepository.GetByIdAsync(createReservaDto.IdHabitacion);

                switch (HabitacionReservada.Data.Estado)
                {
                    case EstadoHabitacion.EnMantenimiento:
                        result.Message = "La habitacion no se puede reservar por que esta en manteniminto.";
                        return result;
                        
                    case EstadoHabitacion.Reservada:
                        result.Message = "La habitacion no se puede reservar por que esta reservada.";
                        return result;

                    case EstadoHabitacion.Limpieza:
                        result.Message = "La habitacion no se puede reservar por que esta en limpieza";
                        return result;

                    default:
                        break;
                }

                

                Reserva reserva = new Reserva
                {
                    FechaInicio = createReservaDto.FechaInicio,
                    FechaFin = createReservaDto.FechaFin,
                    IdHabitacion = createReservaDto.IdHabitacion,
                    IdUsuario = createReservaDto.IdUsuario,
                    CostoTotal = createReservaDto.CostoTotal
                };

                var opResult = await _reservaRepository.SaveAsync(reserva);

                if (opResult.Success)
                {
                    _logger.LogInformation($"Se creo una reservacion para la habitacion {reserva.IdHabitacion} para el dia {reserva.FechaInicio} hasta {reserva.FechaFin}.");
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
        public async Task<ServiceResult> UpdateAsync(UpdateReservaDto updateReservaDto, int? idsesion)
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando actualización de reserva.", updateReservaDto);

            try
            {
                var existingReservaResult = await _reservaRepository.GetByIdAsync(updateReservaDto.Id);
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
                if (!string.IsNullOrWhiteSpace(updateReservaDto.Estado))
                {
                    if (Enum.TryParse(updateReservaDto.Estado, out EstadoReserva estado))
                    {
                        existingReserva.Estado = estado;
                    }
                }

                

                var opResult = await _reservaRepository.UpdateAsync(existingReserva);
                if (opResult.Success)
                {
                    _logger.LogInformation("La reserva fue actualizada correctamente.");
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
