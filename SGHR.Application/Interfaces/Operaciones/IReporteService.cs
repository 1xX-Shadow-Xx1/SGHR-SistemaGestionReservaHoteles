using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Operaciones.Reporte;

namespace SGHR.Application.Interfaces.Operaciones
{
    public interface IReporteService : IBaseServices<CreateReporteDto, UpdateReporteDto, DeleteReporteDto>
    {
    }
}
