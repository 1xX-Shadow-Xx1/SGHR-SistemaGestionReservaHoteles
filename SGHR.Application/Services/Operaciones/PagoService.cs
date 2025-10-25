
using Microsoft.Extensions.Logging;
using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Operaciones.Pago;
using SGHR.Application.Interfaces.Operaciones;
using SGHR.Domain.Entities.Configuration.Operaciones;
using SGHR.Persistence.Interfaces.Reportes;
using System.Data;

namespace SGHR.Application.Services.Operaciones
{
    public class PagoService : IPagoService
    {
        public readonly ILogger<PagoService> _logger;
        public readonly IPagoRepository _pagoRepository;

        public PagoService(ILogger<PagoService> logger, IPagoRepository pagoRepository)
        {
            _logger = logger;
            _pagoRepository = pagoRepository;
        }

        public async Task<ServiceResult> GetAllAsync()
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando obtención de todos los pagos.");

            try
            {
                var opResult = await _pagoRepository.GetAllAsync();
                if (opResult.Success)
                {
                    _logger.LogInformation("Se obtuvo una lista de {count} pagos.", opResult.Data.Count);
                    result.Success = true;
                    result.Data = opResult.Data;
                    result.Message = "Pagos obtenidos correctamente.";
                }
                else
                {
                    result.Success = false;
                    result.Message = "Error al obtener los pagos.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los pagos.");
                result.Success = false;
                result.Message = "Error al obtener los pagos.";
            }
            return result;
        }
        public async Task<ServiceResult> GetByIdAsync(int id)
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando obtención de pago por ID: {Id}", id);

            try
            {
                var opResult = await _pagoRepository.GetByIdAsync(id);
                if (opResult.Success)
                {
                    _logger.LogInformation("Se obtuvo un pago con Id {id}.",id);
                    result.Success = true;
                    result.Data = opResult.Data;
                    result.Message = "Pago obtenido correctamente.";
                }
                else
                {
                    result.Success = false;
                    result.Message = opResult.Message;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el pago.");
                result.Success = false;
                result.Message = "Error al obtener el pago.";
            }
            return result;
        }
        public async Task<ServiceResult> DeleteAsync(int id, int? idsesion = null)
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando eliminación de pago con ID: {Id}", id);

            try
            {
                if (id < 0)
                {
                    result.Success = false;
                    result.Message = "El pago debe tener un ID válido.";
                    return result;
                }
                var PagoExist = await _pagoRepository.GetByIdAsync(id);
                if (!PagoExist.Success)
                {
                    result.Success = false;
                    result.Message = PagoExist.Message;
                    return result;
                }

                var opResult = await _pagoRepository.DeleteAsync(PagoExist.Data,idsesion);
                if (opResult.Success)
                {
                    _logger.LogInformation("Se elimino el pago con Id {id}", id);
                    result.Success = true;
                    result.Message = "Pago eliminado correctamente.";
                }
                else
                {
                    result.Success = false;
                    result.Message = "Error al eliminar el pago.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el pago.");
                result.Success = false;
                result.Message = "Error al eliminar el pago.";
            }
            return result;
        }
        public async Task<ServiceResult> CreateAsync(CreatePagoDto createPagoDto, int? idsesion = null)
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando creación de nuevo pago.", createPagoDto);

            try
            {
                Pago pago = new Pago
                {
                    IdReserva = createPagoDto.IdReserva,
                    Monto = createPagoDto.Monto,
                    MetodoPago = createPagoDto.MetodoPago,
                };

                var opResult = await _pagoRepository.SaveAsync(pago, idsesion);
                if (opResult.Success)
                {
                    _logger.LogInformation("Se registro un pago a la reserva con Id {id}.", createPagoDto.IdReserva);
                    result.Success = true;
                    result.Data = opResult.Data;
                    result.Message = "Pago creado correctamente.";
                }
                else
                {
                    result.Success = false;
                    result.Message = opResult.Message;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el pago.");
                result.Success = false;
                result.Message = "Error al crear el pago.";
            }
            return result;
        }
        public async Task<ServiceResult> UpdateAsync(UpdatePagoDto updatePagoDto, int? idsesion = null)
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando actualización de pago con Id: {Id}", updatePagoDto.Id);

            try
            {
                var existingPagoResult = await _pagoRepository.GetByIdAsync(updatePagoDto.Id);
                if (existingPagoResult == null || !existingPagoResult.Success)
                {
                    result.Success = false;
                    result.Message = existingPagoResult.Message;
                    return result;
                }

                Pago pago = existingPagoResult.Data;
                pago.IdReserva = updatePagoDto.IdReserva;
                pago.Monto = updatePagoDto.Monto;
                pago.MetodoPago = updatePagoDto.MetodoPago;
                pago.FechaPago = updatePagoDto.FechaPago;

                var opResult = await _pagoRepository.UpdateAsync(pago);
                if (opResult.Success)
                {
                    _logger.LogInformation("Se actualizo el pago de la reserva con Id {id}.", updatePagoDto.IdReserva);
                    result.Success = true;
                    result.Data = opResult.Data;
                    result.Message = "Pago actualizado correctamente.";
                }
                else
                {
                    result.Success = false;
                    result.Message = "Error al actualizar el pago.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el pago.");
                result.Success = false;
                result.Message = "Error al actualizar el pago.";
            }
            return result;
        }
    }
}
