using Microsoft.Extensions.Logging;
using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Habitaciones.Amenity;
using SGHR.Application.Interfaces.Habitaciones;
using SGHR.Domain.Entities.Configuration.Habitaciones;
using SGHR.Persistence.Interfaces.Habitaciones;

namespace SGHR.Application.Services.Categorias
{
    public class AmenityService : IAmenityService
    {
        private readonly ILogger<AmenityService> _logger;
        public readonly IAmenityRepository _amenityRepository;
        
        public AmenityService(IAmenityRepository amenityRepository,
                              ILogger<AmenityService> logger)
        {
            _amenityRepository = amenityRepository;
            _logger = logger;
        }

        public async Task<ServiceResult> GetAllAsync()
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando obtención de todos los amenities.");

            try
            {
                var opResult = await _amenityRepository.GetAllAsync();
                if (opResult.Success)
                {
                    _logger.LogInformation("Se obtuvieron {count} Amenities correctamente.", opResult.Data.Count);
                    result.Success = true;
                    result.Data = opResult.Data;
                    result.Message = "Amenities obtenidos correctamente.";
                }
                else
                {
                    result.Success = false;
                    result.Message = "Error al obtener los amenities.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los amenities.");
                result.Success = false;
                result.Message = "Error al obtener los amenities.";
            }
            return result;
        }
        public async Task<ServiceResult> GetByIdAsync(int id)
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando obtención de amenity por ID: {Id}", id);

            try
            {
                var opResult = await _amenityRepository.GetByIdAsync(id);
                if (opResult.Success)
                {
                    _logger.LogInformation("Se obtuvo una amenity con Id {id} correctamente.", id);
                    result.Success = true;
                    result.Data = opResult.Data;
                    result.Message = "Amenity obtenido correctamente.";
                }
                else
                {
                    result.Success = false;
                    result.Message = opResult.Message;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el amenity.");
                result.Success = false;
                result.Message = "Error al obtener el amenity.";
            }
            return result;
        }
        public async Task<ServiceResult> UpdateAsync(UpdateAmenityDto updateAmenityDto, int? idsesion = null)
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando actualización de amenity: {Dto}", updateAmenityDto);

            try
            {

                var existingAmenityResult = await _amenityRepository.GetByIdAsync(updateAmenityDto.Id);
                if (!existingAmenityResult.Success || existingAmenityResult.Data == null)
                {
                    result.Success = false;
                    result.Message = existingAmenityResult.Message;
                    return result;
                }

                var existingAmenity = existingAmenityResult.Data;
                existingAmenity.Nombre = updateAmenityDto.Nombre;
                existingAmenity.Descripcion = updateAmenityDto.Descripcion;
                var opResult = await _amenityRepository.UpdateAsync(existingAmenity, idsesion);
                if (opResult.Success)
                {
                    _logger.LogInformation("Se a actualizado un amenity con Id {id} correctamente.", updateAmenityDto.Id);
                    result.Success = true;
                    result.Data = opResult.Data;
                    result.Message = "Amenity actualizado correctamente.";
                }
                else
                {
                    result.Success = false;
                    result.Message = "Error al actualizar el amenity.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el amenity.");
                result.Success = false;
                result.Message = "Error al actualizar el amenity.";
            }
            return result;
        }
        public async Task<ServiceResult> DeleteAsync(int id, int? idsesion = null)
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando eliminación de amenity: {id}", id);

            try
            {
                if (id < 0)
                {
                    result.Success = false;
                    result.Message = "El amenity debe tener un ID válido.";
                    return result;
                }

                var AmenityExist = await _amenityRepository.GetByIdAsync(id);
                if (!AmenityExist.Success)
                {
                    result.Success = false;
                    result.Message = AmenityExist.Message;
                    return result;
                }
                var opResult = await _amenityRepository.DeleteAsync(AmenityExist.Data, idsesion);
                if (opResult.Success)
                {
                    _logger.LogInformation("Se a eliminado un Amenity con Id {id} correctamente.", id);
                    result.Success = true;
                    result.Data = opResult.Data;
                    result.Message = "Amenity eliminado correctamente.";
                }
                else
                {
                    result.Success = false;
                    result.Message = "Error al eliminar el amenity.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el amenity.");
                result.Success = false;
                result.Message = "Error al eliminar el amenity.";
            }
            return result;
        }
        public async Task<ServiceResult> CreateAsync(CreateAmenityDto createAmenityDto, int? idsesion = null)
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando creación de amenity: {Dto}", createAmenityDto);

            try
            {
                if (createAmenityDto == null)
                {
                    result.Success = false;
                    result.Message = "Error: el amenity no puede ser nulo.";
                    return result;
                }

                Amenity amenity = new Amenity
                {
                    Nombre = createAmenityDto.Nombre,
                    Descripcion = createAmenityDto.Descripcion
                };

                var opResult = await _amenityRepository.SaveAsync(amenity, idsesion);
                if (opResult.Success)
                {
                    _logger.LogInformation("Se a creado un nuevo amenity con el nombre {name} correctamente.", amenity.Nombre);
                    result.Success = true;
                    result.Data = opResult.Data;
                    result.Message = "Amenity creado correctamente.";
                }
                else
                {
                    result.Success = false;
                    result.Message = opResult.Message;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el amenity.");
                result.Success = false;
                result.Message = "Error al crear el amenity.";
            }
            return result;
        }
    }
}
