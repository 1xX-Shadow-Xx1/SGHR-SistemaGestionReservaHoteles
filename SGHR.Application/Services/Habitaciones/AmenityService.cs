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

        public async Task<ServiceResult> GetAll()
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando obtención de todos los amenities.");

            try
            {
                var opResult = await _amenityRepository.GetAll();
                if (opResult.Success)
                {
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

        public async Task<ServiceResult> GetById(int id)
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando obtención de amenity por ID: {Id}", id);

            try
            {
                var opResult = await _amenityRepository.GetById(id);
                if (opResult.Success)
                {
                    result.Success = true;
                    result.Data = opResult.Data;
                    result.Message = "Amenity obtenido correctamente.";
                }
                else
                {
                    result.Success = false;
                    result.Message = "Error al obtener el amenity.";
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

        public async Task<ServiceResult> Update(UpdateAmenityDto updateAmenityDto)
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando actualización de amenity: {Dto}", updateAmenityDto);

            try
            {
                if (updateAmenityDto == null || updateAmenityDto.Id <= 0)
                {
                    result.Success = false;
                    result.Message = "Error: el amenity no puede ser nulo y debe tener un ID válido.";
                    return result;
                }

                var existingAmenityResult = await _amenityRepository.GetById(updateAmenityDto.Id);
                if (!existingAmenityResult.Success || existingAmenityResult.Data == null)
                {
                    result.Success = false;
                    result.Message = "Error: el amenity a actualizar no existe.";
                    return result;
                }

                var existingAmenity = existingAmenityResult.Data;
                existingAmenity.Nombre = updateAmenityDto.Nombre;
                existingAmenity.Descripcion = updateAmenityDto.Descripcion;
                var opResult = await _amenityRepository.Update(existingAmenity);
                if (opResult.Success)
                {
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

        public async Task<ServiceResult> Remove(DeleteAmenityDto deleteAmenity)
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando eliminación de amenity: {Dto}", deleteAmenity);

            try
            {
                if (deleteAmenity == null || deleteAmenity.Id <= 0)
                {
                    result.Success = false;
                    result.Message = "Error: el amenity no puede ser nulo y debe tener un ID válido.";
                    return result;
                }

                var existingAmenityResult = await _amenityRepository.GetById(deleteAmenity.Id);
                if (!existingAmenityResult.Success || existingAmenityResult.Data == null)
                {
                    result.Success = false;
                    result.Message = "Error: el amenity a eliminar no existe.";
                    return result;
                }
                var opResult = await _amenityRepository.Delete(existingAmenityResult.Data);
                if (opResult.Success)
                {
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

        public async Task<ServiceResult> Save(CreateAmenityDto createAmenityDto)
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

                var opResult = await _amenityRepository.Save(amenity);
                if (opResult.Success)
                {
                    result.Success = true;
                    result.Data = opResult.Data;
                    result.Message = "Amenity creado correctamente.";
                }
                else
                {
                    result.Success = false;
                    result.Message = "Error al crear el amenity.";
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
