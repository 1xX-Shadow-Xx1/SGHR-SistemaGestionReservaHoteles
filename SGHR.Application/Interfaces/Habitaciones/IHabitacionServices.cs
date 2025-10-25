using SGHR.Application.Base;

namespace SGHR.Application.Interfaces.Habitaciones
{
    public interface IHabitacionServices
    {
        Task<ServiceResult> GetDisponiblesAsync();
    }
}
