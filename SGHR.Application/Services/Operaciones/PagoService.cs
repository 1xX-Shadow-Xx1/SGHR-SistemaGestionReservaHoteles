
using Microsoft.Extensions.Logging;
using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Operaciones.Pago;
using SGHR.Application.Interfaces.Operaciones;
using SGHR.Domain.Entities.Configuration.Operaciones;
using SGHR.Persistence.Interfaces.Reportes;

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

        public async Task<ServiceResult> GetAll()
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando obtención de todos los pagos.");

            try
            {
                var opResult = await _pagoRepository.GetAll();
                if (opResult.Success)
                {
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

        public async Task<ServiceResult> GetById(int id)
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando obtención de pago por ID: {Id}", id);

            try
            {
                var opResult = await _pagoRepository.GetById(id);
                if (opResult.Success)
                {
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

        public async Task<ServiceResult> Remove(int id)
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando eliminación de pago con ID: {Id}", id);

            try
            {
                if (id <= 0)
                {
                    result.Success = false;
                    result.Message = "El pago debe tener un ID válido.";
                    return result;
                }
                var PagoExist = await _pagoRepository.GetById(id);
                if (!PagoExist.Success)
                {
                    result.Success = false;
                    result.Message = PagoExist.Message;
                    return result;
                }

                var opResult = await _pagoRepository.Delete(PagoExist.Data);
                if (opResult.Success)
                {
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

        public async Task<ServiceResult> Save(CreatePagoDto createPagoDto)
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

                var opResult = await _pagoRepository.Save(pago);
                if (opResult.Success)
                {
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

        public async Task<ServiceResult> Update(UpdatePagoDto updatePagoDto)
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando actualización de pago con ID: {Id}", updatePagoDto.Id);

            try
            {
                var existingPagoResult = await _pagoRepository.GetById(updatePagoDto.Id);
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

                var opResult = await _pagoRepository.Update(pago);
                if (opResult.Success)
                {
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
