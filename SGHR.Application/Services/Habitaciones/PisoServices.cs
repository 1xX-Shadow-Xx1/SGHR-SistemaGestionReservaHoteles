using Microsoft.Extensions.Logging;
using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Habitaciones.Piso;
using SGHR.Application.Dtos.Configuration.Users.Usuario;
using SGHR.Application.Interfaces.Habitaciones;
using SGHR.Domain.Entities.Configuration.Habitaciones;
using SGHR.Domain.Enum.Habitaciones;
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
            ServiceResult result = new ServiceResult();
            if (CreateDto == null)
            {
                result.Message = "El Piso no puede ser nulo.";
                return result;
            }
            try
            {
                var existUser = await _pisoRepository.GetByNumeroPisoAsync(CreateDto.NumeroPiso);
                if (existUser.Success)
                {
                    result.Message = ("Ya existe un Piso con ese numero.");
                    return result;
                }


                Piso Piso = new Piso()
                {
                    NumeroPiso = CreateDto.NumeroPiso,
                    Descripcion = CreateDto.Descripcion
                };

                var OpResult = await _pisoRepository.SaveAsync(Piso);
                if (!OpResult.Success)
                {
                    result.Message = OpResult.Message;
                    return result;
                }

                var getusuario = new PisoDto()
                {
                    Id = OpResult.Data.Id,
                    NumeroPiso = OpResult.Data.NumeroPiso,
                    Descripcion = OpResult.Data.Descripcion,
                    Estado = OpResult.Data.Estado.ToString()
                };

                result.Success = true;
                result.Data = getusuario;
                result.Message = "Piso registrado correctamente.";


            }
            catch (Exception ex)
            {
                result.Message = $"Error creado al Piso : {ex.Message}";
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
                var existPiso = await _pisoRepository.GetByIdAsync(id);
                if (!existPiso.Success)
                {
                    result.Message = $"No existe un piso con ese id.";
                    return result;
                }

                var OpResult = await _pisoRepository.DeleteAsync(existPiso.Data);
                if (!OpResult.Success)
                {
                    result.Message = OpResult.Message;
                    return result;
                }
                result.Success = true;
                result.Message = $"Piso numero {existPiso.Data.NumeroPiso} eliminado correctamente.";


            }
            catch (Exception ex)
            {
                result.Message = $"Error al eliminar el piso: {ex.Message}";
            }
            return result;
        }
        public async Task<ServiceResult> GetAllAsync()
        {
            ServiceResult result = new ServiceResult();
            try
            {
                var ListaPisos = await _pisoRepository.GetAllAsync();
                if (!ListaPisos.Success)
                {
                    result.Message = ListaPisos.Message;
                    return result;
                }

                var Pisos = ListaPisos.Data.ToList().Select(u => new PisoDto()
                {
                    Id = u.Id,
                    NumeroPiso = u.NumeroPiso,
                    Descripcion = u.Descripcion,
                    Estado = u.Estado.ToString()
                }).ToList();

                result.Success = true;
                result.Data = Pisos;
                result.Message = $"Se obtuvieron los pisos correctamnete.";
            }
            catch (Exception ex)
            {
                result.Message = $"Error al obtener los pisos: {ex.Message}";
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
                var opResult = await _pisoRepository.GetByIdAsync(id);
                if (!opResult.Success)
                {
                    result.Message = opResult.Message;
                    return result;
                }

                PisoDto piso = new PisoDto()
                {
                    Id = opResult.Data.Id,
                    NumeroPiso = opResult.Data.NumeroPiso,
                    Descripcion = opResult.Data.Descripcion,
                    Estado = opResult.Data.Estado.ToString()
                };

                result.Success = true;
                result.Data = piso;
                result.Message = $"Se obtuvo el piso con id {id} correctamnete.";

            }
            catch (Exception ex)
            {
                result.Message = $"Error al obtener al piso por id.";
            }
            return result;
        }
        public async Task<ServiceResult> UpdateAsync(UpdatePisoDto UpdateDto)
        {
            ServiceResult result = new ServiceResult();
            if (UpdateDto == null)
            {
                result.Message = "El Piso no puede ser nulo.";
                return result;
            }
            if (UpdateDto.Id <= 0)
            {
                result.Message = $"El id ingresado no es valido.";
                return result;
            }
            try
            {
                var Piso = await _pisoRepository.GetByIdAsync(UpdateDto.Id);
                if (!Piso.Success)
                {
                    result.Message = Piso.Message;
                    return result;
                }

                var existNum = await _pisoRepository.GetByNumeroPisoAsync(UpdateDto.NumeroPiso, UpdateDto.Id);
                if (existNum.Success)
                {
                    result.Message = ("Ya existe un Piso con ese numero.");
                    return result;
                }


                Piso.Data.NumeroPiso = UpdateDto.NumeroPiso;
                Piso.Data.Descripcion = UpdateDto.Descripcion;

                if (Enum.TryParse<EstadoPiso>(UpdateDto.Estado, out var estado))
                {
                    Piso.Data.Estado = estado;
                }

                var OpResult = await _pisoRepository.UpdateAsync(Piso.Data);
                if (!OpResult.Success)
                {
                    result.Message = OpResult.Message;
                    return result;
                }

                var PisoDto = new PisoDto()
                {
                    Id = OpResult.Data.Id,
                    NumeroPiso = OpResult.Data.NumeroPiso,
                    Descripcion = OpResult.Data.Descripcion,
                    Estado = OpResult.Data.Estado.ToString()
                };

                result.Success = true;
                result.Data = PisoDto;
                result.Message = "Piso actualizado correctamente.";

            }
            catch (Exception ex)
            {
                result.Message = $"Error actualizando el Piso: {ex.Message}";
            }
            return result;
        }
    }
}
