using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Reservas.Tarifa;


namespace SGHR.Application.Interfaces.Reservas
{
    public interface ITarifaService : IBaseServices<CreateTarifaDto, UpdateTarifaDto>
    {
    }
}
