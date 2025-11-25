using SGHR.Web.Models;
using SGHR.Web.Models.Operaciones.Pago;

namespace SGHR.Web.Services.Interfaces.Operaciones
{
    public interface IPagoServiceAPI 
    {
        ServicesResultModel getPagoById(int id);
        List<PagoModel> getPagoList();
        Task<ServicesResultModel> RealizarPago(RealizarPagoModel realizarPago);
        Task<ServicesResultModel> AnularPago(int idPago);
        Task<ServicesResultModel> GetResumenDePagos();
    }
}
