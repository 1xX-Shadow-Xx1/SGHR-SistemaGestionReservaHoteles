using Microsoft.Extensions.Logging;
using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Users.Usuario;
using SGHR.Application.Interfaces.Usuarios;
using SGHR.Domain.Entities.Configuration.Usuers;
using SGHR.Domain.Enum.Usuarios;
using SGHR.Domain.Repository;

namespace SGHR.Application.Services.Usuarios
{
    public class UsuarioServices : IUsuarioServices
    {
        public readonly ILogger<UsuarioServices> _logger;
        public readonly IUsuarioRepository _usuarioRepository;

        public UsuarioServices(ILogger<UsuarioServices> logger,
                               IUsuarioRepository usuarioRepository)
        {
            _logger = logger;
            _usuarioRepository = usuarioRepository;
        }

        public async Task<ServiceResult> CreateAsync(CreateUsuarioDto CreateDto)
        {
            ServiceResult result = new ServiceResult();
            if (CreateDto == null)
            {
                result.Message = "El usuario no puede ser nulo.";
                return result;
            }
            try
            {
                var existUser = await _usuarioRepository.GetByCorreoAsync(CreateDto.Correo);
                if (existUser.Success)
                {
                    result.Message = ("Ya existe un usuario con ese correo.");
                    return result;
                }

                Usuario usuario = new Usuario()
                {
                    Nombre = CreateDto.Nombre,
                    Correo = CreateDto.Correo,
                    Contraseña = CreateDto.Contraseña
                };
                if (Enum.TryParse<RolUsuarios>(CreateDto.Rol, out var estado))
                {
                    usuario.Rol = estado;
                }

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
                    Rol = OpResult.Data.Estado.ToString()
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
            if (id <= 0)
            {
                result.Message = $"El id ingresado no es valido.";
                return result;
            }
            try
            {
                var existUsuario = await _usuarioRepository.GetByIdAsync(id);
                if (!existUsuario.Success)
                {
                    result.Message = $"No existe un usuario con ese id.";
                    return result;
                }

                var OpResult = await _usuarioRepository.DeleteAsync(existUsuario.Data);
                if (!OpResult.Success)
                {
                    result.Message = OpResult.Message;
                    return result;
                }
                result.Success = true;
                result.Message = $"Usuario {existUsuario.Data.Nombre} eliminado correctamente.";


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
                    Rol = u.Rol.ToString(),
                    Estado = u.Estado.ToString()
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
            if (id <= 0)
            {
                result.Message = $"El id ingresado no es valido.";
                return result;
            }
            try
            {
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
                    Rol = opResult.Data.Rol.ToString(),
                    Estado = opResult.Data.Estado.ToString()
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
            if (UpdateDto == null)
            {
                result.Message = "El usuario no puede ser nulo.";
                return result;
            }
            if (UpdateDto.Id <= 0)
            {
                result.Message = $"El id ingresado no es valido.";
                return result;
            }
            try
            {
                var existUser = await _usuarioRepository.GetByIdAsync(UpdateDto.Id);
                if (!existUser.Success)
                {
                    result.Message = existUser.Message;
                    return result;
                }

                if(existUser.Data.Correo != UpdateDto.Correo)
                {
                    var verification = await _usuarioRepository.GetByCorreoAsync(UpdateDto.Correo);
                    if (verification.Success)
                    {
                        result.Message = ("Ya existe un usuario con ese correo.");
                        return result;
                    }
                }

                existUser.Data.Nombre = UpdateDto.Nombre;
                existUser.Data.Correo = UpdateDto.Correo;
                existUser.Data.Contraseña = UpdateDto.Contraseña;

                if (Enum.TryParse<RolUsuarios>(UpdateDto.Rol, out var estado))
                {
                    existUser.Data.Rol = estado;
                }

                var OpResult = await _usuarioRepository.UpdateAsync(existUser.Data);
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
                    Rol = OpResult.Data.Estado.ToString()
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
