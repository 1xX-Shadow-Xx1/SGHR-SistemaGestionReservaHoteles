using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SGHR.Application.Dtos.Configuration.Users.Cliente;
using SGHR.Application.Services.Usuarios;
using SGHR.Domain.Entities.Configuration.Usuers;
using SGHR.Domain.Enum.Usuarios;
using SGHR.Domain.Validators.ConfigurationRules.Users;
using SGHR.Persistence.Context;
using SGHR.Persistence.Repositories.EF.Users;

namespace SGHR.Application.Test.UsuariosTest
{
    public class ClienteServicesTest
    {
        private readonly ClienteRepository _clienteRepository;
        private readonly UsuarioRepository _usuarioRepository;
        private readonly SGHRContext _context;
        private readonly ILogger<ClienteRepository> _loggerClienteRepo;
        private readonly ILogger<UsuarioRepository> _loggerUsuarioRepo;
        private readonly ClienteValidator _clienteValidator;
        private readonly UsuarioValidator _usuarioValidator;
        private readonly IConfiguration _configuration;
        private readonly ClienteServices _clienteServices;
        private readonly ILogger<ClienteServices> _loggerServices;

        public ClienteServicesTest()
        {
            _clienteValidator = new ClienteValidator();
            _usuarioValidator = new UsuarioValidator();

            var options = new DbContextOptionsBuilder<SGHRContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new SGHRContext(options);

            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
            });

            _loggerClienteRepo = loggerFactory.CreateLogger<ClienteRepository>();
            _loggerUsuarioRepo = loggerFactory.CreateLogger<UsuarioRepository>();
            _loggerServices = loggerFactory.CreateLogger<ClienteServices>();
            _configuration = new ConfigurationBuilder().Build();

            _clienteRepository = new ClienteRepository(
                _context,
                _clienteValidator,
                _loggerClienteRepo
            );

            _usuarioRepository = new UsuarioRepository(
                _context,
                _usuarioValidator,
                _loggerUsuarioRepo
            );

            _clienteServices = new ClienteServices(
                _loggerServices,
                _clienteRepository,
                _usuarioRepository
            );
        }

        // -------------------- CREATE TESTS --------------------

        [Fact]
        public async Task When_Cliente_is_null_CreateAsync_Should_Fail()
        {
            //Arrange
            CreateClienteDto cliente = null;

            //Act
            var result = await _clienteServices.CreateAsync(cliente);

            //Assert
            Assert.False(result.Success);
            Assert.Equal("El cliente no puede ser nulo.", result.Message);
        }

        [Fact]
        public async Task When_Cedula_already_exists_CreateAsync_Should_Fail()
        {
            //Arrange
            var cliente = new Cliente { Nombre = "Juan", Apellido = "Lopez", Cedula = "123-1234567-1" };
            await _clienteRepository.SaveAsync(cliente);

            var dto = new CreateClienteDto
            {
                Nombre = "Pedro",
                Apellido = "Perez",
                Cedula = "123-1234567-1"
            };

            //Act
            var result = await _clienteServices.CreateAsync(dto);

            //Assert
            Assert.False(result.Success);
            Assert.Equal("Ya existe un cliente registrado con esa cedula.", result.Message);
        }

        [Fact]
        public async Task When_Correo_no_exists_in_usuarios_CreateAsync_Should_Fail()
        {
            //Arrange
            var dto = new CreateClienteDto
            {
                Nombre = "Maria",
                Apellido = "Gomez",
                Cedula = "555",
                Correo = "noexiste@test.com"
            };

            //Act
            var result = await _clienteServices.CreateAsync(dto);

            //Assert
            Assert.False(result.Success);
            Assert.Equal("Usuario no encontrado.", result.Message);
        }

        [Fact]
        public async Task When_Cliente_is_valid_CreateAsync_Should_Succeed()
        {
            //Arrange
            var usuario = new Usuario { Nombre = "Usuario", Correo = "user@test.com", Contraseña = "123456789", Rol = RolUsuarios.Cliente };
            var re  = await _usuarioRepository.SaveAsync(usuario);

            var dto = new CreateClienteDto
            {
                Nombre = "Maria",
                Apellido = "Gomez",
                Cedula = "555-5555555-5",
                Correo = "user@test.com"
            };

            //Act
            var result = await _clienteServices.CreateAsync(dto);

            //Assert
            Assert.Equal("Cliente creado exitosamente.", result.Message);
            Assert.True(result.Success);
            
        }

        // -------------------- DELETE TESTS --------------------

        [Fact]
        public async Task When_Id_is_invalid_DeleteAsync_Should_Fail()
        {
            //Arrange
            int id = 0;

            //Act
            var result = await _clienteServices.DeleteAsync(id);

            //Assert
            Assert.False(result.Success);
            Assert.Equal("El id ingresado no es valido.", result.Message);
        }

        [Fact]
        public async Task When_Cliente_does_not_exist_DeleteAsync_Should_Fail()
        {
            //Arrange
            int id = 99;

            //Act
            var result = await _clienteServices.DeleteAsync(id);

            //Assert
            Assert.False(result.Success);
            Assert.Equal("No existe un cliente con ese id.", result.Message);
        }

        [Fact]
        public async Task When_Cliente_exists_DeleteAsync_Should_Succeed()
        {
            //Arrange
            var cliente = new Cliente { Nombre = "Carlos", Apellido = "Lopez", Cedula = "111-1111111-1" };
            await _clienteRepository.SaveAsync(cliente);

            //Act
            var result = await _clienteServices.DeleteAsync(cliente.Id);

            //Assert
            Assert.True(result.Success);
            Assert.Contains("eliminado correctamente", result.Message);
        }

        // -------------------- GET ALL TESTS --------------------

        [Fact]
        public async Task When_No_clientes_GetAllAsync_Should_Return_Empty()
        {
            //Arrange

            //Act
            var result = await _clienteServices.GetAllAsync();

            //Assert
            Assert.True(result.Success);
            var lista = result.Data as List<ClienteDto>;
            Assert.Empty(lista);
        }

        [Fact]
        public async Task When_Clientes_exist_GetAllAsync_Should_Succeed()
        {
            //Arrange
            var usuario = new Usuario { Nombre = "Usuario", Correo = "test@test.com", Contraseña = "123", Rol = RolUsuarios.Cliente };
            await _usuarioRepository.SaveAsync(usuario);

            var cliente = new Cliente { Nombre = "Juan", Apellido = "Perez", Cedula = "111", IdUsuario = usuario.Id };
            await _clienteRepository.SaveAsync(cliente);

            //Act
            var result = await _clienteServices.GetAllAsync();

            //Assert
            Assert.True(result.Success);
            Assert.Equal("Se obtuvieron los clientes correctamnete.", result.Message);
        }

        // -------------------- GET BY ID TESTS --------------------

        [Fact]
        public async Task When_Id_is_invalid_GetByIdAsync_Should_Fail()
        {
            //Arrange
            int id = 0;

            //Act
            var result = await _clienteServices.GetByIdAsync(id);

            //Assert
            Assert.False(result.Success);
            Assert.Equal("El id ingresado no es valido.", result.Message);
        }

        [Fact]
        public async Task When_Cliente_not_found_GetByIdAsync_Should_Fail()
        {
            //Arrange
            int id = 999;

            //Act
            var result = await _clienteServices.GetByIdAsync(id);

            //Assert
            Assert.False(result.Success);
            Assert.Equal("No existe un cliente con ese id.", result.Message);
        }

        [Fact]
        public async Task When_Cliente_exists_GetByIdAsync_Should_Succeed()
        {
            //Arrange
            var usuario = new Usuario { Nombre = "Luis", Correo = "luis@test.com", Contraseña = "123123456789", Rol = RolUsuarios.Cliente };
            await _usuarioRepository.SaveAsync(usuario);

            var cliente = new Cliente { Nombre = "Mario", Apellido = "Diaz", Cedula = "123-1234567-8", IdUsuario = usuario.Id };
            await _clienteRepository.SaveAsync(cliente);

            //Act
            var result = await _clienteServices.GetByIdAsync(cliente.Id);

            //Assert
            Assert.True(result.Success);
            Assert.Contains("Se obtuvo el usuario con id", result.Message);
        }

        // -------------------- GET BY CEDULA TESTS --------------------

        [Fact]
        public async Task When_Cedula_not_found_GetByCedulaAsync_Should_Fail()
        {
            //Arrange
            string cedula = "999";

            //Act
            var result = await _clienteServices.GetByCedulaAsync(cedula);

            //Assert
            Assert.False(result.Success);
        }

        [Fact]
        public async Task When_Cedula_exists_GetByCedulaAsync_Should_Succeed()
        {
            //Arrange
            var usuario = new Usuario { Nombre = "Jose", Correo = "jose@test.com", Contraseña = "123456789", Rol = RolUsuarios.Cliente };
            await _usuarioRepository.SaveAsync(usuario);

            var cliente = new Cliente { Nombre = "Jose", Apellido = "Mendez", Cedula = "888-8888888-8", IdUsuario = usuario.Id };
            await _clienteRepository.SaveAsync(cliente);

            //Act
            var result = await _clienteServices.GetByCedulaAsync("888-8888888-8");

            //Assert
            Assert.True(result.Success);
            Assert.Equal("Se obtuvo el cliente por correo correctamnete.", result.Message);
        }

        // -------------------- UPDATE TESTS --------------------

        [Fact]
        public async Task When_UpdateDto_is_null_Should_Fail()
        {
            //Arrange
            UpdateClienteDto dto = null;

            //Act
            var result = await _clienteServices.UpdateAsync(dto);

            //Assert
            Assert.False(result.Success);
            Assert.Equal("El cliente no puede ser nulo.", result.Message);
        }

        [Fact]
        public async Task When_Id_is_invalid_UpdateAsync_Should_Fail()
        {
            //Arrange
            var dto = new UpdateClienteDto { Id = 0 };

            //Act
            var result = await _clienteServices.UpdateAsync(dto);

            //Assert
            Assert.False(result.Success);
            Assert.Equal("El id ingresado no es valido.", result.Message);
        }

        [Fact]
        public async Task When_Cliente_valid_UpdateAsync_Should_Succeed()
        {
            //Arrange
            var usuario = new Usuario { Nombre = "User", Correo = "update@test.com", Contraseña = "123456789", Rol = RolUsuarios.Cliente };
            await _usuarioRepository.SaveAsync(usuario);

            var cliente = new Cliente { Nombre = "Carla", Apellido = "Jimenez", Cedula = "111-1111111-1", IdUsuario = usuario.Id };
            await _clienteRepository.SaveAsync(cliente);

            var dto = new UpdateClienteDto
            {
                Id = cliente.Id,
                Nombre = "Carla Actualizada",
                Apellido = "Jimenez",
                Cedula = "222-2222222-2",
                Direccion = "Nueva Dir",
                Telefono = "555-555-5555",
                Correo = "update@test.com"
            };

            //Act
            var result = await _clienteServices.UpdateAsync(dto);

            //Assert
            Assert.Equal("Cliente actualizado exitosamente.", result.Message);
            Assert.True(result.Success);
            
        }
    }
}
