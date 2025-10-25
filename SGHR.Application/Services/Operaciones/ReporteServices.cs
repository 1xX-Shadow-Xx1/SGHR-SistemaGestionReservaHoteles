

using Microsoft.Extensions.Logging;
using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Operaciones.Reporte;
using SGHR.Application.Interfaces.Operaciones;
using SGHR.Persistence.Interfaces.Reportes;

namespace SGHR.Application.Services.Operaciones
{
    public class ReporteServices : IReporteServices
    {
        public readonly ILogger<ReporteServices> _logger;
        public readonly IReporteRepository _reporteRepository;

        public ReporteServices(ILogger<ReporteServices> logger, 
                               IReporteRepository reporteRepository)
        {
            _logger = logger;
            _reporteRepository = reporteRepository;
        }

        public async Task<ServiceResult> CreateAsync(CreateReporteDto CreateDto)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResult> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResult> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResult> UpdateAsync(UpdateReporteDto UpdateDto)
        {
            throw new NotImplementedException();
        }
    }
}
