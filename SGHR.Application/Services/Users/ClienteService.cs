using Microsoft.Extensions.Logging;
using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Operaciones.Auditory;
using SGHR.Application.Dtos.Configuration.Sesiones.Sesion;
using SGHR.Application.Dtos.Configuration.Users.Cliente;
using SGHR.Application.Interfaces.Operaciones;
using SGHR.Application.Interfaces.Users;
using SGHR.Domain.Entities.Configuration.Usuers;
using SGHR.Persistence.Interfaces.Users;

namespace SGHR.Application.Services.Users
{
    public class ClienteService : IClienteService
    {
        public readonly ILogger<ClienteService> _logger;
        public readonly IClienteRepository _clienteRepository;
        public readonly IAuditoryService _auditoryService;

        public ClienteService(IClienteRepository clienteRepository,
                              IAuditoryService auditoryService,
                              ILogger<ClienteService> logger)
        {
            _clienteRepository = clienteRepository;
            _logger = logger;
            _auditoryService = auditoryService;
        }

        public async Task<ServiceResult> GetAll()
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando obtención de todos los clientes.");

            try
            {
                var opResult = await _clienteRepository.GetAll();
                if (!opResult.Success)
                {
                    result.Success = false;
                    result.Message = opResult.Message;
                    return result;
                }
                result.Success = true;
                result.Data = opResult.Data;
                result.Message = "Clientes obtenidos correctamente.";

                var auditory = new CreateAuditoryDto
                {
                    IdUsuario = SesionDto.UsuarioID,
                    Operacion = "GetAll-Clientes",
                    Detalle = ($"El usuario {SesionDto.UsuarioName} obtuvo una lista de todos los clientes.")
                };
                var AudiResult = await _auditoryService.Save(auditory);
                if (!AudiResult.Success)
                {
                    return AudiResult;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los clientes.");
                result.Success = false;
                result.Message = "Error al obtener los clientes.";
            }
            return result;
        }
        public async Task<ServiceResult> GetById(int id)
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando obtención de cliente por ID: {Id}", id);

            try
            {
                var opResult = await _clienteRepository.GetById(id);
                if (!opResult.Success)
                {
                    result.Success = false;
                    result.Message = opResult.Message;
                    return result;
                }
                result.Success = true;
                result.Data = opResult.Data;
                result.Message = "Cliente obtenido correctamente.";

                var auditory = new CreateAuditoryDto
                {
                    IdUsuario = SesionDto.UsuarioID,
                    Operacion = "GetByID-Cliente",
                    Detalle = ($"El usuario {SesionDto.UsuarioName} obtuvo al cliente con ID {id} correctamente.")
                };
                var OpResult = await _auditoryService.Save(auditory);
                if (!OpResult.Success)
                {
                    return OpResult;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el cliente.");
                result.Success = false;
                result.Message = "Error al obtener el cliente.";
            }
            return result;
        }
        public async Task<ServiceResult> Remove(int id)
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando eliminación de cliente con ID: {Id}", id);

            try
            {
                _logger.LogInformation("Validando que el id sea valido.");
                if (id <= 0)
                {
                    _logger.LogWarning("El id no es valido, tiene que ingresar un id valido.");
                    result.Success = false;
                    result.Message = "El ID del cliente no es válido.";
                    return result;
                }

                _logger.LogInformation("Validacion completada, obteniendo cliente por id.");
                var clienteExists = await _clienteRepository.GetById(id);
                if (!clienteExists.Success)
                {
                    result.Success = false;
                    result.Message = clienteExists.Message;
                    return result;
                }

                var opResult = await _clienteRepository.Delete(clienteExists.Data);
                if (!opResult.Success)
                {
                    result.Success = false;
                    result.Message = "Error al eliminar el cliente.";
                    return result;
                }
                result.Success = true;
                result.Message = "Cliente eliminado correctamente.";

                var auditory = new CreateAuditoryDto
                {
                    IdUsuario = SesionDto.UsuarioID,
                    Operacion = "Delete-Cliente",
                    Detalle = ($"El usuario {SesionDto.UsuarioName} elimino al cliente con ID {id} correctamente.")
                };
                var OpResult = await _auditoryService.Save(auditory);
                if (!OpResult.Success)
                {
                    return OpResult;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el cliente.");
                result.Success = false;
                result.Message = "Error al eliminar el cliente.";
            }
            return result;
        }
        public async Task<ServiceResult> Save(CreateClienteDto createClienteDto)
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando creación de nuevo cliente.", createClienteDto);

            try
            {
                _logger.LogInformation("Creando el registro del usuario.");
                Cliente cliente = new Cliente
                {
                    IdUsuario = SesionDto.UsuarioID,
                    Nombre = createClienteDto.Nombre,
                    Apellido = createClienteDto.Apellido,
                    Cedula = createClienteDto.Cedula,
                    Telefono = createClienteDto.Telefono,
                    Direccion = createClienteDto.Direccion
                };

                var opResult = await _clienteRepository.Save(cliente);
                if (!opResult.Success)
                {
                    result.Success = false;
                    result.Message = "Error al crear el cliente.";
                    return result;
                }

                result.Success = true;
                result.Data = opResult.Data;
                result.Message = "Cliente creado correctamente.";

                var auditory = new CreateAuditoryDto
                {
                    IdUsuario = SesionDto.UsuarioID,
                    Operacion = "Registrar-Cliente",
                    Detalle = ($"El usuario {SesionDto.UsuarioName} registro un nuevo cliente.")
                };
                var OpResult = await _auditoryService.Save(auditory);
                if (!OpResult.Success)
                {
                    return OpResult;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el cliente.");
                result.Success = false;
                result.Message = "Error al crear el cliente.";
            }
            return result;
        }
        public async Task<ServiceResult> Update(UpdateClienteDto updateClienteDto)
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando actualización de cliente con ID: {Id}", updateClienteDto.Id);

            try
            {
                _logger.LogInformation("Validando que exista el usuario a actualizar.");
                var existingClienteResult = await _clienteRepository.GetById(updateClienteDto.Id);
                if (!existingClienteResult.Success || existingClienteResult.Data == null)
                {
                    _logger.LogWarning("No se encontro al usuario: {error}", existingClienteResult.Message);
                    result.Success = false;
                    result.Message = existingClienteResult.Message;
                    return result;
                }
                _logger.LogInformation("Validacion completada, comenzando con la actualizacion de los datos del cliente.");
                Cliente clienteToUpdate = existingClienteResult.Data;

                clienteToUpdate.IdUsuario = updateClienteDto.IdUsuario;
                clienteToUpdate.Nombre = updateClienteDto.Nombre;
                clienteToUpdate.Apellido = updateClienteDto.Apellido;
                clienteToUpdate.Cedula = updateClienteDto.Cedula;
                clienteToUpdate.Telefono = updateClienteDto.Telefono;
                clienteToUpdate.Direccion = updateClienteDto.Direccion;

                _logger.LogInformation("Actualizando datos.");
                var opResult = await _clienteRepository.Update(clienteToUpdate);
                if (!opResult.Success)
                {
                    result.Success = false;
                    result.Message = "Error al actualizar el cliente.";
                    return result;
                }

                _logger.LogInformation("Datos actualizados correctamente.");
                result.Success = true;
                result.Data = opResult.Data;
                result.Message = "Cliente actualizado correctamente.";

                var auditory = new CreateAuditoryDto
                {
                    IdUsuario = SesionDto.UsuarioID,
                    Operacion = "Delete-Cliente",
                    Detalle = ($"El usuario {SesionDto.UsuarioName} registro un nuevo cliente.")
                };
                var OpResult = await _auditoryService.Save(auditory);
                if (!OpResult.Success)
                {
                    return OpResult;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el cliente.");
                result.Success = false;
                result.Message = "Error al actualizar el cliente.";
            }
            return result;
        }
        public async Task<ServiceResult> GetByCedulaAsync(string cedula)
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando obtencion de los datos del cliente mediante su cedula.");

            try
            {
                var OpResult = await _clienteRepository.GetByCedulaAsync(cedula);
                if (!OpResult.Success)
                {
                    result.Success = false;
                    result.Message = OpResult.Message;
                    return result;
                }
                result.Success = true;
                result.Message = "Cliente obtenido correctamente.";
                result.Data = OpResult.Data;
                _logger.LogInformation("Cliente obtenido por cedula, obtenido correctamente.");

                var auditory = new CreateAuditoryDto
                {
                    IdUsuario = SesionDto.UsuarioID,
                    Operacion = "GetBy-Cliente-Cedula",
                    Detalle = ($"EL usuario {SesionDto.UsuarioName} obtuvo al cliente mediante el correo .")
                };

                var AudiTory = await _auditoryService.Save(auditory);
                if (!AudiTory.Success)
                {
                    return AudiTory;
                }

            }catch (Exception ex)
            {
                _logger.LogError("Error al obtener al cliente: {ERROR}", ex.Message);
                result.Success = false;
                result.Message = ($"Error: {ex.Message}");
            }

            return result;
        }
    }
}
