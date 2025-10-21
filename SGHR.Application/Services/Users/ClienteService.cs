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

        public async Task<ServiceResult> GetAll()
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando obtención de todos los clientes.");

            try
            {
                var opResult = await _clienteRepository.GetAll();
                if (opResult.Success)
                {
                    result.Success = true;
                    result.Data = opResult.Data;
                    result.Message = "Clientes obtenidos correctamente.";
                }
                else
                {
                    result.Success = false;
                    result.Message = opResult.Message;
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
                if (opResult.Success)
                {
                    result.Success = true;
                    result.Data = opResult.Data;
                    result.Message = "Cliente obtenido correctamente.";
                }
                else
                {
                    result.Success = false;
                    result.Message = opResult.Message;
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
                if (id <= 0)
                {
                    result.Success = false;
                    result.Message = "El ID del cliente no es válido.";
                    return result;
                }

                var clienteExists = await _clienteRepository.GetById(id);
                if (!clienteExists.Success)
                {
                    result.Success = false;
                    result.Message = clienteExists.Message;
                    return result;
                }

                var opResult = await _clienteRepository.Delete(clienteExists.Data);
                if (opResult.Success)
                {
                    result.Success = true;
                    result.Message = "Cliente eliminado correctamente.";
                }
                else
                {
                    result.Success = false;
                    result.Message = "Error al eliminar el cliente.";
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
                Cliente cliente = new Cliente
                {
                    IdUsuario = createClienteDto.IdUsuario,
                    Nombre = createClienteDto.Nombre,
                    Apellido = createClienteDto.Apellido,
                    Cedula = createClienteDto.Cedula,
                    Telefono = createClienteDto.Telefono,
                    Direccion = createClienteDto.Direccion
                };

                var opResult = await _clienteRepository.Save(cliente);
                if (opResult.Success)
                {
                    result.Success = true;
                    result.Data = opResult.Data;
                    result.Message = "Cliente creado correctamente.";
                }
                else
                {
                    result.Success = false;
                    result.Message = "Error al crear el cliente.";
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
                var existingClienteResult = await _clienteRepository.GetById(updateClienteDto.Id);
                if (!existingClienteResult.Success || existingClienteResult.Data == null)
                {
                    result.Success = false;
                    result.Message = existingClienteResult.Message;
                    return result;
                }

                Cliente clienteToUpdate = existingClienteResult.Data;

                clienteToUpdate.IdUsuario = updateClienteDto.IdUsuario;
                clienteToUpdate.Nombre = updateClienteDto.Nombre;
                clienteToUpdate.Apellido = updateClienteDto.Apellido;
                clienteToUpdate.Cedula = updateClienteDto.Cedula;
                clienteToUpdate.Telefono = updateClienteDto.Telefono;
                clienteToUpdate.Direccion = updateClienteDto.Direccion;


                var opResult = await _clienteRepository.Update(clienteToUpdate);
                if (opResult.Success)
                {
                    result.Success = true;
                    result.Data = opResult.Data;
                    result.Message = "Cliente actualizado correctamente.";
                }
                else
                {
                    result.Success = false;
                    result.Message = "Error al actualizar el cliente.";
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
    }
}
