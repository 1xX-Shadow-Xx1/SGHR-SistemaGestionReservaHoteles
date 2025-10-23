using Microsoft.Extensions.Logging;
using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Operaciones.Auditory;
using SGHR.Application.Dtos.Configuration.Sesiones.Sesion;
using SGHR.Application.Dtos.Configuration.Users.Usuario;
using SGHR.Application.Interfaces.Operaciones;
using SGHR.Application.Interfaces.Sesiones;
using SGHR.Application.Interfaces.Users;
using SGHR.Domain.Entities.Configuration.Usuers;
using SGHR.Domain.Repository;
using SGHR.Persistence.Interfaces.Sesiones;

namespace SGHR.Application.Services.Users
{
    public class UsuarioService : IUsuarioService
    {
        public readonly ILogger<UsuarioService> _logger;
        public readonly IUsuarioRepository _usuarioRepository;
        public readonly IAuditoryService _auditoryService;
        public readonly ISesionRepository _sesionRepository;
        public readonly ISesionServices _sesionServices;

        public UsuarioService(IUsuarioRepository usuarioRepository,
                             ILogger<UsuarioService> logger,
                             ISesionRepository sesionRepository,
                             ISesionServices sesionServices,
                             IAuditoryService auditoryService)
        {
            _usuarioRepository = usuarioRepository;
            _logger = logger;
            _auditoryService = auditoryService;
            _sesionRepository = sesionRepository;
            _sesionServices = sesionServices;
        }

        public async Task<ServiceResult> GetAll()
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando obtención de todos los usuarios.");
            try
            {

                var opResult = await _usuarioRepository.GetAll();
                if (!opResult.Success)
                {
                    result.Success = false;
                    result.Message = opResult.Message;
                    return result;
                }

                result.Success = true;
                result.Data = opResult.Data;
                result.Message = "Se obtuvieron los usuarios exitosamente";
                _logger.LogInformation(result.Message);

                var auditory = new CreateAuditoryDto
                {
                    IdUsuario = SesionDto.UsuarioID,
                    Operacion = "GetAll-Userios",
                    Detalle = ($"El usuario {SesionDto.UsuarioName} obtuvo una lista de todos los usuarios .")
                };

                var AudiResult = await _auditoryService.Save(auditory);
                if (!AudiResult.Success)
                {
                    return AudiResult;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los usuarios.");
                result.Success = false;
                result.Message = "Error al obtener los usuarios.";
            }
            return result;
        }
        public async Task<ServiceResult> GetById(int id)
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando obtención de usuario por ID: {Id}", id);

            try
            {
                var opResult = await _usuarioRepository.GetById(id);
                if (!opResult.Success)
                {
                    result.Success = false;
                    result.Message = opResult.Message;
                    return result;
                }
                result.Success = true;
                result.Data = opResult.Data;
                result.Message = opResult.Message;
                _logger.LogInformation("Usuario obtenido correctamente.");

                var auditory = new CreateAuditoryDto
                {
                    IdUsuario = SesionDto.UsuarioID,
                    Operacion = "GetById-Usuario",
                    Detalle = ($"El usuario {SesionDto.UsuarioName} solicito obtener al usurio con ID: {id}.")
                };
                var AudiResult = await _auditoryService.Save(auditory);
                if (!AudiResult.Success)
                {
                    return AudiResult;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el usuario.");
                result.Success = false;
                result.Message = "Error al obtener el usuario.";
            }
            return result;
        }
        public async Task<ServiceResult> LoginAsync(string email, string password)
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation($"Iniciando proceso de Login al usuario {email}");

            _logger.LogInformation($"Obteniendo credenciales Usuario {email}");
            var opResult = await _usuarioRepository.GetByEmailAndPassword(email, password);
            if (opResult.Success)
            {
                result.Success = true;
                result.Data = opResult.Data;
                result.Message = "Login exitoso.";
                _logger.LogInformation($"El Usuario {email} se a logeado correctamente, iniciando Sesion.");

                var Sesion = await _sesionServices.OpenSesion(opResult.Data.ID);
                if(!Sesion.Success)
                {
                    return result = Sesion;
                }
                SesionDto.SesionID = Sesion.Data.ID;
                SesionDto.UsuarioID = opResult.Data.ID;
                SesionDto.UsuarioName = opResult.Data.Correo;
                _logger.LogInformation("Sesion actual establecida correctamente.");

                var auditory = new CreateAuditoryDto
                {
                    IdUsuario = SesionDto.UsuarioID,
                    Operacion = "Login",
                    Detalle = ($"El Usuario {opResult.Data.Nombre}, a iniciado Sesion.")
                };

                var AudiResult = await _auditoryService.Save(auditory);
                if (!AudiResult.Success)
                {
                    return AudiResult;
                }
            }
            else
            {
                _logger.LogWarning($"No se pudo obtener las credenciales correctamente: {result.Message}");
                result.Success = false;
                result.Message = opResult.Message;
            }
            return result;
        }
        public async Task<ServiceResult> CloseAsync()
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando Cierre de Sesion");

            var SesionExist = await _sesionServices.CloseSesion();
            if (!SesionExist.Success)
            {
                return SesionExist;
            }
            result = SesionExist;

            var auditory = new CreateAuditoryDto
            {
                IdUsuario = SesionDto.UsuarioID,
                Operacion = "Close-Sesion",
                Detalle = ($"El Usuario {SesionDto.UsuarioName} cerro sesion.")
            };
            var AudiResult = await _auditoryService.Save(auditory);
            if (!AudiResult.Success)
            {
                return AudiResult;
            }

            return result;
        }
        public async Task<ServiceResult> Remove(int id)
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando eliminación de usuario con ID: {Id}", id);

            try
            {
                _logger.LogInformation("Verificando de que se ingreso un ID valido.");
                if (id <= 0)
                {
                    _logger.LogWarning("El id ingresado no es valido.");
                    result.Success = false;
                    result.Message = "El ID del usuario no es válido.";
                    return result;
                }
                _logger.LogInformation("El id es valido, iniciando el proceso de obtener usuario por id.");

                var usuarioExist = await _usuarioRepository.GetById(id);
                if (!usuarioExist.Success)
                {
                    _logger.LogWarning("No se pudo obtener el usuario: " + usuarioExist.Message);
                    result.Success = false;
                    result.Message = usuarioExist.Message;
                    return result;
                }

                _logger.LogInformation($"Iniciando borrado logico del usuario {usuarioExist.Message}");
                var opResult = await _usuarioRepository.Delete(usuarioExist.Data);
                if (!opResult.Success)
                {
                    result.Success = false;
                    result.Message = opResult.Message;
                    return result;
                }

                _logger.LogInformation($"Usuario {opResult.Data.Correo} eliminado correctamente.");
                result.Success = true;
                result.Data = opResult.Data;
                result.Message = opResult.Message;

                var auditory = new CreateAuditoryDto
                {
                    IdUsuario = SesionDto.UsuarioID,
                    Operacion = "Remove-UserbyID",
                    Detalle = ($"El Usuario {SesionDto.UsuarioName} elimino a {opResult.Data.Correo}.")
                };

                var AudiResult = await _auditoryService.Save(auditory);
                if (!AudiResult.Success)
                {
                    return AudiResult;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error al eliminar el usuario: " + ex.Message);
                result.Success = false;
                result.Message = "Error al eliminar el usuario.";
            }
            return result;
        }
        public async Task<ServiceResult> Save(CreateUsuarioDto createUsuarioDto)
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando creación de nuevo usuario.", createUsuarioDto);

            try
            {
                Usuario usuario = new Usuario
                {
                    Nombre = createUsuarioDto.Nombre,
                    Correo = createUsuarioDto.Correo,
                    Contraseña = createUsuarioDto.Contraseña,
                    Rol = createUsuarioDto.Rol
                };

                var opResult = await  _usuarioRepository.Save(usuario);
                if (!opResult.Success)
                {
                    result.Success = false;
                    result.Message = opResult.Message;
                }
                result.Success = true;
                result.Data = opResult.Data;
                result.Message = $"El Usuario {opResult.Data.Nombre} se creo correctamente";
                _logger.LogInformation($"Se creo el usuario correctamente.");

                var auditory = new CreateAuditoryDto
                {
                    IdUsuario = opResult.Data.ID,
                    Operacion = "Registrar",
                    Detalle = ($"Se creo un nuevo usuario.")
                };

                var AudiResult = await _auditoryService.Save(auditory);
                if (!AudiResult.Success)
                {
                    return AudiResult;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el usuario.");
                result.Success = false;
                result.Message = "Error al crear el usuario.";
            }
            return result;
        }
        public async Task<ServiceResult> Update(UpdateUsuarioDto updateUsuarioDto)
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando actualización de usuario con ID: {Id}", updateUsuarioDto.Id);

            try
            {
                _logger.LogInformation("Verificando que exista el usuario con el id ingresado.");
                var usuarioExists = await _usuarioRepository.GetById(updateUsuarioDto.Id);
                if (!usuarioExists.Success || usuarioExists.Data == null)
                {
                    _logger.LogWarning("No se encontro ningun usuario con ese ID {id}", updateUsuarioDto.Id);
                    result.Success = false;
                    result.Message = usuarioExists.Message;
                    return result;
                }

                _logger.LogInformation("Se inicio la actualizacion de los datos del Usuario.");
                usuarioExists.Data.Nombre = updateUsuarioDto.Nombre;
                usuarioExists.Data.Correo = updateUsuarioDto.Correo;
                usuarioExists.Data.Contraseña = updateUsuarioDto.Contraseña;
                usuarioExists.Data.Rol = updateUsuarioDto.Rol;
                usuarioExists.Data.Estado = updateUsuarioDto.Estado;

                var opResult = await  _usuarioRepository.Update(usuarioExists.Data);
                if (!opResult.Success)
                {
                    result.Success = false;
                    result.Message = opResult.Message;
                }
                result.Success = true;
                result.Data = opResult.Data;
                result.Message = opResult.Message;
                _logger.LogInformation("Usuario actualizado correctamente.");

                var auditory = new CreateAuditoryDto
                {
                    IdUsuario = opResult.Data.ID,
                    Operacion = "Registrar",
                    Detalle = ($"Se creo un nuevo usuario.")
                };

                var AudiResult = await _auditoryService.Save(auditory);
                if (!AudiResult.Success)
                {
                    return AudiResult;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el usuario.");
                result.Success = false;
                result.Message = "Error al actualizar el usuario.";
            }
            return result;
        }
    }
}
