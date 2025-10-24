using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Reservas.Reserva;

namespace SGHR.Application.Interfaces.Reservas
{
    public interface IReservaService : IBaseServices<CreateReservaDto, UpdateReservaDto>
    {
    }
}
