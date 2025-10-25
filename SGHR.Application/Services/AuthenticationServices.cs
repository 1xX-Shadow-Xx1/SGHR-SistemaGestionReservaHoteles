
using Microsoft.Extensions.Logging;
using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Users.Usuario;
using SGHR.Application.Interfaces;
using SGHR.Domain.Entities.Configuration.Usuers;
using SGHR.Domain.Enum.Usuario;
using SGHR.Domain.Enum.Usuarios;
using SGHR.Domain.Repository;

namespace SGHR.Application.Services
{
    public class AuthenticationServices : IAuthenticationServices
    {
        public readonly ILogger<AuthenticationServices> _logger;
        public readonly IUsuarioRepository _usuarioRepository;

        public AuthenticationServices(ILogger<AuthenticationServices> logger, 
                                      IUsuarioRepository usuarioRepository)
        {
            _logger = logger;
            _usuarioRepository = usuarioRepository;
        }

        public async Task<ServiceResult> RegistrarUsuarioAsync(CreateUsuarioDto createUsuarioDto)
        {
            ServiceResult result = new ServiceResult();
            if (createUsuarioDto == null)
            {
                result.Message = "El usuario no puede ser nulo.";
                return result;
            }
            try
            {
                var existUser = await _usuarioRepository.GetByCorreoAsync(createUsuarioDto.Correo);
                if (existUser.Success)
                {
                    result.Message = ("Ya existe un usuario con ese correo.");
                    return result;
                }
                if (!existUser.Success)
                {
                    Usuario usuario = new Usuario()
                    {
                        Nombre = createUsuarioDto.Nombre,
                        Correo = createUsuarioDto.Correo,
                        Contraseña = createUsuarioDto.Contraseña,
                        Rol = RolUsuarios.Cliente,
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
                        Rol = OpResult.Data.Rol.ToString()
                    };

                    result.Success = true;
                    result.Data = getusuario;
                    result.Message = "Usuario registrado correctamente.";
                }

            }
            catch (Exception ex)
            {
                result.Message = $"Error al registrar el usuario: {ex.Message}";
            }
            return result;
        }
        public async Task<ServiceResult> LoginSesionAsync(string correo, string password)
        {
            ServiceResult result = new ServiceResult();

            try
            {
                var existUser = await _usuarioRepository.GetByCorreoAsync(correo);
                if (!existUser.Success)
                {
                    result.Message = ("No se a encontrado el usuario.");
                    return result;
                }

                if (existUser.Success)
                {
                    switch (existUser.Data.Estado)
                    {
                        case (EstadoUsuario.Suspendido):
                            result.Message = "Usuario suspendido, no puede iniciar sesion.";
                            return result;
                        case (EstadoUsuario.Eliminado):
                            result.Message = "El usuario a sido eliminado y por lo que no puede logearse.";
                            return result;
                    }
                }

                if(existUser.Success && existUser.Data.Contraseña == password)
                {
                    existUser.Data.Estado = EstadoUsuario.Activo;
                    var OpResult = await _usuarioRepository.UpdateAsync(existUser.Data);
                    if (!OpResult.Success)
                    {
                        result.Message = OpResult.Message;
                        return result;
                    }

                    var usuario = new UsuarioDto
                    {
                        Id = existUser.Data.Id,
                        Nombre = existUser.Data.Nombre,
                        Correo = existUser.Data.Correo,
                        Rol = existUser.Data.Rol.ToString(),
                        Estado = existUser.Data.Estado.ToString()
                    };

                    result.Success = true;
                    result.Message = $"Usuario {existUser.Data.Nombre} Logeado correctamente.";
                    result.Data = usuario;
                }
                else
                {
                    result.Message = "Contraseña incorrecta.";
                }
            }
            catch (Exception ex)
            {
                result.Message = $"Error al hacer la autenticacion: {ex.Message}";
            }
            return result;
        }  
        public async Task<ServiceResult> CloseSerionAsync(int idusuario)
        {
            ServiceResult result = new ServiceResult();
            if (idusuario < 0)
            {
                result.Message = $"El id ingresado no es valido.";
                return result;
            }
            try
            {
                var existUser = await _usuarioRepository.GetByIdAsync(idusuario);
                if (!existUser.Success)
                {
                    result.Message = ("No se a encontro el usuario.");
                    return result;
                }

                if (existUser.Success)
                {
                    if (existUser.Data.Estado == EstadoUsuario.Inactivo)
                    {
                        result.Message = "No puede cerrar cerrar sesion a un usuario que esta inactivo.";
                        return result;
                    }

                    existUser.Data.Estado = EstadoUsuario.Inactivo;
                    var OpResult = await _usuarioRepository.UpdateAsync(existUser.Data);
                    if (!OpResult.Success)
                    {
                        result.Message = OpResult.Message;
                        return result;
                    }

                    var usuario = new UsuarioDto
                    {
                        Id = OpResult.Data.Id,
                        Nombre = OpResult.Data.Nombre,
                        Correo = OpResult.Data.Correo,
                        Rol = OpResult.Data.Rol.ToString(),
                        Estado = OpResult.Data.Estado.ToString()
                    };

                    result.Success = true;
                    result.Message = $"Sesion cerrada correctamente.";
                    result.Data = usuario;
                }


            }
            catch (Exception ex)
            {

            }
            return result;
        }
    }
}
