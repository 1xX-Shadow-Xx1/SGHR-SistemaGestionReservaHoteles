

using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Dashboard;
using SGHR.Application.Dtos.Configuration.Operaciones.Pago;
using SGHR.Application.Dtos.Configuration.Reservas.Reserva;
using SGHR.Application.Interfaces;
using SGHR.Application.Interfaces.Operaciones;
using SGHR.Application.Interfaces.Reservas;

namespace SGHR.Application.Services
{
    public class DashboardServices : IDashboardServices
    {
        private readonly IPagoServices _pagoServices;
        private readonly IReservaServices _reservaServices;

        public DashboardServices(IPagoServices pagoServices, IReservaServices reservaServices)
        {
            _pagoServices = pagoServices;
            _reservaServices = reservaServices;
        }

        public async Task<ServiceResult> GetDashboardDataAsync()
        {
            ServiceResult result = new ServiceResult();
            try
            {
                var opResultResumen = await _pagoServices.ObtenerResumenPagosAsync();
                if(!opResultResumen.Success)
                {
                    result.Message = opResultResumen.Message;
                    return result;
                }
                var opResulReserva = await _reservaServices.GetAllAsync();
                if(!opResulReserva.Success)
                {
                    result.Message = opResulReserva.Message;
                    return result;
                }
                var opResulPago = await _pagoServices.ObtenerPagosAsync();
                if(!opResulPago.Success)
                {
                    result.Message = opResulPago.Message;
                    return result;
                }

                var dashboardDto = new DashBoardDto
                {
                    ResumenPago = opResultResumen.Data,
                    Reserva = opResulReserva.Data,
                    Pago = opResulPago.Data
                };

                result.Success = true;
                result.Data = dashboardDto;
                result.Message = "Datos del dashboard obtenidos correctamente.";
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return result;
        }
    }
}
