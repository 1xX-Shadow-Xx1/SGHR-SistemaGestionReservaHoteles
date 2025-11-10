

using Microsoft.Extensions.Logging;
using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Habitaciones.Amenity;
using SGHR.Application.Interfaces.Habitaciones;
using SGHR.Domain.Entities.Configuration.Habitaciones;
using SGHR.Persistence.Interfaces.Habitaciones;

namespace SGHR.Application.Services.Habitaciones
{
    public class AmenityServices : IAmenityServices
    {
        private readonly ILogger<AmenityServices> _logger;
        private readonly IAmenityRepository _amenityRepository;

        public AmenityServices(ILogger<AmenityServices> logger,
                               IAmenityRepository amenityRepository)
        {
            _logger = logger;
            _amenityRepository = amenityRepository;
        }

        public async Task<ServiceResult> CreateAsync(CreateAmenityDto CreateDto)
        {
            ServiceResult result = new ServiceResult();
            if (CreateDto == null)
            {
                result.Message = "El Amenety no puede ser nulo.";
                return result;
            }
            try
            {
                var existAmenityName = await _amenityRepository.GetByNombreAsync(CreateDto.Nombre);
                if (existAmenityName.Success)
                {
                    result.Message = ("Ya existe un Amenity con ese nombre.");
                    return result;
                }
                var existAmenityDesc = await _amenityRepository.GetAllAsync();
                if (existAmenityDesc.Success && existAmenityDesc.Data.FirstOrDefault(a => a.Descripcion == CreateDto.Descripcion) != null)
                {
                    result.Message = ("Ya existe un Amenity con esa descripcion.");
                    return result;
                }

                Amenity amenity = new Amenity()
                {
                    Nombre = CreateDto.Nombre,
                    Descripcion = CreateDto.Descripcion,
                    Precio = CreateDto.Precio,
                    PorCapacidad = CreateDto.PorCapacidad
                };

                var OpResult = await _amenityRepository.SaveAsync(amenity);
                if (!OpResult.Success)
                {
                    result.Message = OpResult.Message;
                    return result;
                }

                var amenityDto = new AmenityDto()
                {
                    Id = OpResult.Data.Id,
                    Nombre = OpResult.Data.Nombre,
                    Descripcion = OpResult.Data.Descripcion,
                    Precio = OpResult.Data.Precio,
                    PorCapacidad = OpResult.Data.PorCapacidad
                };

                result.Success = true;
                result.Data = amenityDto;
                result.Message = "Amenity registrado correctamente.";


            }
            catch (Exception ex)
            {
                result.Message = $"Error creado el Amenity : {ex.Message}";
            }
            return result;
        }

        public async Task<ServiceResult> DeleteAsync(int id)
        {
            ServiceResult result = new ServiceResult();
            if (id <= 0)
            {
                result.Message = $"El id ingresado no es valido.";
                return result;
            }
            try
            {
                var existAmenity = await _amenityRepository.GetByIdAsync(id);
                if (!existAmenity.Success)
                {
                    result.Message = $"No existe un Amenety con ese id.";
                    return result;
                }

                var OpResult = await _amenityRepository.DeleteAsync(existAmenity.Data);
                if (!OpResult.Success)
                {
                    result.Message = OpResult.Message;
                    return result;
                }
                result.Success = true;
                result.Message = $"Amenety {existAmenity.Data.Nombre} eliminado correctamente.";


            }
            catch (Exception ex)
            {
                result.Message = $"Error al eliminar el Amenety: {ex.Message}";
            }
            return result;
        }

        public async Task<ServiceResult> GetAllAsync()
        {
            ServiceResult result = new ServiceResult();
            try
            {
                var ListaAmenity = await _amenityRepository.GetAllAsync();
                if (!ListaAmenity.Success)
                {
                    result.Message = ListaAmenity.Message;
                    return result;
                }

                var amenityDto = ListaAmenity.Data.ToList().Select(u => new AmenityDto()
                {
                    Id = u.Id,
                    Nombre = u.Nombre,
                    Descripcion = u.Descripcion,
                    Precio = u.Precio,
                    PorCapacidad = u.PorCapacidad
                }).ToList();

                result.Success = true;
                result.Data = amenityDto;
                result.Message = $"Se obtuvieron los amenity correctamnete.";
            }
            catch (Exception ex)
            {
                result.Message = $"Error al obtener los amenity: {ex.Message}";
            }
            return result;
        }

        public async Task<ServiceResult> GetByIdAsync(int id)
        {
            ServiceResult result = new ServiceResult();
            if (id <= 0)
            {
                result.Message = $"El id ingresado no es valido.";
                return result;
            }
            try
            {
                var opResult = await _amenityRepository.GetByIdAsync(id);
                if (!opResult.Success)
                {
                    result.Message = opResult.Message;
                    return result;
                }

                AmenityDto piso = new AmenityDto()
                {
                    Id = opResult.Data.Id,
                    Nombre = opResult.Data.Nombre,
                    Descripcion = opResult.Data.Descripcion,
                    Precio = opResult.Data.Precio,
                    PorCapacidad = opResult.Data.PorCapacidad
                };

                result.Success = true;
                result.Data = piso;
                result.Message = $"Se obtuvo el amenity con id {id} correctamnete.";

            }
            catch (Exception ex)
            {
                result.Message = $"Error al obtener al amenity por id.";
            }
            return result;
        }

        public async Task<ServiceResult> UpdateAsync(UpdateAmenityDto UpdateDto)
        {
            ServiceResult result = new ServiceResult();
            if (UpdateDto == null)
            {
                result.Message = "El Amenety no puede ser nulo.";
                return result;
            }
            if (UpdateDto.Id <= 0)
            {
                result.Message = $"El id ingresado no es valido.";
                return result;
            }
            try
            {
                var amenity = await _amenityRepository.GetByIdAsync(UpdateDto.Id);
                if (!amenity.Success)
                {
                    result.Message = amenity.Message;
                    return result;
                }

                var existAmenityName = await _amenityRepository.GetByNombreAsync(UpdateDto.Nombre);
                if (existAmenityName.Success && existAmenityName.Data.Id != UpdateDto.Id)
                {
                    result.Message = ("Ya existe un Amenity con ese nombre.");
                    return result;
                }
                var existAmenityDesc = await _amenityRepository.GetAllAsync();
                if (existAmenityDesc.Success && existAmenityDesc.Data.FirstOrDefault(a => a.Descripcion == UpdateDto.Descripcion && a.Id != UpdateDto.Id) != null)
                {
                    result.Message = ("Ya existe un Amenity con esa descripcion.");
                    return result;
                }


                amenity.Data.Nombre = UpdateDto.Nombre;
                amenity.Data.Descripcion = UpdateDto.Descripcion;

                var OpResult = await _amenityRepository.UpdateAsync(amenity.Data);
                if (!OpResult.Success)
                {
                    result.Message = OpResult.Message;
                    return result;
                }

                var amenityDto = new AmenityDto()
                {
                    Id = OpResult.Data.Id,
                    Nombre = OpResult.Data.Nombre,
                    Descripcion = OpResult.Data.Descripcion,
                    Precio = OpResult.Data.Precio,
                    PorCapacidad = OpResult.Data.PorCapacidad
                };

                result.Success = true;
                result.Data = amenityDto;
                result.Message = "Amenety actualizado correctamente.";

            }
            catch (Exception ex)
            {
                result.Message = $"Error actualizando el Amenety: {ex.Message}";
            }
            return result;
        }
    }
}
