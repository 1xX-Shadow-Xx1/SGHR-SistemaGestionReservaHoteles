using SGHR.Web.Models;
using SGHR.Web.Models.Reservas.Reserva;
using SGHR.Web.Models.Reservas.ServicioAdicional;
using SGHR.Web.Services.Interfaces.Base;

namespace SGHR.Web.Services.Interfaces.Reservas
{
    public interface IReservaServiceAPI : IBaseServicesAPI<ReservaModel, CreateReservaModel, UpdateReservaModel>
    {
        Task<ApiResult<ReservaModel>> RemoveServicio_ReservaPut(string nameServicio, int idreserva);
        Task<ApiResult<ReservaModel>> AddServicio_ReservaPut(string nameServicio, int idreserva);
        Task<ApiResult<List<ServicioAdicionalModel>>> GetServicesbyReserva(int idreserva);
        List<ServicioAdicionalModel> GetServiciosAdicionalesdisponibles();
    }
}
