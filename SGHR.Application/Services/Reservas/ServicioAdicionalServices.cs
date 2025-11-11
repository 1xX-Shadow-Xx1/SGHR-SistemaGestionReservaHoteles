
using Microsoft.Extensions.Logging;
using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Habitaciones.Piso;
using SGHR.Application.Dtos.Configuration.Reservas.ServicioAdicional;
using SGHR.Application.Interfaces.Reservas;
using SGHR.Domain.Entities.Configuration.Reservas;
using SGHR.Domain.Enum.Reservas;
using SGHR.Persistence.Interfaces.Reservas;

namespace SGHR.Application.Services.Reservas
{
    public class ServicioAdicionalServices : IServicioAdicionalServices
    {
        private readonly ILogger<ServicioAdicionalServices> _logger;
        private readonly IServicioAdicionalRepository _servicioAdicionalRepository;

        public ServicioAdicionalServices(ILogger<ServicioAdicionalServices> logger,
                                         IServicioAdicionalRepository servicioAdicionalRepository)
        {
            _logger = logger;
            _servicioAdicionalRepository = servicioAdicionalRepository;
        }

        public async Task<ServiceResult> CreateAsync(CreateServicioAdicionalDto CreateDto)
        {
            ServiceResult result = new ServiceResult();
            if (CreateDto == null)
            {
                result.Message = "La tarifa no puede ser nula.";
                return result;
            }
            try
            {
                var ListServicioAdicional = await _servicioAdicionalRepository.GetAllAsync();
                if (!ListServicioAdicional.Success)
                {
                    result.Message = ListServicioAdicional.Message;
                    return result;
                }

                var ExistServicioAdicional = ListServicioAdicional.Data.FirstOrDefault(s => s.Nombre == CreateDto.Nombre);
                if (ExistServicioAdicional != null)
                {
                    result.Message = "Ya existe un servicio adicional con ese nombre.";
                    return result;
                }

                ServicioAdicional servicio = new ServicioAdicional()
                {
                    Nombre = CreateDto.Nombre,
                    Descripcion = CreateDto.Descripcion,
                    Precio = CreateDto.Precio
                };

                var opResult = await _servicioAdicionalRepository.SaveAsync(servicio);
                if (!opResult.Success)
                {
                    result.Message = opResult.Message;
                    return result;
                }

                ServicioAdicionalDto servicioDto = new ServicioAdicionalDto()
                {
                    Id = opResult.Data.Id,
                    Nombre = opResult.Data.Nombre,
                    Descripcion = opResult.Data.Descripcion,
                    Precio = opResult.Data.Precio,
                    Estado = opResult.Data.Estado
                };

                result.Success = true;
                result.Data = servicioDto;
                result.Message = $"Se a registrado el servicio adicional correctamente.";

            }
            catch (Exception ex)
            {
                result.Message = $"Error al registrar el servicio adicional: {ex.Message}";
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
                var existServicioAdicional = await _servicioAdicionalRepository.GetByIdAsync(id);
                if (!existServicioAdicional.Success)
                {
                    result.Message = $"No existe un servicio con ese id.";
                    return result;
                }

                var OpResult = await _servicioAdicionalRepository.DeleteAsync(existServicioAdicional.Data);
                if (!OpResult.Success)
                {
                    result.Message = OpResult.Message;
                    return result;
                }
                result.Success = true;
                result.Message = $"Servicio adicional eliminado correctamente.";


            }
            catch (Exception ex)
            {
                result.Message = $"Error al eliminar el servicio: {ex.Message}";
            }
            return result;
        }

        public async Task<ServiceResult> GetAllAsync()
        {
            ServiceResult result = new ServiceResult();
            try
            {
                var ListaServicios = await _servicioAdicionalRepository.GetAllAsync();
                if (!ListaServicios.Success)
                {
                    result.Message = ListaServicios.Message;
                    return result;
                }

                var servicios = ListaServicios.Data.ToList().Select(u => new ServicioAdicionalDto()
                {
                    Id = u.Id,
                    Nombre = u.Nombre,
                    Descripcion = u.Descripcion,
                    Precio = u.Precio,
                    Estado = u.Estado
                }).ToList();

                result.Success = true;
                result.Data = servicios;
                result.Message = $"Se obtuvieron los servicios correctamnete.";
            }
            catch (Exception ex)
            {
                result.Message = $"Error al obtener los servicios: {ex.Message}";
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
                var opResult = await _servicioAdicionalRepository.GetByIdAsync(id);
                if (!opResult.Success)
                {
                    result.Message = opResult.Message;
                    return result;
                }

                ServicioAdicionalDto piso = new ServicioAdicionalDto()
                {
                    Id = opResult.Data.Id,
                    Nombre = opResult.Data.Nombre,
                    Descripcion = opResult.Data.Descripcion,
                    Precio = opResult.Data.Precio,
                    Estado = opResult.Data.Estado
                };

                result.Success = true;
                result.Data = piso;
                result.Message = $"Se obtuvo el servicio con id {id} correctamnete.";

            }
            catch (Exception ex)
            {
                result.Message = $"Error al obtener el servicio por id.";
            }
            return result;
        }

        public async Task<ServiceResult> UpdateAsync(UpdateServicioAdicionalDto UpdateDto)
        {
            ServiceResult result = new ServiceResult();
            if (UpdateDto == null)
            {
                result.Message = "El servicio adicional no puede ser nulo.";
                return result;
            }
            if (UpdateDto.Id <= 0)
            {
                result.Message = $"El id ingresado no es valido.";
                return result;
            }
            try
            {
                var ExistServicioAdicional = await _servicioAdicionalRepository.GetByIdAsync(UpdateDto.Id);
                if (!ExistServicioAdicional.Success)
                {
                    result.Message = ExistServicioAdicional.Message;
                    return result;
                }

                var existName = await _servicioAdicionalRepository.GetByNombreAsync(UpdateDto.Nombre);
                if (existName.Success && existName.Data.Id != UpdateDto.Id)
                {
                    result.Message = ("Ya existe un Servicio adicional con ese numero.");
                    return result;
                }


                ExistServicioAdicional.Data.Nombre = UpdateDto.Nombre;
                ExistServicioAdicional.Data.Descripcion = UpdateDto.Descripcion;
                ExistServicioAdicional.Data.Precio = UpdateDto.Precio;
                ExistServicioAdicional.Data.Estado = UpdateDto.Estado;
                

                var OpResult = await _servicioAdicionalRepository.UpdateAsync(ExistServicioAdicional.Data);
                if (!OpResult.Success)
                {
                    result.Message = OpResult.Message;
                    return result;
                }

                var servicioDto = new ServicioAdicionalDto()
                {
                    Id = OpResult.Data.Id,
                    Nombre = OpResult.Data.Nombre,
                    Descripcion = OpResult.Data.Descripcion,
                    Precio = OpResult.Data.Precio,
                    Estado = OpResult.Data.Estado
                };

                result.Success = true;
                result.Data = servicioDto;
                result.Message = "Servicio actualizado correctamente.";

            }
            catch (Exception ex)
            {
                result.Message = $"Error actualizando el Servicio: {ex.Message}";
            }
            return result;
        }
    }
}
