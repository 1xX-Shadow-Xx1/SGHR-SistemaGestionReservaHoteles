

using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Operaciones.Pago;

namespace SGHR.Application.Interfaces.Operaciones
{
    public interface IPagoService : IBaseServices<CreatePagoDto, UpdatePagoDto, DeletePagoDto>
    {
    }
}
