

using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Sesiones.Sesion;
using SGHR.Application.Interfaces.Sesion;
using SGHR.Domain.Enum.Usuario;
using SGHR.Domain.Repository;
using SGHR.Persistence.Interfaces.Sesiones;

namespace SGHR.Application.Services.Sesion
{
    public class SesionServices : ISesionServices
    {
        private readonly ISesionRepository _sesionRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private const double TIEMPO_MAXIMO_INACTIVIDAD = 10;


        public SesionServices(ISesionRepository sesionRepository, IUsuarioRepository usuarioRepository)
        {
            _sesionRepository = sesionRepository;
            _usuarioRepository = usuarioRepository;
        }

        public async Task<ServiceResult> GetSesionByIdUser(int idUser)
        {
            ServiceResult result = new ServiceResult();
            try
            {
                var opResult = await _sesionRepository.GetActiveSesionByUserAsync(idUser);
                if (!opResult.Success)
                {
                    result.Success = false;
                    result.Message = opResult.Message;
                    return result;
                }

                if (opResult.Data.Id == null)
                {
                    result.Success = false;
                    result.Message = $"No se encontró una sesión activa para el usuario con id {idUser}.";
                    return result;

                }

                var sesion = new SesionDto
                {
                    Id = opResult.Data.Id,
                    IdUsuario = opResult.Data.IdUsuario,
                    Estado = opResult.Data.Estado,
                    FechaInicio = opResult.Data.FechaInicio,
                    FechaFin = opResult.Data.FechaFin

                };


                result.Success = true;
                result.Data = sesion;
                result.Message = $"Se obtuvo la sesión del usuario con id {idUser} correctamente.";

            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Error al obtener la sesión: {ex.Message}";
            }
            return result;
        }
        public async Task<ServiceResult> CloseSesionAsync(int idUsuario)
        {
            ServiceResult result = new ServiceResult();
            try
            {
                var opResult = await _sesionRepository.GetActiveSesionByUserAsync(idUsuario);
                if (!opResult.Success)
                {
                    result.Success = false;
                    result.Message = opResult.Message;
                    return result;
                }
                var sesion = opResult.Data;
                sesion.Estado = false;
                sesion.FechaFin = DateTime.Now;
                var updateResult = await _sesionRepository.UpdateAsync(sesion);
                if (!updateResult.Success)
                {
                    result.Success = false;
                    result.Message = updateResult.Message;
                    return result;
                }
                result.Success = true;
                result.Message = $"Sesión del usuario con id {idUsuario} cerrada correctamente.";
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Error al cerrar la sesión: {ex.Message}";
            }
            return result;
        }
        public async Task<ServiceResult> CheckActivitySesionGlobalAsync()
        {
            ServiceResult result = new ServiceResult();
            try
            {
                var sesiones = await _sesionRepository.GetActiveSessionsAsync();

                foreach (var s in sesiones.Data)
                {
                    if ((DateTime.Now - s.UltimaActividad).TotalMinutes > TIEMPO_MAXIMO_INACTIVIDAD)
                    {
                        s.Estado = false;
                        await _sesionRepository.UpdateAsync(s);

                        // También marca al usuario como inactivo
                        var usuario = await _usuarioRepository.GetByIdAsync(s.IdUsuario);
                        if (usuario.Success)
                        {
                            usuario.Data.Estado = EstadoUsuario.Inactivo;
                            await _usuarioRepository.UpdateAsync(usuario.Data);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Error al detectar la actividad de la sesión: {ex.Message}";
            }
            return result;
        }
        public async Task<ServiceResult> CheckActivitySesionByUserAsync(int idUsuario)
        {
            ServiceResult result = new ServiceResult();
            try
            {
                var opResult = await _sesionRepository.GetActiveSesionByUserAsync(idUsuario);
                if (!opResult.Success)
                {
                    result.Success = false;
                    result.Message = opResult.Message;
                    return result;
                }

                if (opResult.Data.Id == null)
                {
                    result.Success = false;
                    result.Message = $"No se encontró una sesión activa para el usuario con id {idUsuario}.";
                    return result;
                }

                var sesion = new CheckSesionDto
                {
                    IdSesion = opResult.Data.Id,
                    IdUsuario = opResult.Data.IdUsuario,
                    Estado = opResult.Data.Estado
                };

                result.Success = true;
                result.Message = "Actividad de sesión verificada correctamente.";
                result.Data = sesion;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Error al detectar la actividad de la sesión: {ex.Message}";
            }
            return result;
        }
        public async Task<ServiceResult> UpdateActivitySesionByUserAsync(int idsesion)
        {
            ServiceResult result = new ServiceResult();
            try
            {
                var opResult = await _sesionRepository.GetByIdAsync(idsesion);
                if (!opResult.Success)
                {
                    result.Success = false;
                    result.Message = opResult.Message;
                    return result;
                }
                if (opResult.Data.Id == null)
                {
                    result.Success = false;
                    result.Message = $"No se encontró la sesion activa con id {idsesion}.";
                    return result;
                }
                var sesion = opResult.Data;
                sesion.UltimaActividad = DateTime.Now;
                var updateResult = await _sesionRepository.UpdateAsync(sesion);
                if (!updateResult.Success)
                {
                    result.Success = false;
                    result.Message = updateResult.Message;
                    return result;
                }

                var checksesionDto = new CheckSesionDto
                {
                    IdSesion = opResult.Data.Id,
                    IdUsuario = opResult.Data.IdUsuario,
                    Estado = opResult.Data.Estado
                };

                result.Success = true;
                result.Data = checksesionDto;
                result.Message = "Actividad de sesión actualizada correctamente.";
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Error al actualizar la actividad de la sesión: {ex.Message}";
            }
            return result;

        }
    }
}
