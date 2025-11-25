using SGHR.Web.Models;
using SGHR.Web.Models.Reservas.Reserva;
using SGHR.Web.Models.Reservas.ServicioAdicional;
using SGHR.Web.Services.Interfaces.Base;

namespace SGHR.Web.Services.Interfaces.Reservas
{
    public interface IReservaServiceAPI : IBaseServicesAPI<ReservaModel, CreateReservaModel, UpdateReservaModel>
    {
        Task<ServicesResultModel> RemoveServicio_ReservaPut(string nameServicio, int idreserva);
        Task<ServicesResultModel> AddServicio_ReservaPut(string nameServicio, int idreserva);
        Task<ServicesResultModel> GetServicesbyReserva(int idreserva);
        List<ServicioAdicionalModel> GetServiciosAdicionalesdisponibles();
    }
}
