
using Microsoft.Extensions.Logging;
using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Users.Cliente;
using SGHR.Application.Interfaces.Usuarios;
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
            if (CreateDto == null)
            {
                result.Message = "El cliente no puede ser nulo.";
                return result;
            }

            try
            {

                // Verificar si ya existe un cliente con ese correo
                var LisClientes = await _clienteRepository.GetAllAsync();
                if (!LisClientes.Success)
                {
                    result.Message = LisClientes.Message;
                    return result;
                }
                
                var existCliente = LisClientes.Data.FirstOrDefault(c => c.Cedula == CreateDto.Cedula);
                if (existCliente != null)
                {
                    result.Message = "Ya existe un cliente registrado con esa cedula.";
                    return result;
                }

                // Crear la entidad Cliente
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
                    var Usuarios = await _usuarioRepository.GetAllAsync();
                    if (!Usuarios.Success)
                    {
                        result.Message = Usuarios.Message;
                        return result;
                    }

                    var verificarCorreos = Usuarios.Data.FirstOrDefault(u => u.Correo == CreateDto.Correo);
                    if (verificarCorreos == null)
                    {
                        result.Message = $"Correo no encontrado, tiene que registrar un correo existente.";
                        return result;
                    }

                    if(LisClientes.Data.Where(u => u.IdUsuario == verificarCorreos.Id).FirstOrDefault() != null)
                    {
                        result.Message = "Ya hay un cliente registrado con ese correo.";
                        return result;
                    }

                    cliente.IdUsuario = verificarCorreos.Id;
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
            if(id <= 0)
            {
                result.Message = "Tiene que introducir un id valido.";
                return result;
            }
            try
            {
                var existCliente = await _clienteRepository.GetByIdAsync(id);
                if (!existCliente.Success)
                {
                    result.Message = $"No existe un cliente con ese id.";
                    return result;
                }

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
            if (id <= 0)
            {
                result.Message = "Tiene que introducir un id valido.";
                return result;
            }
            try
            {
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
            if (UpdateDto == null)
            {
                result.Message = "El cliente no puede ser nulo.";
                return result;
            }
            if (UpdateDto.Id <= 0)
            {
                result.Message = "Ingrese un id valido.";
                return result;
            }
            try
            {

                var LisClientes = await _clienteRepository.GetAllAsync();
                if (!LisClientes.Success)
                {
                    result.Message = LisClientes.Message;
                    return result;
                }

                var existCliente = LisClientes.Data.FirstOrDefault(c => c.Cedula == UpdateDto.Cedula);
                if (existCliente != null)
                {
                    result.Message = "Ya existe un cliente registrado con esa cedula.";
                    return result;
                }

                var cliente = LisClientes.Data.FirstOrDefault(c => c.Id == UpdateDto.Id);

                cliente.Id = UpdateDto.Id;
                cliente.Nombre = UpdateDto.Nombre;
                cliente.Direccion = UpdateDto.Direccion;
                cliente.Apellido = UpdateDto.Apellido;
                cliente.Telefono = UpdateDto.Telefono;
                cliente.Cedula = UpdateDto.Cedula;

                if (!string.IsNullOrWhiteSpace(UpdateDto.Correo))
                {
                    var Usuarios = await _usuarioRepository.GetAllAsync();
                    if (!Usuarios.Success)
                    {
                        result.Message = Usuarios.Message;
                        return result;
                    }

                    var verificarCorreos = Usuarios.Data.FirstOrDefault(u => u.Correo == UpdateDto.Correo);
                    if (verificarCorreos == null)
                    {
                        result.Message = $"Correo no encontrado, tiene que registrar un correo existente.";
                        return result;
                    }

                    if (LisClientes.Data.Where(u => u.IdUsuario == verificarCorreos.Id).FirstOrDefault() != null)
                    {
                        result.Message = "Ya hay un cliente registrado con ese correo.";
                        return result;
                    }

                    cliente.IdUsuario = verificarCorreos.Id;
                }

                var opResult = await _clienteRepository.UpdateAsync(cliente);
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

            }
            return result;
        }
    }
}
