
using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Operaciones.Pago;

namespace SGHR.Application.Interfaces.Operaciones
{
    public interface IPagoServices 
    {
        Task<ServiceResult> RealizarPagoAsync(RealizarPagoDto dto);
        Task<ServiceResult> ObtenerResumenPagosAsync();
        Task<ServiceResult> AnularPagoAsync(int idPago);
        Task<ServiceResult> ObtenerPagosAsync();

    }
}
