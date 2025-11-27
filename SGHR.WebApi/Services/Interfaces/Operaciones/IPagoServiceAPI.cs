using SGHR.Web.Models;
using SGHR.Web.Models.Operaciones.Pago;

namespace SGHR.Web.Services.Interfaces.Operaciones
{
    public interface IPagoServiceAPI 
    {
        ApiResult<PagoModel> getPagoById(int id);
        List<PagoModel> getPagoList();
        Task<ApiResult<PagoModel>> RealizarPago(RealizarPagoModel realizarPago);
        Task<ApiResult<PagoModel>> AnularPago(int idPago);
        Task<ApiResult<ResumenPagoModel>> GetResumenDePagos();
    }
}
