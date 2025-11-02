

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SGHR.Domain.Entities.Configuration.Usuers;
using SGHR.Domain.Validators.ConfigurationRules.Users;
using SGHR.Persistence.Context;
using SGHR.Persistence.Repositories.EF.Users;

namespace SGHR.Persistence.Test.UsersTest
{
    public class ClienteRepositoryTest
    {
        private readonly ClienteRepository _clienteRepository;
        private readonly SGHRContext _context;
        private readonly ILogger<ClienteRepository> _logger;
        private readonly ClienteValidator _clienteValidator;
        private readonly IConfiguration _configuration;

        public ClienteRepositoryTest()
        {
            _clienteValidator = new ClienteValidator();

            var options = new DbContextOptionsBuilder<SGHRContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new SGHRContext(options);

            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
            });
            _logger = loggerFactory.CreateLogger<ClienteRepository>();


            _configuration = new ConfigurationBuilder().Build();

            _clienteRepository = new ClienteRepository(
                _context,
                _clienteValidator,
                _logger
            );
        }

        // ---------- SAVE TESTS ----------

        [Fact]
        public async Task SaveCliente_When_IsNull_ShouldFail()
        {
            //Arrange
            Cliente cliente = null;

            //Act
            var result = await _clienteRepository.SaveAsync(cliente);

            //Assert
            Assert.False(result.Success);
            Assert.Equal("Cliente no puede ser nulo.", result.Message);
        }

        [Fact]
        public async Task SaveCliente_When_DataIsInvalid_ShouldFailValidation()
        {
            //Arrange
            var cliente = new Cliente
            {
                Nombre = "",
                Apellido = "",
                Cedula = "123", // formato incorrecto
                Telefono = "abc",
            };

            //Act
            var result = await _clienteRepository.SaveAsync(cliente);

            //Assert
            Assert.False(result.Success);
            Assert.NotEmpty(result.Message);
        }

        [Fact]
        public async Task SaveCliente_When_DataIsValid_ShouldSaveSuccessfully()
        {
            //Arrange
            var cliente = new Cliente
            {
                Nombre = "Carlos",
                Apellido = "Ramirez",
                Cedula = "001-1234567-8",
                Telefono = "8095555555",
                Direccion = "Calle Falsa 123"
            };

            //Act
            var result = await _clienteRepository.SaveAsync(cliente);

            //Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal("Carlos", result.Data.Nombre);
        }

        [Fact]
        public async Task SaveCliente_When_DatabaseThrowsException_ShouldHandleError()
        {
            //Arrange

            var cliente = new Cliente
            {
                Nombre = "ErrorDB",
                Apellido = "Test",
                Cedula = "001-1234567-8"
            };

            _context.Dispose();

            //Act
            var result = await _clienteRepository.SaveAsync(cliente);

            //Assert
            Assert.False(result.Success);
            Assert.Equal("Ocurrió un error al guardar el cliente.", result.Message);
        }

        // ---------- UPDATE TESTS ----------

        [Fact]
        public async Task UpdateCliente_When_DataIsValid_ShouldUpdateSuccessfully()
        {
            //Arrange
            var cliente = new Cliente
            {
                Nombre = "Maria",
                Apellido = "Lopez",
                Cedula = "002-1234567-8"
            };

            
            await _clienteRepository.SaveAsync(cliente);

            //Act
            cliente.Nombre = "Maria Actualizada";

            var result = await _clienteRepository.UpdateAsync(cliente);

            //Assert
            Assert.True(result.Success);
            Assert.Equal("Maria Actualizada", result.Data.Nombre);
        }

        [Fact]
        public async Task UpdateCliente_When_DataIsInvalid_ShouldFail()
        {
            //Arrange
            var cliente = new Cliente
            {
                Nombre = "",
                Apellido = "",
                Cedula = "123"
            };

            //Act
            var result = await _clienteRepository.UpdateAsync(cliente);

            //Assert
            Assert.False(result.Success);
            Assert.NotEmpty(result.Message);
        }

        // ---------- DELETE TESTS ----------

        [Fact]
        public async Task DeleteCliente_When_DataIsValid_ShouldDeleteSuccessfully()
        {
            //Arrange
            var cliente = new Cliente
            {
                Nombre = "Pedro",
                Apellido = "Santos",
                Cedula = "003-1234567-8"
            };
            await _clienteRepository.SaveAsync(cliente);


            //Act
            var result = await _clienteRepository.DeleteAsync(cliente);

            //Assert
            Assert.True(result.Success);
        }

        [Fact]
        public async Task DeleteCliente_When_DatabaseFails_ShouldHandleError()
        {
            //Arrange

            var cliente = new Cliente
            {
                Nombre = "Pedro",
                Apellido = "Error",
                Cedula = "003-1234567-8"
            };

            _context.Dispose();

            //Act
            var result = await _clienteRepository.DeleteAsync(cliente);

            //Assert
            Assert.False(result.Success);
            Assert.Equal("Ocurrió un error al eliminar el cliente.", result.Message);
        }

        // ---------- GET BY ID TESTS ----------

        [Fact]
        public async Task GetByIdCliente_When_Exists_ShouldReturnCliente()
        {
            //Arrange
            var cliente = new Cliente
            {
                Nombre = "Laura",
                Apellido = "Torres",
                Cedula = "004-1234567-8"
            };
            await _clienteRepository.SaveAsync(cliente);

            //Act
            var result = await _clienteRepository.GetByIdAsync(cliente.Id);

            //Assert
            Assert.True(result.Success);
            Assert.Equal("Laura", result.Data.Nombre);
        }

        [Fact]
        public async Task GetByIdCliente_When_DatabaseFails_ShouldHandleError()
        {
            //Arrange
            _context.Dispose();

            //Act
            var result = await _clienteRepository.GetByIdAsync(999);

            //Assert
            Assert.False(result.Success);
            Assert.Equal("Ocurrió un error al obtener el cliente.", result.Message);
        }

        // ---------- GET ALL TESTS ----------

        [Fact]
        public async Task GetAllClientes_When_Exists_ShouldReturnList()
        {
            //Arrange
            await _clienteRepository.SaveAsync(new Cliente
            {
                Nombre = "Jose",
                Apellido = "Diaz",
                Cedula = "005-1234567-8"
            });

            //Act
            var result = await _clienteRepository.GetAllAsync();

            //Assert
            Assert.True(result.Success);
            Assert.NotEmpty(result.Data);
        }

        [Fact]
        public async Task GetAllClientes_When_DatabaseFails_ShouldHandleError()
        {
            //Arrange
            _context.Dispose();

            //Act
            var result = await _clienteRepository.GetAllAsync();

            //Assert
            Assert.False(result.Success);
            Assert.Equal("Ocurrió un error al obtener los clientes.", result.Message);
        }

        // ---------- GET BY CEDULA TESTS ----------

        [Fact]
        public async Task GetByCedulaCliente_When_Found_ShouldReturnCliente()
        {
            //Arrange
            var cliente = new Cliente
            {
                Nombre = "Andres",
                Apellido = "Reyes",
                Cedula = "006-1234567-8"
            };
            await _clienteRepository.SaveAsync(cliente);

            //Act
            var result = await _clienteRepository.GetByCedulaAsync("006-1234567-8");

            //Assert
            Assert.True(result.Success);
            Assert.Equal("Andres", result.Data.Nombre);
        }

        [Fact]
        public async Task GetByCedulaCliente_When_NotFound_ShouldFail()
        {
            //Arrange
            //Act
            var result = await _clienteRepository.GetByCedulaAsync("999-9999999-9");

            //Assert
            Assert.False(result.Success);
            Assert.Equal("Cliente no encontrado", result.Message);
        }

        // ---------- GET BY NOMBRE TESTS ----------

        [Fact]
        public async Task GetByNombreCliente_When_Found_ShouldReturnList()
        {
            //Arrange
            await _clienteRepository.SaveAsync(new Cliente
            {
                Nombre = "Mario",
                Apellido = "Santos",
                Cedula = "007-1234567-8"
            });

            //Act
            var result = await _clienteRepository.GetByNombreAsync("Mario");

            //Assert
            Assert.True(result.Success);
            Assert.NotEmpty(result.Data);
        }

        [Fact]
        public async Task GetByNombreCliente_When_DatabaseFails_ShouldHandleError()
        {
            //Arrange
            _context.Dispose();

            //Act
            var result = await _clienteRepository.GetByNombreAsync("ErrorTest");

            //Assert
            Assert.False(result.Success);
            Assert.Equal("Ocurrió un error al obtener los clientes por nombre", result.Message);
        }

    }
}
