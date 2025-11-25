using SGHR.Web.Models;
using SGHR.Web.Models.Habitaciones.Habitacion;
using SGHR.Web.Services.Interfaces.Base;

namespace SGHR.Web.Services.Interfaces.Habitaciones
{
    public interface IHabitacionServiceAPI : IBaseServicesAPI<HabitacionModel, CreateHabitacionModel,  UpdateHabitacionModel>
    {
        Task<ServicesResultModel> GetHabitacionesDisponiblesRangeDate(DateTime startDate, DateTime endDate);
        Task<ServicesResultModel> GetHabitacionesDisponibles();
        ServicesResultModel GetHabitacionByNumero(string numeroHabitacion);
    }
}
