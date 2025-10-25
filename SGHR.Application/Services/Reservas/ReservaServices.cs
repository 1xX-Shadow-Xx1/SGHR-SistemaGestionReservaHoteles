
using Microsoft.Extensions.Logging;
using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Reservas.Reserva;
using SGHR.Application.Interfaces.Reservas;
using SGHR.Domain.Repository;

namespace SGHR.Application.Services.Reservas
{
    public class ReservaServices : IReservaServices
    {
        public readonly ILogger<ReservaServices> _logger;
        public readonly IReservaRepository _repository;

        public ReservaServices(ILogger<ReservaServices> logger, 
                               IReservaRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task<ServiceResult> CreateAsync(CreateReservaDto CreateDto)
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

        public async Task<ServiceResult> UpdateAsync(UpdateReservaDto UpdateDto)
        {
            throw new NotImplementedException();
        }
    }
}
