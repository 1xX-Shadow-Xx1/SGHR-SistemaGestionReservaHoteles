
using Microsoft.Extensions.Logging;
using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Users.Usuario;
using SGHR.Application.Interfaces;
using SGHR.Domain.Entities.Configuration.Sesiones;
using SGHR.Domain.Entities.Configuration.Usuers;
using SGHR.Domain.Enum.Usuario;
using SGHR.Domain.Enum.Usuarios;
using SGHR.Domain.Repository;
using SGHR.Persistence.Interfaces.Sesiones;

namespace SGHR.Application.Services
{
    public class AuthenticationServices : IAuthenticationServices
    {
        public readonly ILogger<AuthenticationServices> _logger;
        public readonly IUsuarioRepository _usuarioRepository;
        public readonly ISesionRepository _sesionRepository;

        public AuthenticationServices(ILogger<AuthenticationServices> logger, 
                                      IUsuarioRepository usuarioRepository,
                                      ISesionRepository sesionRepository)
        {
            _logger = logger;
            _usuarioRepository = usuarioRepository;
            _sesionRepository = sesionRepository;
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
                        Rol = OpResult.Data.Rol
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
                        Rol = existUser.Data.Rol,
                        Estado = existUser.Data.Estado
                    };

                    var sesion = new SGHR.Domain.Entities.Configuration.Sesiones.Sesion
                    {
                        IdUsuario = usuario.Id,
                        Estado = true,
                        FechaInicio = DateTime.Now,
                        UltimaActividad = DateTime.Now
                    };
                    
                    var opSesion = await _sesionRepository.SaveAsync(sesion);
                    if (!opSesion.Success)
                    {
                        result.Message = opSesion.Message;
                        return result;
                    }

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
        public async Task<ServiceResult> CloseSesionAsync(int idusuario)
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

                    if(existUser.Data.Estado != EstadoUsuario.Suspendido && existUser.Data.Estado != EstadoUsuario.Eliminado)
                    {
                        existUser.Data.Estado = EstadoUsuario.Inactivo;
                    }
                    
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
                        Rol = OpResult.Data.Rol,
                        Estado = OpResult.Data.Estado
                    };

                    result.Success = true;
                    result.Message = $"Sesion cerrada correctamente.";
                    result.Data = usuario;
                }


            }
            catch (Exception ex)
            {
                result.Message = "Ocurrio un error al cerrar la sesion";
            }
            return result;
        }
    }
}
