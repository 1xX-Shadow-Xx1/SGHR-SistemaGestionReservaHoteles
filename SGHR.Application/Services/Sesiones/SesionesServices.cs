using Microsoft.Extensions.Logging;
using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Operaciones.Auditory;
using SGHR.Application.Dtos.Configuration.Sesiones.Sesion;
using SGHR.Application.Interfaces.Operaciones;
using SGHR.Application.Interfaces.Sesiones;
using SGHR.Domain.Entities.Configuration.Operaciones;
using SGHR.Domain.Entities.Configuration.Sesiones;
using SGHR.Domain.Repository;
using SGHR.Persistence.Interfaces.Operaciones;
using SGHR.Persistence.Interfaces.Sesiones;


namespace SGHR.Application.Services.Sesiones
{
    public class SesionesServices : ISesionServices
    {
        public readonly ILogger<SesionesServices> _logger;
        public readonly ISesionRepository _sesionRepository;
        public readonly IUsuarioRepository _usuarioRepository;
        public readonly IAuditoryService _auditoryService;

        public SesionesServices(ILogger<SesionesServices> logger,
                                ISesionRepository sesionRepository,
                                IUsuarioRepository usuarioRepository,
                                IAuditoryService auditoryService
                                )
        {            
            _logger = logger;
            _sesionRepository = sesionRepository;
            _auditoryService = auditoryService;
        }

        public async Task<ServiceResult> OpenSesion(int id)
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation($"Iniciando con el proceso de abrir sesion del usuario con ID: {id}.");

            try
            {
                _logger.LogInformation("Creando la sesion");
                Sesion sesion  = new Sesion
                {
                    IdUsuario = id,
                    FechaInicio = DateTime.Now,
                    Estado = true
                };

                _logger.LogInformation("Guardando la sesion en la Base de datos");
                var OpResult = await _sesionRepository.Save(sesion);
                if (OpResult.Success)
                {
                    _logger.LogInformation("Sesion guardad correctamente en la base de datos.");
                    result.Success = true;
                    result.Message = OpResult.Message;
                    result.Data = OpResult.Data;
                    SesionDto.SesionID = OpResult.Data.ID;
                    SesionDto.UsuarioID = OpResult.Data.IdUsuario;
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
        public async Task<ServiceResult> CloseSesion()
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando con el Cierre de la Sesion.");

            try
            {
                _logger.LogInformation($"Iniciando con la obtencion de la Sesion con ID {SesionDto.SesionID} activa para cerrar.");
                var SesionExist = await _sesionRepository.GetById(SesionDto.SesionID);
                if (!SesionExist.Success)
                {
                    _logger.LogWarning($"No se pudo obtener la sesion mediante el ID {SesionDto.SesionID}: {SesionExist.Message}");
                    result.Success = false;
                    result.Message = SesionExist.Message;
                    return result;
                }

                _logger.LogInformation("Sesion obtenida correctamente, iniciando con la actualizacion de la sesion.");
                SesionExist.Data.FechaFin = DateTime.Now;
                SesionExist.Data.Estado = false;

                var OpResult = await _sesionRepository.Update(SesionExist.Data);
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
        public async Task<ServiceResult> GetSesion()
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation($"Iniciando con la Obtencion de todas las Sesiones.");

            try
            {
                _logger.LogInformation("Iniciando el metodo de obtencion.");
                var OpResult = await _sesionRepository.GetAll();
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

                var auditory = new CreateAuditoryDto
                {
                    IdUsuario = SesionDto.UsuarioID,
                    Operacion = "GetAll-Sesion",
                    Detalle = ($"El usuario {SesionDto.UsuarioName} obtuvo una lista de todas las sesiones.")
                };

                var AudiResult = await _auditoryService.Save(auditory);
                if (!AudiResult.Success)
                {
                    return AudiResult;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error con la obtencion: {ex.Message}");
                result.Success = false;
                result.Message = ($"Error: {ex.Message}");
            }
            return result;
        }
        public async Task<ServiceResult> GetSesionByUsers(string correo)
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

                _logger.LogInformation($"Iniciando obtencion de las sesiones por el id {OpResult.Data.ID} del usuario");
                var Sesions = await _sesionRepository.GetAllBY(s => s.IdUsuario == OpResult.Data.ID);
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

                var auditory = new CreateAuditoryDto
                {
                    IdUsuario = SesionDto.UsuarioID,
                    Operacion = "GetAll-SesionByUsers",
                    Detalle = ($"El usuario {SesionDto.UsuarioName} obtuvo una lista de las sesiones del usuario {correo}.")
                };
                var AudiResult = await _auditoryService.Save(auditory);
                if (!AudiResult.Success)
                {
                    return AudiResult;
                }

            }
            catch (Exception ex) 
            {
                _logger.LogError($"Error al obtener: {ex.Message}");
                result.Success = false;
                result.Message = ($"Error al obtener: {ex.Message}");
            }
            return result;
        }
        public async Task<ServiceResult> GetOpenSesion()
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation($"Iniciando con la obtencion del la Lista de sesiones activas.");

            try
            {
                _logger.LogInformation("Obteniendo Lista de sesiones activas.");
                var OpResult = await _sesionRepository.GetAllBY(s => s.Estado == true);
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

                var auditory = new CreateAuditoryDto
                {
                    IdUsuario = SesionDto.UsuarioID,
                    Operacion = "GetAll-OpenSesion",
                    Detalle = ($"El usuario {SesionDto.UsuarioName} obtuvo una lista de las sesiones que estan activas en el momento.")
                };
                var AudiResult = await _auditoryService.Save(auditory);
                if (!AudiResult.Success)
                {
                    return AudiResult;
                }
            }
            catch(Exception ex)
            {
                _logger.LogError($"Error al Obtener las sesiones activas: {ex.Message}");
                result.Success = false;
                result.Message = ($"Error al obetener las sesiones activas: {ex.Message}");
            }
            return result;
        }
    }
}
