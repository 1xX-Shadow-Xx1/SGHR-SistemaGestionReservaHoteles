using Microsoft.Extensions.Logging;
using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Users.Usuario;
using SGHR.Application.Interfaces.Usuarios;
using SGHR.Application.ValidatorServices.Usuarios;
using SGHR.Domain.Entities.Configuration.Usuers;
using SGHR.Domain.Enum.Usuario;
using SGHR.Domain.Repository;
using SGHR.Persistence.Interfaces.Sesiones;

namespace SGHR.Application.Services.Usuarios
{
    public class UsuarioServices : IUsuarioServices
    {
        private readonly ILogger<UsuarioServices> _logger;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ISesionRepository _sesionRepository;

        public UsuarioServices(ILogger<UsuarioServices> logger,
                               IUsuarioRepository usuarioRepository,
                               ISesionRepository sesionRepository)
        {
            _logger = logger;
            _usuarioRepository = usuarioRepository;
            _sesionRepository = sesionRepository;
        }

        public async Task<ServiceResult> CreateAsync(CreateUsuarioDto CreateDto)
        {
            ServiceResult result = new ServiceResult();
            try
            {
                var validate = new UsuarioValidatorServices(_usuarioRepository).ValidateSave(CreateDto, out string errorMessage);
                if (!validate)
                {
                    result.Message = errorMessage;
                    return result;
                }
                Usuario usuario = new Usuario()
                {
                    Nombre = CreateDto.Nombre,
                    Correo = CreateDto.Correo,
                    Contraseña = CreateDto.Contraseña,
                    Rol = CreateDto.Rol
                };               

                var OpResult = await _usuarioRepository.SaveAsync(usuario);
                if (!OpResult.Success)
                {
                    result.Message = OpResult.Message;
                    return result;
                }

                var getusuario = new UsuarioDto()
                {
                    Id = OpResult.Data.Id,
                    Nombre = OpResult.Data.Nombre,
                    Correo = OpResult.Data.Correo,
                    Rol = OpResult.Data.Rol
                };

                result.Success = true;
                result.Data = getusuario;
                result.Message = "Usuario registrado correctamente.";
            }
            catch (Exception ex)
            {
                result.Message = $"Error creado al usuario : {ex.Message}";
            }
            return result;
        }
        public async Task<ServiceResult> DeleteAsync(int id)
        {
            ServiceResult result = new ServiceResult();     
            try
            {
                var validate = new UsuarioValidatorServices(_usuarioRepository).ValidateDelete(id, out string errorMessage);
                if (!validate)
                {
                    result.Message = errorMessage;
                    return result;
                }
                var user = await _usuarioRepository.GetByIdAsync(id);
                if (!user.Success)
                {
                    result.Message = user.Message;
                    return result;
                }

                var OpResult = await _usuarioRepository.DeleteAsync(user.Data);
                if (!OpResult.Success)
                {
                    result.Message = OpResult.Message;
                    return result;
                }
                result.Success = true;
                result.Message = $"Usuario {user.Data.Nombre} eliminado correctamente.";


            }
            catch (Exception ex)
            {
                result.Message = $"Error al eliminar el usuario: {ex.Message}";
            }
            return result;
        }
        public async Task<ServiceResult> GetAllAsync()
        {
            ServiceResult result = new ServiceResult();
            try
            {
                var ListaUsuarios = await _usuarioRepository.GetAllAsync();
                if (!ListaUsuarios.Success)
                {
                    result.Message = ListaUsuarios.Message;
                    return result;
                }

                var usuarios = ListaUsuarios.Data.ToList().Select(u => new UsuarioDto()
                {
                    Id = u.Id,
                    Nombre = u.Nombre,
                    Correo = u.Correo,
                    Rol = u.Rol,
                    Estado = u.Estado
                }).ToList();

                result.Success = true;
                result.Data = usuarios;
                result.Message = $"Se obtuvieron los usuarios correctamnete.";
            }
            catch (Exception ex)
            {
                result.Message = $"Error al obtener los usuarios: {ex.Message}";
            }
            return result;
        }
        public async Task<ServiceResult> GetByIdAsync(int id)
        {
            ServiceResult result = new ServiceResult();
            try
            {
                var validate = new UsuarioValidatorServices(_usuarioRepository).ValidateGetById(id, out string errorMessage);
                if (!validate)
                {
                    result.Message = errorMessage;
                    return result;
                }
                var opResult = await _usuarioRepository.GetByIdAsync(id);
                if (!opResult.Success)
                {
                    result.Message = opResult.Message;
                    return result;
                }

                UsuarioDto usuario = new UsuarioDto()
                {
                    Id = opResult.Data.Id,
                    Nombre = opResult.Data.Nombre,
                    Correo = opResult.Data.Correo,
                    Contraseña = opResult.Data.Contraseña,
                    Rol = opResult.Data.Rol,
                    Estado = opResult.Data.Estado
                };

                result.Success = true;
                result.Data = usuario;
                result.Message = $"Se obtuvo el usuario con id {id} correctamnete.";

            }
            catch (Exception ex)
            {
                result.Message = $"Error al obtener al usuario por id.";
            }
            return result;
        }
        public async Task<ServiceResult> UpdateAsync(UpdateUsuarioDto UpdateDto)
        {
            ServiceResult result = new ServiceResult();
            try
            {
                var validate = new UsuarioValidatorServices(_usuarioRepository).ValidateUpdate(UpdateDto, out string errorMessage);
                if (!validate)
                {
                    result.Message = errorMessage;
                    return result;
                }
                var usuario = await _usuarioRepository.GetByIdAsync(UpdateDto.Id);
                if (!usuario.Success)
                {
                    result.Message = usuario.Message;
                    return result;
                }

                usuario.Data.Nombre = UpdateDto.Nombre;
                usuario.Data.Correo = UpdateDto.Correo;
                usuario.Data.Contraseña = UpdateDto.Contraseña;
                usuario.Data.Estado = UpdateDto.Estado;
                usuario.Data.Rol = UpdateDto.Rol;
                
                if(UpdateDto.Estado == EstadoUsuario.Inactivo ||
                   UpdateDto.Estado == EstadoUsuario.Suspendido ||
                   UpdateDto.Estado == EstadoUsuario.Eliminado)
                {
                    var sesionResult = await _sesionRepository.GetActiveSesionByUserAsync(usuario.Data.Id);
                    if (sesionResult.Success && sesionResult.Data != null)
                    {
                        sesionResult.Data.Estado = false;
                        var sesion = await _sesionRepository.UpdateAsync(sesionResult.Data);
                        if (!sesion.Success)
                        {
                            result.Message = sesion.Message;
                            return result;
                        }
                    }
                }

                var OpResult = await _usuarioRepository.UpdateAsync(usuario.Data);
                if (!OpResult.Success)
                {
                    result.Message = OpResult.Message;
                    return result;
                }

                var getusuario = new UsuarioDto()
                {
                    Id = OpResult.Data.Id,
                    Nombre = OpResult.Data.Nombre,
                    Correo = OpResult.Data.Correo,
                    Rol = OpResult.Data.Rol
                };

                result.Success = true;
                result.Data = getusuario;
                result.Message = "Usuario actualizado correctamente.";

            }
            catch (Exception ex)
            {
                result.Message = $"Error actualizando al usuario: {ex.Message}";
            }
            return result;
        }

    }
}
