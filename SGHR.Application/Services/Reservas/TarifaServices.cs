
using Microsoft.Extensions.Logging;
using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Reservas.Tarifa;
using SGHR.Application.Interfaces.Reservas;
using SGHR.Persistence.Interfaces.Reservas;

namespace SGHR.Application.Services.Reservas
{
    public class TarifaServices : ITarifaServices
    {
        public readonly ILogger<TarifaServices> _logger;
        public readonly ITarifaRepository _tarifaRepository;

        public TarifaServices(ILogger<TarifaServices> logger,
                              ITarifaRepository tarifaRepository)
        {
            _logger = logger;
            _tarifaRepository = tarifaRepository;
        }

        public async Task<ServiceResult> CreateAsync(CreateTarifaDto CreateDto)
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

        public async Task<ServiceResult> UpdateAsync(UpdateTarifaDto UpdateDto)
        {
            throw new NotImplementedException();
        }
    }
}
