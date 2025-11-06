
using Microsoft.Extensions.Logging;
using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Users.Cliente;
using SGHR.Application.Interfaces.Usuarios;
using SGHR.Application.ValidatorServices.Usuarios;
using SGHR.Domain.Entities.Configuration.Usuers;
using SGHR.Domain.Repository;
using SGHR.Persistence.Interfaces.Users;

namespace SGHR.Application.Services.Usuarios
{
    public class ClienteServices : IClienteServices
    {
        public readonly ILogger<ClienteServices> _logger;
        public readonly IClienteRepository _clienteRepository;
        public readonly IUsuarioRepository _usuarioRepository;

        public ClienteServices(ILogger<ClienteServices> logger, 
                               IClienteRepository clienteRepository,
                               IUsuarioRepository usuarioRepository)
        {
            _logger = logger;
            _clienteRepository = clienteRepository;           
            _usuarioRepository = usuarioRepository;
        }

        public async Task<ServiceResult> CreateAsync(CreateClienteDto CreateDto)
        {
            ServiceResult result = new ServiceResult();
            try
            {
                var validator = new ClienteValidatorServices(_clienteRepository, _usuarioRepository).ValidateCreate(CreateDto, out string errorMessage);
                if (!validator)
                {
                    result.Message = errorMessage;
                    return result;
                }
                var cliente = new Cliente
                {
                    Nombre = CreateDto.Nombre,
                    Telefono = CreateDto.Telefono,
                    Direccion = CreateDto.Direccion,
                    Apellido = CreateDto.Apellido,
                    Cedula = CreateDto.Cedula,
                };

                if (!string.IsNullOrWhiteSpace(CreateDto.Correo))
                {
                    var Usuario = await _usuarioRepository.GetByCorreoAsync(CreateDto.Correo);
                    if (!Usuario.Success)
                    {
                        result.Message = Usuario.Message;
                        return result;
                    }
                    cliente.IdUsuario = Usuario.Data.Id;
                }

                var opResult = await _clienteRepository.SaveAsync(cliente);
                if (!opResult.Success)
                {
                    result.Message = opResult.Message;
                    return result;
                }

                ClienteDto clienteDto = new ClienteDto()
                {
                    Id = cliente.Id,
                    Nombre = opResult.Data.Nombre,
                    Apellido = opResult.Data.Apellido,
                    Cedula = opResult.Data.Cedula,
                    Correo = CreateDto.Correo,
                    Direccion = opResult.Data.Direccion,
                    Telefono = opResult.Data.Telefono
                };

                result.Success = true;
                result.Message = "Cliente creado exitosamente.";
                result.Data = clienteDto;


            }
            catch (Exception ex)
            {
                result.Message = $"Error al crear el cliente: {ex.Message}";
            }
            return result;
        }
        public async Task<ServiceResult> DeleteAsync(int id)
        {
            ServiceResult result = new ServiceResult();
            try
            {
                var validate = new ClienteValidatorServices(_clienteRepository, _usuarioRepository).ValidateDelete(id, out string errorMessage);
                if (!validate)
                {
                    result.Message = errorMessage;
                    return result;
                }
                var existCliente = await _clienteRepository.GetByIdAsync(id);

                var OpResult = await _clienteRepository.DeleteAsync(existCliente.Data);
                if (!OpResult.Success)
                {
                    result.Message = OpResult.Message;
                    return result;
                }
                result.Success = true;
                result.Message = $"Cliente {existCliente.Data.Nombre} eliminado correctamente.";

            }
            catch (Exception ex)
            {
                result.Message = $"Error al eliminar el cliente: {ex.Message}";
            }
            return result;
        }
        public async Task<ServiceResult> GetAllAsync()
        {
            ServiceResult result = new ServiceResult();

            try
            {
                var ListaClientes = await _clienteRepository.GetAllAsync();
                if (!ListaClientes.Success)
                {
                    result.Message = ListaClientes.Message;
                    return result;
                }
                var ListaUsuarios = await _usuarioRepository.GetAllAsync();

                var clientesDtos = (
                    from c in ListaClientes.Data
                    join u in ListaUsuarios.Data on c.IdUsuario equals u.Id into userGroup
                    from u in userGroup.DefaultIfEmpty() 
                    select new ClienteDto
                    {
                        Id = c.Id,
                        Nombre = c.Nombre,
                        Apellido = c.Apellido,
                        Cedula = c.Cedula,
                        Direccion = c.Direccion,
                        Telefono = c.Telefono,
                        Correo = u != null ? u.Correo : "Sin correo" 
                    }
                ).ToList();

                result.Success = true;
                result.Data = clientesDtos;
                result.Message = $"Se obtuvieron los clientes correctamnete.";



            }
            catch (Exception ex)
            {
                result.Message = $"Error al obtener los clientes: {ex.Message}";
            }
            return result;
        }
        public async Task<ServiceResult> GetByCedulaAsync(string cedula)
        {
            ServiceResult result = new ServiceResult();
            try
            {
                var vali = new ClienteValidatorServices(_clienteRepository, _usuarioRepository).ValidateGetByCedula(cedula, out string errorMessage);
                if (!vali)
                {
                    result.Message = errorMessage;
                    return result;
                }
                var OpResult = await _clienteRepository.GetByCedulaAsync(cedula);
                if (!OpResult.Success)
                {
                    result.Message = OpResult.Message;
                    return result;
                }

                ClienteDto clientesDtos = new ClienteDto()
                {
                    Id = OpResult.Data.Id,
                    Nombre = OpResult.Data.Nombre,
                    Apellido = OpResult.Data.Apellido,
                    Cedula = OpResult.Data.Cedula,
                    Direccion = OpResult.Data.Direccion,
                    Telefono = OpResult.Data.Telefono
                };


                if(OpResult.Data.IdUsuario != null)
                {
                    var UsuarioResult = await _usuarioRepository.GetByIdAsync(OpResult.Data.IdUsuario.Value);
                    if(!UsuarioResult.Success)
                    {
                        result.Message = UsuarioResult.Message;
                        return result;
                    }
                    if(UsuarioResult.Data.Correo != null)
                    {
                        clientesDtos.Correo = UsuarioResult.Data.Correo;
                    }
                    
                }
                
                result.Success = true;
                result.Data = clientesDtos;
                result.Message = $"Se obtuvo el cliente por correo correctamnete.";

            }
            catch (Exception ex)
            {
                result.Message = $"Error al obtener el cliente por id: {ex.Message}";
            }
            return result;
        }
        public async Task<ServiceResult> GetByIdAsync(int id)
        {
            ServiceResult result = new ServiceResult();
            try
            {
                var vali = new ClienteValidatorServices(_clienteRepository, _usuarioRepository).ValidateGetById(id, out string errorMessage);
                if (!vali)
                {
                    result.Message = errorMessage;
                    return result;
                }
                var opResult = await _clienteRepository.GetByIdAsync(id);
                if (!opResult.Success)
                {
                    result.Message = opResult.Message;
                    return result;
                }

                ClienteDto clientesDtos = new ClienteDto()
                {
                    Id = opResult.Data.Id,
                    Nombre = opResult.Data.Nombre,
                    Apellido = opResult.Data.Apellido,
                    Cedula = opResult.Data.Cedula,
                    Direccion = opResult.Data.Direccion,
                    Telefono = opResult.Data.Telefono
                };


                if (opResult.Data.IdUsuario != null)
                {
                    var UsuarioResult = await _usuarioRepository.GetByIdAsync(opResult.Data.IdUsuario.Value);
                    if (!UsuarioResult.Success)
                    {
                        result.Message = UsuarioResult.Message;
                        return result;
                    }
                    if (UsuarioResult.Data.Correo != null)
                    {
                        clientesDtos.Correo = UsuarioResult.Data.Correo;
                    }

                }

                result.Success = true;
                result.Data = clientesDtos;
                result.Message = $"Se obtuvo el usuario con id {id} correctamnete.";

            }
            catch (Exception ex)
            {
                result.Message = $"Error al obtener el cliente por id: {ex.Message}";
            }
            return result;
        }
        public async Task<ServiceResult> UpdateAsync(UpdateClienteDto UpdateDto)
        {
            ServiceResult result = new ServiceResult();
            try
            {
                var vali = new ClienteValidatorServices(_clienteRepository, _usuarioRepository).ValidateUpdate(UpdateDto, out string errorMessage);
                if (!vali)
                {
                    result.Message = errorMessage;
                    return result;
                }
                var cliente = await _clienteRepository.GetByIdAsync(UpdateDto.Id);
                if(!cliente.Success)
                {
                    result.Message = cliente.Message;
                    return result;
                }

                cliente.Data.Id = UpdateDto.Id;
                cliente.Data.Nombre = UpdateDto.Nombre;
                cliente.Data.Direccion = UpdateDto.Direccion;
                cliente.Data.Apellido = UpdateDto.Apellido;
                cliente.Data.Telefono = UpdateDto.Telefono;
                cliente.Data.Cedula = UpdateDto.Cedula;

                if (!string.IsNullOrWhiteSpace(UpdateDto.Correo))
                {
                    var Usuarios = await _usuarioRepository.GetByCorreoAsync(UpdateDto.Correo);
                    if (!Usuarios.Success)
                    {
                        result.Message = Usuarios.Message;
                        return result;
                    }

                    cliente.Data.IdUsuario = Usuarios.Data.Id;
                }

                var opResult = await _clienteRepository.UpdateAsync(cliente.Data);
                if (!opResult.Success)
                {
                    result.Message = opResult.Message;
                    return result;
                }

                ClienteDto clienteDto = new ClienteDto()
                {
                    Id = opResult.Data.Id,
                    Nombre = opResult.Data.Nombre,
                    Apellido = opResult.Data.Apellido,
                    Cedula = opResult.Data.Cedula,
                    Correo = UpdateDto.Correo,
                    Direccion = opResult.Data.Direccion,
                    Telefono = opResult.Data.Telefono
                };

                result.Success = true;
                result.Message = "Cliente actualizado exitosamente.";
                result.Data = clienteDto;

            }
            catch (Exception ex)
            {
                result.Message = "Ocurrio un Error al actualizar el cliente.";
            }
            return result;
        }
    }
}
