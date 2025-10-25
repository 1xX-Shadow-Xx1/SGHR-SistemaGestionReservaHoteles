

using Microsoft.Extensions.Logging;
using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Habitaciones.Categoria;
using SGHR.Application.Interfaces.Habitaciones;
using SGHR.Persistence.Interfaces.Habitaciones;

namespace SGHR.Application.Services.Habitaciones
{
    public class CategoriaServices : ICategoriaServices
    {
        public readonly ILogger<CategoriaServices> _logger;
        public readonly ICategoriaRepository _categoriaRepository;

        public CategoriaServices(ILogger<CategoriaServices> logger, 
                                 ICategoriaRepository categoriaRepository)
        {
            _logger = logger;
            _categoriaRepository = categoriaRepository;
        }

        public async Task<ServiceResult> CreateAsync(CreateCategoriaDto CreateDto)
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

        public async Task<ServiceResult> UpdateAsync(UpdateCategoriaDto UpdateDto)
        {
            throw new NotImplementedException();
        }
    }
}
