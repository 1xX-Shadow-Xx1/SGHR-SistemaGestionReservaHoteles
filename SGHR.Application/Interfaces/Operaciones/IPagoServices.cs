
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
        Task<ServiceResult> GetPagoByCliente(int idcliente);
        Task<ServiceResult> GetPagoByIdAsync(int idPago);
        Task<ServiceResult> GetAllNoRechazados();

    }
}
