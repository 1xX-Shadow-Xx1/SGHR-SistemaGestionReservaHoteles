using Microsoft.Extensions.Logging;
using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Sesiones.Sesion;
using SGHR.Application.Interfaces.Sesiones;
using SGHR.Domain.Entities.Configuration.Sesiones;
using SGHR.Domain.Repository;
using SGHR.Persistence.Interfaces.Sesiones;


namespace SGHR.Application.Services.Sesiones
{
    public class SesionesServices : ISesionServices
    {
        public readonly ILogger<SesionesServices> _logger;
        public readonly ISesionRepository _sesionRepository;
        public readonly IUsuarioRepository _usuarioRepository;

        public SesionesServices(ILogger<SesionesServices> logger,
                                ISesionRepository sesionRepository,
                                IUsuarioRepository usuarioRepository)
        {            
            _logger = logger;
            _sesionRepository = sesionRepository;
            _usuarioRepository = usuarioRepository;
        }

        public async Task<ServiceResult> OpenSesionAsync(StartSesionDto startSesionDto)
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation($"Iniciando con el proceso de abrir sesion del usuario con ID: {startSesionDto.IdUsuario}.");

            try
            {
                _logger.LogInformation("Creando la sesion");
                Sesion sesion  = new Sesion
                {
                    IdUsuario = startSesionDto.IdUsuario,
                    Estado = true
                };

                _logger.LogInformation("Guardando la sesion en la Base de datos");
                var OpResult = await _sesionRepository.SaveAsync(sesion);
                if (OpResult.Success)
                {
                    _logger.LogInformation("Sesion guardad correctamente en la base de datos.");
                    result.Success = true;
                    result.Message = OpResult.Message;
                    result.Data = OpResult.Data;
                    return result;
                }
                _logger.LogWarning($"No se pudo guardar la sesion correctamente: {OpResult.Message}");
                result.Success = false;
                result.Message = OpResult.Message;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crear sesion: {ex.Message}");
                result.Success = false;
                result.Message = ($"Error al Iniciar: {ex.Message}");
            }
            return result;            
        }
        public async Task<ServiceResult> CloseSesionAsync(CloseSesionDto closeSesionDto, int? idsesion = null)
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando con el Cierre de la Sesion.");

            try
            {
                _logger.LogInformation($"Iniciando con la obtencion de la Sesion con ID {closeSesionDto.Id} activa para cerrar.");
                var SesionExist = await _sesionRepository.GetByIdAsync(closeSesionDto.Id);
                if (!SesionExist.Success)
                {
                    _logger.LogWarning($"No se pudo obtener la sesion mediante el ID {closeSesionDto.Id}: {SesionExist.Message}");
                    result.Success = false;
                    result.Message = SesionExist.Message;
                    return result;
                }

                _logger.LogInformation("Sesion obtenida correctamente, iniciando con la actualizacion de la sesion.");
                SesionExist.Data.FechaFin = DateTime.Now;
                SesionExist.Data.Estado = false;

                var OpResult = await _sesionRepository.UpdateAsync(SesionExist.Data, idsesion);
                if (!OpResult.Success)
                {
                    _logger.LogWarning($"No se pudo actualizar la sesion correctamente: {OpResult.Message}");
                    result.Success = false;
                    result.Message = OpResult.Message;
                    return result;
                }

                _logger.LogInformation("Sesion cerrada correctamente.");
                result.Success = true;
                result.Message = OpResult.Message;
                result.Data = OpResult.Data;
            }
            catch(Exception ex) 
            {
                _logger.LogError($"Error al cerrar sesion: {ex.Message}");
                result.Success = false;
                result.Message = ($"Error al Cerrar Sesion: {ex.Message}");
            }
            return result;
        }
        public async Task<ServiceResult> GetSesionAsync()
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation($"Iniciando con la Obtencion de todas las Sesiones.");

            try
            {
                _logger.LogInformation("Iniciando el metodo de obtencion.");
                var OpResult = await _sesionRepository.GetAllAsync();
                if (!OpResult.Success)
                {
                    _logger.LogWarning($"No se pudo obetener las sesiones: {OpResult.Message}");
                    result.Success = false;
                    result.Message = OpResult.Message;
                    return result;
                }
                
                _logger.LogInformation($"Sesiones obtenidas correctamente.");
                result.Success = true;
                result.Data = OpResult.Data;
                result.Message = "Sesiones obtenidas correctamente";

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error con la obtencion: {ex.Message}");
                result.Success = false;
                result.Message = ($"Error: {ex.Message}");
            }
            return result;
        }
        public async Task<ServiceResult> GetSesionByUsersAsync(string correo)
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation($"Iniciando la obtencion de sesiones del usuario: {correo}");           

            try
            {
                _logger.LogInformation($"Iniciando obtencion del Usuario por el correo: {correo}");
                var OpResult = await _usuarioRepository.GetByCorreoAsync(correo);
                if (!OpResult.Success)
                {
                    _logger.LogWarning($"No se pudo obtener el usuario: {OpResult.Message}");
                    result.Success = false;
                    result.Message = OpResult.Message;
                    return result;
                }

                _logger.LogInformation($"Iniciando obtencion de las sesiones por el id {OpResult.Data.Id} del usuario");
                var Sesions = await _sesionRepository.GetAllByAsync(s => s.IdUsuario == OpResult.Data.Id);
                if (!Sesions.Success)
                {
                    _logger.LogWarning($"No se pudo obtener las sesiones: {Sesions.Message}");
                    result.Success = false;
                    result.Message = Sesions.Message;
                    return result;
                }

                _logger.LogInformation($"Lista de sesiones del usuario {OpResult.Data.Correo} obtenida correctamente.");
                result.Success = true;
                result.Data = Sesions.Data;
                result.Message= ($"Lista de sesiones del usuario {OpResult.Data.Correo} obtenida correctamente.");

            }
            catch (Exception ex) 
            {
                _logger.LogError($"Error al obtener: {ex.Message}");
                result.Success = false;
                result.Message = ($"Error al obtener: {ex.Message}");
            }
            return result;
        }
        public async Task<ServiceResult> GetOpenSesionAsync()
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation($"Iniciando con la obtencion del la Lista de sesiones activas.");

            try
            {
                _logger.LogInformation("Obteniendo Lista de sesiones activas.");
                var OpResult = await _sesionRepository.GetAllByAsync(s => s.Estado == true);
                if (!OpResult.Success)
                {
                    _logger.LogWarning($"No se lograron obtener las sesiones activas: {OpResult.Message}");
                    result.Success = false;
                    result.Message = OpResult.Message;
                    return result;
                }

                _logger.LogInformation("Lista de Sesiones activas obtenidas correctamente.");
                result.Success = true;
                result.Message = "Lista de Sesiones activas obtenidas correctamente.";
                result.Data = OpResult.Data;
            }
            catch(Exception ex)
            {
                _logger.LogError($"Error al Obtener las sesiones activas: {ex.Message}");
                result.Success = false;
                result.Message = ($"Error al obetener las sesiones activas: {ex.Message}");
            }
            return result;
        }

        public async Task<ServiceResult> DeleteAsync(int id, int? idsesion = null)
        {
            ServiceResult result = new ServiceResult();
            try
            {
                var SesionResult = await _sesionRepository.GetByIdAsync(id);
                if (!SesionResult.Success)
                {
                    result.Success = false;
                    result.Message = SesionResult.Message;
                    return result;
                }

                var opResult = await _sesionRepository.DeleteAsync(SesionResult.Data, idsesion);
                if(!opResult.Success)
                {
                    result.Success = false;
                    result.Message = opResult.Message;
                    return result;
                }

                result.Success = true;
                result.Message = "Sesion eliminada correctamente.";
                
            }catch(Exception ex)
            {
                result.Success = false;
                result.Message = ($"Error al tratar de eliminal la sesion: {ex.Message}");
            }
            return result;
        }

        public async Task<ServiceResult> GetByIdAsync(int id)
        {
            ServiceResult result = new ServiceResult();
            try
            {
                var SesionResult = await _sesionRepository.GetByIdAsync(id);
                if (!SesionResult.Success)
                {
                    result.Success = false;
                    result.Message= SesionResult.Message;
                    return result;
                }

                result.Success = true;
                result.Message = ($"Sesion con ID {id} obtenida correcntamente.");
                result.Data = SesionResult.Data;

            }catch(Exception ex)
            {
                result.Success = false;
                result.Message = ($"Error al tratar de obtener la sesion por id: {ex.Message}");
            }
            return result;
        }
    }
}
