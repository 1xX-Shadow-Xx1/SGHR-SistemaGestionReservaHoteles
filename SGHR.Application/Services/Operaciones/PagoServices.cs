using Microsoft.Extensions.Logging;
using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Operaciones.Pago;
using SGHR.Application.Interfaces.Operaciones;
using SGHR.Domain.Entities.Configuration.Operaciones;
using SGHR.Domain.Enum.Operaciones;
using SGHR.Domain.Enum.Reservas;
using SGHR.Domain.Repository;
using SGHR.Persistence.Interfaces.Reportes;
using SGHR.Persistence.Interfaces.Users;

namespace SGHR.Application.Services.Operaciones
{
    public class PagoServices : IPagoServices
    {
        private readonly ILogger<PagoServices> _logger;
        private readonly IPagoRepository _pagoRepository;
        private readonly IReservaRepository _reservaRepository;
        private readonly IClienteRepository _clienteRepository;

        public PagoServices(ILogger<PagoServices> logger, 
                            IPagoRepository pagoRepository,
                            IReservaRepository reservaRepository,
                            IHabitacionRepository habitacionRepository,
                            IUsuarioRepository usuarioRepository,
                            IClienteRepository clienteRepository)
        {
            _logger = logger;
            _pagoRepository = pagoRepository;
            _reservaRepository = reservaRepository;
            _clienteRepository = clienteRepository;
        }

        public async Task<ServiceResult> RealizarPagoAsync(RealizarPagoDto realiarpagoDto)
        {
            ServiceResult result = new ServiceResult();

            if (realiarpagoDto == null)
            {
                result.Message = "El pago no puede ser nulo.";
                return result;
            }

            if (realiarpagoDto.Monto <= 0)
            {
                result.Message = "El monto del pago debe ser mayor a cero.";
                return result;
            }

            try
            {
                var reservaResult = await _reservaRepository.GetByIdAsync(realiarpagoDto.IdReserva);
                if (!reservaResult.Success || reservaResult.Data == null)
                {
                    result.Message = $"No se encontró la reserva con ID {realiarpagoDto.IdReserva}.";
                    return result;
                }

                var reserva = reservaResult.Data;

                if (reserva.Estado != EstadoReserva.Pendiente && reserva.Estado != EstadoReserva.Activa && reserva.Estado != EstadoReserva.PagoParcial)
                {
                    result.Message = $"No se puede realizar un pago a una reserva con estado {reserva.Estado}.";
                    return result;
                }

                var pagosResult = await _pagoRepository.GetAllAsync();
                if (!pagosResult.Success)
                {
                    result.Message = pagosResult.Message;
                    return result;
                }

                var pagosDeReserva = pagosResult.Data.Where(p => p.IdReserva == realiarpagoDto.IdReserva && p.Estado != EstadoPago.Rechazado).ToList();
                decimal totalPagado = Math.Round(pagosDeReserva.Sum(p => p.Monto), 2);

                if (totalPagado + realiarpagoDto.Monto > reserva.CostoTotal)
                {
                    result.Message = $"El monto excede el total de la reserva. Monto restante a pagar: {reserva.CostoTotal - totalPagado}";
                    return result;
                }

                EstadoPago estadoPago = (totalPagado + realiarpagoDto.Monto) >= reserva.CostoTotal
                    ? EstadoPago.Completado
                    : EstadoPago.Parcial;

                Pago nuevoPago = new Pago()
                {
                    IdReserva = realiarpagoDto.IdReserva,
                    Monto = realiarpagoDto.Monto,
                    MetodoPago = realiarpagoDto.MetodoPago,
                    FechaPago = DateTime.Now,
                    Estado = estadoPago
                };

                var saveResult = await _pagoRepository.SaveAsync(nuevoPago);
                if (!saveResult.Success)
                {
                    result.Message = saveResult.Message;
                    return result;
                }

                totalPagado += realiarpagoDto.Monto;

                if (totalPagado >= reserva.CostoTotal)
                {
                    reserva.Estado = EstadoReserva.Confirmada;
                    result.Message = "Pago completado. La reserva ha sido confirmada.";
                }
                else
                {
                    reserva.Estado = EstadoReserva.PagoParcial;
                    result.Message = $"Pago parcial registrado. Restan {(reserva.CostoTotal - totalPagado):C2} para completar el total.";
                }

                await _reservaRepository.UpdateAsync(reserva);

                result.Success = true;
                result.Data = new
                {
                    IdReserva = reserva.Id,
                    TotalPagado = totalPagado,
                    Restante = reserva.CostoTotal - totalPagado,
                    EstadoReserva = reserva.Estado.ToString(),
                    EstadoPago = nuevoPago.Estado.ToString()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en RealizarPago de PagoServices");
                result.Message = $"Error al realizar el pago: {ex.Message}";
            }

            return result;
        }
        public async Task<ServiceResult> ObtenerPagosAsync()
        {
            ServiceResult result = new ServiceResult();

            try
            {
                var pagosResult = await _pagoRepository.GetAllAsync();
                if (!pagosResult.Success || pagosResult.Data == null)
                {
                    result.Message = pagosResult.Message ?? "No se pudieron obtener los pagos.";
                    return result;
                }

                var pagosDto = pagosResult.Data
                    .OrderByDescending(p => p.FechaPago)
                    .Select(p => new PagoDto
                    {
                        Id = p.Id,
                        IdReserva = p.IdReserva,
                        Monto = p.Monto,
                        MetodoPago = p.MetodoPago,
                        FechaPago = p.FechaPago,
                        Estado = p.Estado.ToString()
                    })                    
                    .ToList();

                result.Success = true;
                result.Data = pagosDto;
                result.Message = "Pagos obtenidos correctamente.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en ObtenerPagosAsync de PagoServices");
                result.Message = $"Error al obtener los pagos: {ex.Message}";
            }

            return result;
        }
        public async Task<ServiceResult> ObtenerResumenPagosAsync()
        {
            ServiceResult result = new ServiceResult();

            try
            {
                var pagosResult = await _pagoRepository.GetAllAsync();
                if (!pagosResult.Success || pagosResult.Data == null)
                {
                    result.Message = pagosResult.Message ?? "No se pudieron obtener los pagos.";
                    return result;
                }

                var pagos = pagosResult.Data;

                // Totales por estado
                decimal totalRecaudado = pagos
                    .Where(p => p.Estado == EstadoPago.Completado || p.Estado == EstadoPago.Parcial)
                    .Sum(p => p.Monto);

                decimal totalRechazado = pagos
                    .Where(p => p.Estado == EstadoPago.Rechazado)
                    .Sum(p => p.Monto);

                // Cantidades por estado
                int cantidadPendientes = pagos.Count(p => p.Estado == EstadoPago.Pendiente);
                int cantidadCompletados = pagos.Count(p => p.Estado == EstadoPago.Completado);
                int cantidadParciales = pagos.Count(p => p.Estado == EstadoPago.Parcial);
                int cantidadRechazados = pagos.Count(p => p.Estado == EstadoPago.Rechazado);

                ResumenPagoDto resumenPago = new ResumenPagoDto
                {
                    TotalRecaudado = totalRecaudado,
                    TotalRechazado = totalRechazado,
                    Pendientes = cantidadPendientes,
                    Completados = cantidadCompletados,
                    Parciales = cantidadParciales,
                    Rechazados = cantidadRechazados,
                    TotalPagos = pagos.Count()
                };

                // Estructura del resultado
                result.Success = true;
                result.Message = "Resumen de pagos obtenido correctamente.";
                result.Data = resumenPago;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en ObtenerResumenPagosAsync");
                result.Message = $"Error al obtener el resumen de pagos: {ex.Message}";
            }

            return result;
        }
        public async Task<ServiceResult> AnularPagoAsync(int idPago)
        {
            ServiceResult result = new ServiceResult();

            try
            {
                // Buscar el pago
                var pagoResult = await _pagoRepository.GetByIdAsync(idPago);
                if (!pagoResult.Success || pagoResult.Data == null)
                {
                    result.Message = $"No se encontró el pago con ID {idPago}.";
                    return result;
                }

                var pago = pagoResult.Data;

                if (pago.Estado == EstadoPago.Rechazado)
                {
                    result.Message = "El pago ya fue rechazado o anulado anteriormente.";
                    return result;
                }

                // Obtener reserva asociada
                var reservaResult = await _reservaRepository.GetByIdAsync(pago.IdReserva);
                if (!reservaResult.Success || reservaResult.Data == null)
                {
                    result.Message = "No se encontró la reserva asociada a este pago.";
                    return result;
                }

                var reserva = reservaResult.Data;

                // Cambiar estado del pago
                pago.Estado = EstadoPago.Rechazado;
                var updatePago = await _pagoRepository.UpdateAsync(pago);
                if (!updatePago.Success)
                {
                    result.Message = updatePago.Message;
                    return result;
                }

                // Obtener todos los pagos válidos de la reserva
                var pagosResult = await _pagoRepository.GetAllAsync();
                if (!pagosResult.Success)
                {
                    result.Message = pagosResult.Message;
                    return result;
                }

                var pagosReserva = pagosResult.Data
                    .Where(p => p.IdReserva == reserva.Id && p.Estado != EstadoPago.Rechazado)
                    .ToList();

                decimal totalPagado = pagosReserva.Sum(p => p.Monto);

                // Actualizar estado de la reserva según los pagos válidos
                if (totalPagado == 0)
                {
                    reserva.Estado = EstadoReserva.Pendiente;
                }
                else if (totalPagado < reserva.CostoTotal)
                {
                    reserva.Estado = EstadoReserva.PagoParcial;
                }
                else
                {
                    reserva.Estado = EstadoReserva.Confirmada;
                }

                var updateReserva = await _reservaRepository.UpdateAsync(reserva);
                if (!updateReserva.Success)
                {
                    result.Message = updateReserva.Message;
                    return result;
                }

                // Resultado exitoso
                result.Success = true;
                result.Message = $"El pago con ID {idPago} ha sido anulado correctamente.";
                result.Data = new
                {
                    IdReserva = reserva.Id,
                    TotalPagadoActual = totalPagado,
                    EstadoReserva = reserva.Estado.ToString(),
                    EstadoPago = pago.Estado.ToString()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en AnularPagoAsync");
                result.Message = $"Error al anular el pago: {ex.Message}";
            }

            return result;
        }
        public async Task<ServiceResult> GetPagoByCliente(int idcliente)
        {
            ServiceResult result = new ServiceResult();
            try
            {
                var cliente = await _clienteRepository.GetByIdAsync(idcliente);
                if (!cliente.Success)
                {
                    result.Message = cliente.Message;
                    return result;
                }

                var lisReservas = await _reservaRepository.GetByClienteAsync(idcliente);
                if (!lisReservas.Success)
                {
                    result.Message = lisReservas.Message;
                    return result;
                }

                var reservas = lisReservas.Data.ToArray();

                var pagos = new List<Pago>();

                for (int i = 0; i <= lisReservas.Data.Count; i++)
                {
                    var lispagos = await _pagoRepository.GetByReservaAsync(reservas[i].Id);
                    if (!lispagos.Success)
                    {
                        result.Message = lispagos.Message;
                        return result;
                    }

                    for (int p = 0; p <= lispagos.Data.Count(); p++)
                    {
                        pagos.Add(lispagos.Data[p]);
                    }
                }

                var pagosDto = pagos
                    .Select(p => new PagoDto
                    {
                        Id = p.Id,
                        IdReserva = p.IdReserva,
                        Monto = p.Monto,
                        MetodoPago = p.MetodoPago,
                        FechaPago = p.FechaPago,
                        Estado = p.Estado.ToString()
                    })
                    .ToList();

                result.Success = true;
                result.Data = pagosDto;
                result.Message = "Pagos obtenidos correctamente.";
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return result;
        }
        public async Task<ServiceResult> GetPagoByIdAsync(int idPago)
        {
            ServiceResult result = new ServiceResult();
            try
            {
                if(idPago <= 0)
                {
                    result.Message = "El id del pago es inválido.";
                    return result;
                }
                var pagoResult = await _pagoRepository.GetByIdAsync(idPago);
                if (!pagoResult.Success || pagoResult.Data == null)
                {
                    result.Message = pagoResult.Message ?? $"No se encontró el pago con ID {idPago}.";
                    return result;
                }
                var pago = pagoResult.Data;
                var pagoDto = new PagoDto
                {
                    Id = pago.Id,
                    IdReserva = pago.IdReserva,
                    Monto = pago.Monto,
                    MetodoPago = pago.MetodoPago,
                    FechaPago = pago.FechaPago,
                    Estado = pago.Estado.ToString()
                };
                result.Success = true;
                result.Data = pagoDto;
                result.Message = "Pago obtenido correctamente.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en GetPagoByIdAsync de PagoServices");
                result.Message = $"Error al obtener el pago: {ex.Message}";
            }
            return result;
        }
        public async Task<ServiceResult> GetAllNoRechazados()
        {
            ServiceResult result = new ServiceResult();
            try
            {
                var pagosResult = await _pagoRepository.GetAllAsync();
                if (!pagosResult.Success || pagosResult.Data == null)
                {
                    result.Message = pagosResult.Message ?? "No se pudieron obtener los pagos.";
                    return result;
                }
                var pagosDto = pagosResult.Data
                    .Where(p => p.Estado != EstadoPago.Rechazado)
                    .OrderByDescending(p => p.FechaPago)
                    .Select(p => new PagoDto
                    {
                        Id = p.Id,
                        IdReserva = p.IdReserva,
                        Monto = p.Monto,
                        MetodoPago = p.MetodoPago,
                        FechaPago = p.FechaPago,
                        Estado = p.Estado.ToString()
                    })
                    .ToList();
                result.Success = true;
                result.Data = pagosDto;
                result.Message = "Pagos obtenidos correctamente.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en GetAllNoRechaced de PagoServices");
                result.Message = $"Error al obtener los pagos: {ex.Message}";
            }
            return result;
        }
    }
}
