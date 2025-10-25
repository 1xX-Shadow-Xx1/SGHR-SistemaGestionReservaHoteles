

using Microsoft.Extensions.Logging;
using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Habitaciones.Piso;
using SGHR.Application.Interfaces.Habitaciones;
using SGHR.Persistence.Interfaces.Habitaciones;

namespace SGHR.Application.Services.Habitaciones
{
    public class PisoServices : IPisoServices
    {
        public readonly ILogger<PisoServices> _logger;
        public readonly IPisoRepository _pisoRepository;

        public PisoServices(ILogger<PisoServices> logger, 
                            IPisoRepository pisoRepository)
        {
            _logger = logger;
            _pisoRepository = pisoRepository;
        }

        public async Task<ServiceResult> CreateAsync(CreatePisoDto CreateDto)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResult> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResult> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResult> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResult> UpdateAsync(UpdatePisoDto UpdateDto)
        {
            throw new NotImplementedException();
        }
    }
}
