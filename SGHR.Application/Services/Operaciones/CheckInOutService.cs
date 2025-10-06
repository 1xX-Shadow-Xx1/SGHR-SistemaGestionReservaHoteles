
using Microsoft.Extensions.Logging;
using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Operaciones.CheckInOut;
using SGHR.Application.Interfaces.Operaciones;
using SGHR.Domain.Entities.Configuration.Operaciones;
using SGHR.Persistence.Interfaces.Operaciones;

namespace SGHR.Application.Services.Operaciones
{
    public class CheckInOutService : ICheckInOutService
    {
        public readonly ILogger<CheckInOutService> _logger;
        public readonly ICheckInOutRepository _checkInOutRepository;

        public CheckInOutService(ICheckInOutRepository checkInOutRepository,
                                 ILogger<CheckInOutService> logger)
        {
            _checkInOutRepository = checkInOutRepository;
            _logger = logger;
        }

        public async Task<ServiceResult> GetAll()
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando obtención de todos los CheckInOut.");

            try
            {
                var opResult = await _checkInOutRepository.GetAll();
                if (opResult.Success)
                {
                    result.Success = true;
                    result.Data = opResult.Data;
                    result.Message = "CheckInOut obtenidos correctamente.";
                }
                else
                {
                    result.Success = false;
                    result.Message = "Error al obtener los CheckInOut.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los CheckInOut.");
                result.Success = false;
                result.Message = "Error al obtener los CheckInOut.";
            }
            return result;
        }

        public async Task<ServiceResult> GetById(int id)
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando obtención de CheckInOut por ID: {Id}", id);

            try
            {
                var opResult = await _checkInOutRepository.GetById(id);
                if (opResult.Success)
                {
                    result.Success = true;
                    result.Data = opResult.Data;
                    result.Message = "CheckInOut obtenido correctamente.";
                }
                else
                {
                    result.Success = false;
                    result.Message = "Error al obtener el CheckInOut.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el CheckInOut.");
                result.Success = false;
                result.Message = "Error al obtener el CheckInOut.";
            }
            return result;
        }

        public async Task<ServiceResult> Remove(DeleteCheckInOutDto deleteCheckInOutDto)
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando eliminación de CheckInOut con ID: {Id}", deleteCheckInOutDto.Id);

            try
            {
                var checkInOutExists = await _checkInOutRepository.GetById(deleteCheckInOutDto.Id);

                var opResult = await _checkInOutRepository.Delete(checkInOutExists.Data);
                if (opResult.Success)
                {
                    result.Success = true;
                    result.Message = "CheckInOut eliminado correctamente.";
                }
                else
                {
                    result.Success = false;
                    result.Message = "Error al eliminar el CheckInOut.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el CheckInOut.");
                result.Success = false;
                result.Message = "Error al eliminar el CheckInOut.";
            }
            return result;
        }

        public async Task<ServiceResult> Save(CreateCheckInOutDto createCheckInOutDto)
        {
            ServiceResult result= new ServiceResult();
            _logger.LogInformation("Iniciando creación de CheckInOut.", createCheckInOutDto);

            try
            {
                CheckInOut checkInOut = new CheckInOut
                {
                    IdReserva = createCheckInOutDto.IdReserva,
                    FechaCheckIn = DateTime.Now,
                    FechaCheckOut = null,
                    AtendidoPor = createCheckInOutDto.AtendidoPor
                };

                var opResult = await _checkInOutRepository.Save(checkInOut);
                if (opResult.Success)
                {
                    result.Success = true;
                    result.Data = opResult.Data;
                    result.Message = "CheckInOut creado correctamente.";
                }
                else
                {
                    result.Success = false;
                    result.Message = "Error al crear el CheckInOut.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el CheckInOut.");
                result.Success = false;
                result.Message = "Error al crear el CheckInOut.";
            }
            return result;
        }

        public async Task<ServiceResult> Update(UpdateCheckInOutDto updateCheckInOutDto)
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando actualización de CheckInOut.", updateCheckInOutDto);

            try
            {
                var checkInOutExists = await _checkInOutRepository.GetById(updateCheckInOutDto.Id);
                if (!checkInOutExists.Success)
                {
                    result.Success = false;
                    result.Message = "Error: el CheckInOut no existe";
                    return result;
                }

                CheckInOut checkInOut = checkInOutExists.Data;
                checkInOut.IdReserva = updateCheckInOutDto.IdReserva;
                checkInOut.FechaCheckIn = updateCheckInOutDto.FechaCheckIn;
                checkInOut.FechaCheckOut = updateCheckInOutDto.FechaCheckOut;
                checkInOut.AtendidoPor = updateCheckInOutDto.AtendidoPor;

                var opResult = await _checkInOutRepository.Update(checkInOut);
                if (opResult.Success)
                {
                    result.Success = true;
                    result.Data = opResult.Data;
                    result.Message = "CheckInOut actualizado correctamente.";
                }
                else
                {
                    result.Success = false;
                    result.Message = "Error al actualizar el CheckInOut.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el CheckInOut.");
                result.Success = false;
                result.Message = "Error al actualizar el CheckInOut.";
            }
            return result;
        }
    }
}
