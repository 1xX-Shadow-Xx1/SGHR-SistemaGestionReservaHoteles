using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Habitaciones.Habitacion;

namespace SGHR.Application.Interfaces.Habitaciones
{
    public interface IHabitacionServices : IBaseServices<CreateHabitacionDto, UpdateHabitacionDto>
    {
        Task<ServiceResult> GetDisponiblesAsync();
    }
}
