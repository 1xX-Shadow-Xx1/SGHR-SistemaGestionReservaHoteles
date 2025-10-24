using Microsoft.Extensions.Logging;
using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Users.Usuario;
using SGHR.Application.Interfaces.Users;
using SGHR.Domain.Entities.Configuration.Usuers;
using SGHR.Domain.Enum.Usuario;
using SGHR.Domain.Repository;
using System.Linq.Expressions;

namespace SGHR.Application.Services.Users
{


    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ILogger<UsuarioService> _logger;

        public UsuarioService(IUsuarioRepository usuarioRepository, ILogger<UsuarioService> logger)
        {
            _usuarioRepository = usuarioRepository;
            _logger = logger;
        }

        public async Task<ServiceResult> CreateAsync(UsuarioCreateDto usuarioCreateDto, int? sesionId = null)
        {
            ServiceResult result = new ServiceResult();

            try
            {
                var exist = await _usuarioRepository.GetByCorreoAsync(usuarioCreateDto.Correo);
                if (exist.Success)
                {
                    result.Success = false;
                    result.Message = "Ya existe un usuario registrado con ese correo.";
                    return result;
                }

                var usuario = new Usuario()
                {
                    Nombre = usuarioCreateDto.Nombre,
                    Correo = usuarioCreateDto.Correo,
                    Contraseña = usuarioCreateDto.Contraseña,
                    Rol = usuarioCreateDto.Rol
                };

                var opResult = await _usuarioRepository.SaveAsync(usuario,sesionId);
                if (!opResult.Success)
                {
                    result.Success = false;
                    result.Message = opResult.Message;
                    return result;
                }

                var usauriodto = new UsuarioDto()
                {
                    Nombre = opResult.Data.Nombre,
                    Correo = opResult.Data.Correo,
                    Estado = opResult.Data.Estado.ToString(),
                    Rol = opResult.Data.Rol
                };

                result.Success = true;
                result.Message = "Usuario registrado correctamente.";
                result.Data = usauriodto;

            }catch (Exception ex)
            {
                result.Success = false;
                result.Message = ($"Error al registrar un nuevo usuario: {ex.Message}");
            }
            return result;
        }
        public async Task<ServiceResult> DeleteAsync(int id, int? sesionId = null)
        {
            ServiceResult result = new ServiceResult();

            try
            {
                var existUser = await _usuarioRepository.GetByIdAsync(id);
                if (!existUser.Success)
                {
                    result.Success = false;
                    result.Message = existUser.Message;
                    return result;
                }

                var opResult = await _usuarioRepository.DeleteAsync(existUser.Data);
                if (!opResult.Success)
                {
                    result.Success = false;
                    result.Message = opResult.Message;
                    return result;
                }

                var usuarioDto = new UsuarioDto()
                {
                    Id = opResult.Data.Id,
                    Nombre = opResult.Data.Nombre,
                    Correo = opResult.Data.Correo,
                    Estado = opResult.Data.Estado.ToString(),
                    Rol =opResult.Data.Rol
                };

                result.Success = true;
                result.Data = usuarioDto;
                result.Message = "El usuario eliminado correctamente.";

            }catch (Exception ex)
            {
                result.Success = false;
                result.Message = ($"Error al eliminar el usuario: {ex.Message}");
            }
            return result;
        }
        public async Task<ServiceResult> ExistsAsync(Expression<Func<UsuarioDto, bool>> filter)
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando comprobación de existencia de usuario (DTO)");

            try
            {
                Expression<Func<Usuario, bool>> entityFilter = u =>
                    filter.Compile()(new UsuarioDto
                    {
                        Id = u.Id,
                        Nombre = u.Nombre,
                        Correo = u.Correo,
                        Estado = u.Estado.ToString(),
                        Rol = u.Rol
                    });

                var repoResult = await _usuarioRepository.ExistsAsync(entityFilter);
                if (!repoResult.Success)
                {
                    _logger.LogError("Error verificando existencia de usuario en repositorio: {Message}", repoResult.Message);
                    result.Success = false;
                    result.Message = repoResult.Message;
                    return result;
                }

                result.Success = true;
                result.Message = "Existencia comprobada.";
                result.Data = repoResult.Data;
                _logger.LogInformation("Comprobación de existencia completada, resultado: {Exists}", repoResult.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error verificando existencia de usuario (DTO)");
                result.Success = false;
                result.Message = ($"Ocurrió un error al comprobar la existencia del usuario: {ex.Message}");
            }
            return result;
        }
        public async Task<ServiceResult> GetActivosAsync()
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando obtención de usuarios activos");

            try
            {
                var repoResult = await _usuarioRepository.GetActivosAsync();

                if (!repoResult.Success)
                {
                    result.Success = false;
                    result.Message = repoResult.Message;
                    _logger.LogError("Error obteniendo usuarios activos: {Message}", repoResult.Message);
                    return result;
                }

                var usuariosDto = repoResult.Data!.Select(u => new UsuarioDto
                {
                    Id = u.Id,
                    Nombre = u.Nombre,
                    Correo = u.Correo,
                    Estado = u.Estado.ToString(),
                    Rol = u.Rol
                }).ToList();
                result.Success = true;
                result.Message = "Usuarios obtenidos correctamente.";
                result.Data = usuariosDto;
                _logger.LogInformation("Usuarios activos obtenidos correctamente, total: {Count}", usuariosDto.Count);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Excepción al obtener usuarios activos");
                result.Success = false;
                result.Message = ($"Ocurrió un error al obtener los usuarios activos: {ex.Message}");
            }
            return result;
        }
        public async Task<ServiceResult> GetAllAsync()
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando obtención de todos los usuarios");

            try
            {
                var repoResult = await _usuarioRepository.GetAllAsync();

                if (!repoResult.Success)
                {
                    _logger.LogError("Error obteniendo todos los usuarios: {Message}", repoResult.Message);
                    result.Success = false;
                    result.Message = repoResult.Message;
                    return result;
                }

                var usuariosDto = repoResult.Data!.Select(u => new UsuarioDto
                {
                    Id = u.Id,
                    Nombre = u.Nombre,
                    Correo = u.Correo,
                    Estado = u.Estado.ToString(),
                    Rol = u.Rol
                }).ToList();

                result.Success = true;
                result.Message = "Usuarios obtenidos correctamente.";
                result.Data = usuariosDto;
                _logger.LogInformation("Todos los usuarios obtenidos correctamente, total: {Count}", usuariosDto.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Excepción al obtener todos los usuarios");
                result.Success = false;
                result.Message = ($"Ocurrió un error al obtener los usuarios: {ex.Message}");
            }
            return result;
        }
        public async Task<ServiceResult> GetAllByAsync(Expression<Func<UsuarioDto, bool>> filter)
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando obtención de usuarios con filtro");

            try
            {
                Expression<Func<Usuario, bool>> repoFilter = u =>
                filter.Compile().Invoke(new UsuarioDto
                {
                    Id = u.Id,
                    Nombre = u.Nombre,
                    Correo = u.Correo,
                    Estado = u.Estado.ToString(),
                    Rol = u.Rol
                });

                var repoResult = await _usuarioRepository.GetAllByAsync(repoFilter);

                if (!repoResult.Success)
                {
                    result.Success = false;
                    result.Message = repoResult.Message;
                    _logger.LogError("Error obteniendo usuarios del repositorio: {Message}", repoResult.Message);
                    return result;
                }

                var usuariosDto = repoResult.Data!
                    .Select(u => new UsuarioDto
                    {
                        Id = u.Id,
                        Nombre = u.Nombre,
                        Correo = u.Correo,
                        Estado = u.Estado.ToString(),
                        Rol = u.Rol
                    })
                    .AsQueryable()
                    .Where(filter) 
                    .ToList();

                result.Success = true;
                result.Message = repoResult.Message;
                result.Data = usuariosDto;

                _logger.LogInformation("Usuarios filtrados obtenidos correctamente, total: {Count}", usuariosDto.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Excepción al obtener usuarios filtrados");
                result.Success = false;
                result.Message = ($"Ocurrió un error al obtener los usuarios filtrados: {ex.Message}");
            }
            return result;
        }
        public async Task<ServiceResult> GetByCorreoAsync(string correo)
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando obtención de usuario por correo: {Correo}", correo);

            try
            {
                var repoResult = await _usuarioRepository.GetByCorreoAsync(correo);

                if (!repoResult.Success || repoResult.Data == null)
                {
                    result.Success = false;
                    result.Message = repoResult.Message ?? $"Usuario con correo {correo} no encontrado";
                    _logger.LogWarning("Usuario con correo {Correo} no encontrado", correo);
                    return result;
                }

                var usuarioDto = new UsuarioDto
                {
                    Id = repoResult.Data.Id,
                    Nombre = repoResult.Data.Nombre,
                    Correo = repoResult.Data.Correo,
                    Estado = repoResult.Data.Estado.ToString(),
                    Rol = repoResult.Data.Rol
                };

                result.Success = true;
                result.Data = usuarioDto;
                result.Message = "Usuario obtenido correctamente";
                _logger.LogInformation("Usuario con correo {Correo} obtenido correctamente", correo);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Ocurrió un error al obtener el usuario: {ex.Message}";
                _logger.LogError(ex, "Error obteniendo usuario por correo: {Correo}", correo);
            }

            return result;
        }
        public async Task<ServiceResult> GetByIdAsync(int id)
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando obtención de usuario por Id: {Id}", id);

            try
            {
                var repoResult = await _usuarioRepository.GetByIdAsync(id);

                if (!repoResult.Success || repoResult.Data == null)
                {
                    result.Success = false;
                    result.Message = repoResult.Message ?? $"Usuario con Id {id} no encontrado";
                    _logger.LogWarning("Usuario con Id {Id} no encontrado", id);
                    return result;
                }

                var usuarioDto = new UsuarioDto
                {
                    Id = repoResult.Data.Id,
                    Nombre = repoResult.Data.Nombre,
                    Correo = repoResult.Data.Correo,
                    Estado = repoResult.Data.Estado.ToString(),
                    Rol = repoResult.Data.Rol
                };

                result.Success = true;
                result.Data = usuarioDto;
                result.Message = "Usuario obtenido correctamente";
                _logger.LogInformation("Usuario con Id {Id} obtenido correctamente", id);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Ocurrió un error al obtener el usuario: {ex.Message}";
                _logger.LogError(ex, "Error obteniendo usuario por Id: {Id}", id);
            }

            return result;
        }
        public async Task<ServiceResult> GetByRolAsync(string rol)
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando obtención de usuarios por rol: {Rol}", rol);

            try
            {
                var repoResult = await _usuarioRepository.GetByRolAsync(rol);

                if (!repoResult.Success || repoResult.Data == null || !repoResult.Data.Any())
                {
                    result.Success = false;
                    result.Message = repoResult.Message ?? $"No se encontraron usuarios con el rol '{rol}'";
                    _logger.LogWarning("No se encontraron usuarios con rol: {Rol}", rol);
                    return result;
                }

                var usuariosDto = repoResult.Data
                    .Select(u => new UsuarioDto
                    {
                        Id = u.Id,
                        Nombre = u.Nombre,
                        Correo = u.Correo,
                        Estado = u.Estado.ToString(),
                        Rol = u.Rol
                    })
                    .ToList();

                result.Success = true;
                result.Data = usuariosDto;
                result.Message = "Usuarios obtenidos correctamente";
                _logger.LogInformation("{Count} usuarios con rol {Rol} obtenidos correctamente", usuariosDto.Count, rol);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Ocurrió un error al obtener los usuarios por rol: {ex.Message}";
                _logger.LogError(ex, "Error obteniendo usuarios con rol: {Rol}", rol);
            }

            return result;
        }
        public async Task<ServiceResult> LoginAsync(UsuarioLoginDto usuarioLoginDto)
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando login para el usuario con correo: {Correo}", usuarioLoginDto.Correo);

            try
            {
                // Obtener el usuario por correo
                var repoResult = await _usuarioRepository.GetByCorreoAsync(usuarioLoginDto.Correo);

                if (!repoResult.Success || repoResult.Data == null)
                {
                    result.Success = false;
                    result.Message = "Usuario no encontrado.";
                    _logger.LogWarning("Intento de login fallido: usuario no encontrado para correo {Correo}", usuarioLoginDto.Correo);
                    return result;
                }

                var usuario = repoResult.Data;

                // Validar contraseña (en un escenario real, usar hash y salt)
                if (usuario.Contraseña != usuarioLoginDto.Contraseña)
                {
                    result.Success = false;
                    result.Message = "Contraseña incorrecta.";
                    _logger.LogWarning("Intento de login fallido: contraseña incorrecta para correo {Correo}", usuarioLoginDto.Correo);
                    return result;
                }

                // Mapeo a DTO para devolver datos
                var usuarioDto = new UsuarioDto
                {
                    Id = usuario.Id,
                    Nombre = usuario.Nombre,
                    Correo = usuario.Correo,
                    Rol = usuario.Rol,
                    Estado = usuario.Estado.ToString()
                };

                result.Success = true;
                result.Data = usuarioDto;
                result.Message = "Login exitoso.";
                _logger.LogInformation("Usuario con correo {Correo} ha iniciado sesión correctamente", usuarioLoginDto.Correo);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Ocurrió un error durante el login: {ex.Message}";
                _logger.LogError(ex, "Error durante el login para el usuario con correo {Correo}", usuarioLoginDto.Correo);
            }

            return result;
        }
        public async Task<ServiceResult> UpdateAsync(UsuarioUpdateDto usuarioUpdateDto, int? sesionId = null)
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando actualización del usuario con Id: {Id}", usuarioUpdateDto.Id);

            try
            {
                // Obtener el usuario existente
                var repoResult = await _usuarioRepository.GetByIdAsync(usuarioUpdateDto.Id);
                if (!repoResult.Success || repoResult.Data == null)
                {
                    result.Success = false;
                    result.Message = "Usuario no encontrado.";
                    _logger.LogWarning("Actualización fallida: usuario no encontrado con Id {Id}", usuarioUpdateDto.Id);
                    return result;
                }

                var usuario = repoResult.Data;

                // Actualizar los campos permitidos
                usuario.Nombre = usuarioUpdateDto.Nombre;
                usuario.Correo = usuarioUpdateDto.Correo;
                if (!string.IsNullOrWhiteSpace(usuarioUpdateDto.Contraseña))
                {
                    usuario.Contraseña = usuarioUpdateDto.Contraseña; 
                }
                usuario.Rol = usuarioUpdateDto.Rol;
                // Si el estado se permite actualizar desde DTO
                if (!string.IsNullOrWhiteSpace(usuarioUpdateDto.Estado))
                {
                    if (Enum.TryParse(usuarioUpdateDto.Estado, out EstadoUsuario estado))
                    {
                        usuario.Estado = estado;
                    }
                }

                // Guardar cambios usando el repositorio
                var updateResult = await _usuarioRepository.UpdateAsync(usuario, sesionId);
                if (!updateResult.Success)
                {
                    result.Success = false;
                    result.Message = updateResult.Message;
                    _logger.LogError("Error actualizando usuario con Id {Id}: {Message}", usuario.Id, updateResult.Message);
                    return result;
                }

                // Mapear a DTO
                var usuarioDto = new UsuarioDto
                {
                    Id = usuario.Id,
                    Nombre = usuario.Nombre,
                    Correo = usuario.Correo,
                    Rol = usuario.Rol,
                    Estado = usuario.Estado.ToString()
                };

                result.Success = true;
                result.Data = usuarioDto;
                result.Message = "Usuario actualizado correctamente.";
                _logger.LogInformation("Usuario con Id {Id} actualizado correctamente", usuario.Id);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Ocurrió un error al actualizar el usuario: {ex.Message}";
                _logger.LogError(ex, "Excepción al actualizar usuario con Id {Id}", usuarioUpdateDto.Id);
            }

            return result;
        }

    }
}
