
using Microsoft.Extensions.Logging;
using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Habitaciones.Habitacion;
using SGHR.Application.Interfaces.Habitaciones;
using SGHR.Domain.Repository;

namespace SGHR.Application.Services.Habitaciones
{
    public class HabitacionServices : IHabitacionServices
    {
        public readonly ILogger<HabitacionServices> _logger;
        public readonly IHabitacionRepository _habitacionRepository;

        public HabitacionServices(ILogger<HabitacionServices> logger, 
                                  IHabitacionRepository habitacionRepository)
        {
            _logger = logger;
            _habitacionRepository = habitacionRepository;
        }

        public async Task<ServiceResult> CreateAsync(CreateHabitacionDto CreateDto)
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

        public async Task<ServiceResult> GetDisponiblesAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResult> UpdateAsync(UpdateHabitacionDto UpdateDto)
        {
            throw new NotImplementedException();
        }
    }
}
