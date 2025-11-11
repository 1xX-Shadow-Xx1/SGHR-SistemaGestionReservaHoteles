using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Reservas.Reserva;

namespace SGHR.Application.Interfaces.Reservas
{
    public interface IReservaServices : IBaseServices<CreateReservaDto, UpdateReservaDto>
    {
        Task<ServiceResult> AddServicioAdicional(int id, string nombreServicio);
        Task<ServiceResult> RemoveServicioAdicional(int id, string nombreServicio);
        Task<ServiceResult> GetServiciosByReservaId(int id);
    }
}
