
using Microsoft.Extensions.Logging;
using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Users.Usuario;
using SGHR.Application.Interfaces.Users;
using SGHR.Domain.Entities.Configuration.Usuers;
using SGHR.Domain.Repository;

namespace SGHR.Application.Services.Users
{
    public class UsuarioService : IUsuarioService
    {
        public readonly ILogger<UsuarioService> _logger;
        public readonly IUsuarioRepository _usuarioRepository;

        public UsuarioService(IUsuarioRepository usuarioRepository,
                             ILogger<UsuarioService> logger)
        {
            _usuarioRepository = usuarioRepository;
            _logger = logger;
        }

        public async Task<ServiceResult> GetAll()
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando obtención de todos los usuarios.");

            try
            {
                var opResult =  _usuarioRepository.GetAll();
                if (opResult.Result.Success)
                {
                    result.Success = true;
                    result.Data = opResult.Result.Data;
                    result.Message = "Se obtuvieron los usuarios exitosamente";
                }
                else
                {
                    result.Success = false;
                    result.Message = opResult.Result.Message;
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
                var opResult =  _usuarioRepository.GetById(id);
                if (opResult.Result.Success)
                {
                    result.Success = true;
                    result.Data = opResult.Result.Data;
                    result.Message = opResult.Result.Message;
                }
                else
                {
                    result.Success = false;
                    result.Message = opResult.Result.Message;
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

        public async Task<ServiceResult> GetUsuarioByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResult> LoginAsync(string email, string password)
        {
            var opResult = await _usuarioRepository.GetByEmailAndPassword(email, password);
            ServiceResult result = new ServiceResult();

            if (opResult != null)
            {
                result.Success = true;
                result.Data = opResult;
                result.Message = "Login exitoso.";
            }
            else
            {
                result.Success = false;
                result.Message = opResult.Message;
            }

            return result;
        }

        public async Task<ServiceResult> Remove(int id)
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando eliminación de usuario con ID: {Id}", id);

            try
            {
                if (id <= 0)
                {
                    result.Success = false;
                    result.Message = "El ID del usuario no es válido.";
                    return result;
                }

                var usuarioExist = await _usuarioRepository.GetById(id);
                if (!usuarioExist.Success)
                {
                    result.Success = false;
                    result.Message = usuarioExist.Message;
                    return result;
                }

                var opResult =  _usuarioRepository.Delete(usuarioExist.Data);
                if (opResult.Result.Success)
                {
                    result.Success = true;
                    result.Data = opResult.Result.Data;
                    result.Message = opResult.Result.Message;
                }
                else
                {
                    result.Success = false;
                    result.Message = opResult.Result.Message;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el usuario.");
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

                var opResult =  _usuarioRepository.Save(usuario);
                if (opResult.Result.Success)
                {
                    result.Success = true;
                    result.Data = opResult.Result.Data;
                    result.Message = $"El Usuario {opResult.Result.Data.Nombre} se creo correctamente";
                }
                else
                {
                    result.Success = false;
                    result.Message = opResult.Result.Message;
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
                var usuarioExists = await _usuarioRepository.GetById(updateUsuarioDto.Id);
                if (!usuarioExists.Success || usuarioExists.Data == null)
                {
                    result.Success = false;
                    result.Message = usuarioExists.Message;
                    return result;
                }

                usuarioExists.Data.Nombre = updateUsuarioDto.Nombre;
                usuarioExists.Data.Correo = updateUsuarioDto.Correo;
                usuarioExists.Data.Contraseña = updateUsuarioDto.Contraseña;
                usuarioExists.Data.Rol = updateUsuarioDto.Rol;
                usuarioExists.Data.Estado = updateUsuarioDto.Estado;

                var opResult =  _usuarioRepository.Update(usuarioExists.Data);
                if (opResult.Result.Success)
                {
                    result.Success = true;
                    result.Data = opResult.Result.Data;
                    result.Message = opResult.Result.Message;
                }
                else
                {
                    result.Success = false;
                    result.Message = opResult.Result.Message;
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
