using Microsoft.Extensions.Logging;
using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Users.Cliente;
using SGHR.Application.Interfaces.Users;
using SGHR.Domain.Entities.Configuration.Usuers;
using SGHR.Persistence.Interfaces.Users;

namespace SGHR.Application.Services.Users
{
    public class ClienteService : IClienteService
    {
        public readonly ILogger<ClienteService> _logger;
        public readonly IClienteRepository _clienteRepository;

        public ClienteService(IClienteRepository clienteRepository,
                              ILogger<ClienteService> logger)
        {
            _clienteRepository = clienteRepository;
            _logger = logger;
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

            }catch (Exception ex)
            {
                _logger.LogError("Error al obtener al cliente: {ERROR}", ex.Message);
                result.Success = false;
                result.Message = ($"Error: {ex.Message}");
            }

            return result;
        }
        public async Task<ServiceResult> CreateAsync(CreateClienteDto CreateDto, int? sesionId = null)
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando creación de nuevo cliente.", CreateDto);

            try
            {
                _logger.LogInformation("Creando el registro del usuario.");
                Cliente cliente = new Cliente
                {
                    IdUsuario = CreateDto.IdUsuario,
                    Nombre = CreateDto.Nombre,
                    Apellido = CreateDto.Apellido,
                    Cedula = CreateDto.Cedula,
                    Telefono = CreateDto.Telefono,
                    Direccion = CreateDto.Direccion
                };

                var opResult = await _clienteRepository.SaveAsync(cliente);
                if (!opResult.Success)
                {
                    result.Success = false;
                    result.Message = "Error al crear el cliente.";
                    return result;
                }

                result.Success = true;
                result.Data = opResult.Data;
                result.Message = "Cliente creado correctamente.";

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el cliente.");
                result.Success = false;
                result.Message = "Error al crear el cliente.";
            }
            return result;
        }
        public async Task<ServiceResult> UpdateAsync(UpdateClienteDto UpdateDto, int? sesionId = null)
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando actualización de cliente con ID: {Id}", UpdateDto.Id);

            try
            {
                _logger.LogInformation("Validando que exista el usuario a actualizar.");
                var existingClienteResult = await _clienteRepository.GetByIdAsync(UpdateDto.Id);
                if (!existingClienteResult.Success || existingClienteResult.Data == null)
                {
                    _logger.LogWarning("No se encontro al usuario: {error}", existingClienteResult.Message);
                    result.Success = false;
                    result.Message = existingClienteResult.Message;
                    return result;
                }
                _logger.LogInformation("Validacion completada, comenzando con la actualizacion de los datos del cliente.");
                Cliente clienteToUpdate = existingClienteResult.Data;

                clienteToUpdate.IdUsuario = UpdateDto.IdUsuario;
                clienteToUpdate.Nombre = UpdateDto.Nombre;
                clienteToUpdate.Apellido = UpdateDto.Apellido;
                clienteToUpdate.Cedula = UpdateDto.Cedula;
                clienteToUpdate.Telefono = UpdateDto.Telefono;
                clienteToUpdate.Direccion = UpdateDto.Direccion;

                _logger.LogInformation("Actualizando datos.");
                var opResult = await _clienteRepository.UpdateAsync(clienteToUpdate);
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
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el cliente.");
                result.Success = false;
                result.Message = "Error al actualizar el cliente.";
            }
            return result;
        }
        public async Task<ServiceResult> DeleteAsync(int id, int? sesionId = null)
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
                var clienteExists = await _clienteRepository.GetByIdAsync(id);
                if (!clienteExists.Success)
                {
                    result.Success = false;
                    result.Message = clienteExists.Message;
                    return result;
                }

                var opResult = await _clienteRepository.DeleteAsync(clienteExists.Data);
                if (!opResult.Success)
                {
                    result.Success = false;
                    result.Message = "Error al eliminar el cliente.";
                    return result;
                }
                result.Success = true;
                result.Message = "Cliente eliminado correctamente.";

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el cliente.");
                result.Success = false;
                result.Message = "Error al eliminar el cliente.";
            }
            return result;
        }
        public async Task<ServiceResult> GetByIdAsync(int id)
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando obtención de cliente por ID: {Id}", id);

            try
            {
                var opResult = await _clienteRepository.GetByIdAsync(id);
                if (!opResult.Success)
                {
                    result.Success = false;
                    result.Message = opResult.Message;
                    return result;
                }
                result.Success = true;
                result.Data = opResult.Data;
                result.Message = "Cliente obtenido correctamente.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el cliente.");
                result.Success = false;
                result.Message = "Error al obtener el cliente.";
            }
            return result;
        }
        public async Task<ServiceResult> GetAllAsync()
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando obtención de todos los clientes.");

            try
            {
                var opResult = await _clienteRepository.GetAllAsync();
                if (!opResult.Success)
                {
                    result.Success = false;
                    result.Message = opResult.Message;
                    return result;
                }
                result.Success = true;
                result.Data = opResult.Data;
                result.Message = "Clientes obtenidos correctamente.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los clientes.");
                result.Success = false;
                result.Message = "Error al obtener los clientes.";
            }
            return result;
        }
    }
}
