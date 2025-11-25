using SGHR.Web.Data.Interfaces.Base;
using SGHR.Web.Models;
using SGHR.Web.Models.Habitaciones.Habitacion;

namespace SGHR.Web.Data.Interfaces.Habitaciones
{
    public interface IHabitacionRepositoryMemory : IBaseRepositoryMemory<HabitacionModel>
    {
        ServicesResultModel GetHabitacionByNumero(string numeroHabitacion);
    }
}
