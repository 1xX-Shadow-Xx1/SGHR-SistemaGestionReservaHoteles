using SGHR.Domain.Base;
using SGHR.Domain.Entities.Configuration.Reportes;
using SGHR.Domain.Repository;

namespace SGHR.Persistence.Interfaces.Reportes
{
    public interface IReporteRepository : IBaseRepository<Reporte>
    {
        Task<OperationResult<List<Reporte>>> GetByTipoAsync(string tipoReporte);
        Task<OperationResult<List<Reporte>>> GetByFechaAsync(DateTime fechaInicio, DateTime fechaFin);
        Task<OperationResult<List<Reporte>>> GetByUsuarioAsync(int usuarioId);
    }
}
