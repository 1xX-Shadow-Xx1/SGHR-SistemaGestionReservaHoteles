using Azure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SGHR.Domain.Base;
using SGHR.Domain.Entities.Configuration.Usuers;
using SGHR.Domain.Enum.Usuario;
using SGHR.Domain.Enum.Usuarios;
using SGHR.Domain.Validators.ConfigurationRules.Users;
using SGHR.Persistence.Context;
using SGHR.Persistence.Repositories.EF.Users;

namespace SGHR.Persistence.Test.UsersTest
{
    public class UsuarioRepositoryTest 
    {
        private readonly UsuarioRepository _usuarioRepository;
        private readonly SGHRContext _context;
        private readonly ILogger<UsuarioRepository> _logger;
        private readonly UsuarioValidator _usuarioValidator;
        private readonly IConfiguration _configuration;

        public UsuarioRepositoryTest()  
        {
            _usuarioValidator = new UsuarioValidator();

            var options = new DbContextOptionsBuilder<SGHRContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new SGHRContext(options);

            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
            });
            _logger = loggerFactory.CreateLogger<UsuarioRepository>();


            _configuration = new ConfigurationBuilder().Build();

            _usuarioRepository = new UsuarioRepository(
                _context,
                _usuarioValidator,
                _logger
            );
        }

        // ---------------------------
        // SAVE TESTS
        // ---------------------------

        [Fact]
        public async Task SaveUsuario_When_Is_NullOrEmpity()
        {
            //Arrange
            Usuario usuario = null;

            //Act
            var result = await _usuarioRepository.SaveAsync(usuario);
            string message = "Usuario no puede ser nulo.";

            //Assert
            Assert.IsType<OperationResult<Usuario>>(result);
            Assert.Equal(message, result.Message);
        }

        [Fact]
        public async Task SaveUsuario_When_Nombre_Is_TooShort_Should_Fail()
        {
            //Arrange
            var usuario = new Usuario
            {
                Nombre = "Ke",
                Correo = "kevin@example.com",
                Contraseña = "12345678",
                Rol = RolUsuarios.Cliente
            };

            //Act
            var result = await _usuarioRepository.SaveAsync(usuario);

            //Assert
            Assert.False(result.Success);
            Assert.Contains("Nombre debe tener al menos 3 caracteres.", result.Message, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public async Task SaveUsuario_When_Correo_Is_Invalid_Should_Fail()
        {
            //Arrange
            var usuario = new Usuario
            {
                Nombre = "Kevin Test",
                Correo = "correo_invalido",
                Contraseña = "12345678",
                Rol = RolUsuarios.Cliente
            };

            //Act
            var result = await _usuarioRepository.SaveAsync(usuario);

            //Assert
            Assert.False(result.Success);
            Assert.Contains("Correo inválido", result.Message);
        }

        [Fact]
        public async Task SaveUsuario_When_Password_Is_TooShort_Should_Fail()
        {
            //Arrange
            var usuario = new Usuario
            {
                Nombre = "Kevin Test",
                Correo = "kevin@test.com",
                Contraseña = "123",
                Rol = RolUsuarios.Cliente
            };

            //Act
            var result = await _usuarioRepository.SaveAsync(usuario);

            //Assert
            Assert.False(result.Success);
            Assert.Contains("La contraseña debe tener al menos 8 caracteres.", result.Message, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public async Task SaveUsuario_When_Rol_Is_Invalid_Should_Fail()
        {
            //Arrange
            var usuario = new Usuario
            {
                Nombre = "Kevin Test",
                Correo = "kevin@example.com",
                Contraseña = "12345678",
                Rol = (RolUsuarios)999
            };

            //Act
            var result = await _usuarioRepository.SaveAsync(usuario);

            //Assert
            Assert.False(result.Success);
            Assert.Contains("Rol contiene un valor inválido.", result.Message, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public async Task SaveUsuario_When_Data_Is_Valid_Should_Succeed()
        {
            //Arrange
            var usuario = new Usuario
            {
                Nombre = "Kevin Perfecto",
                Correo = "kevinperfecto@example.com",
                Contraseña = "segura123",
                Rol = RolUsuarios.Cliente
            };

            //Act
            var result = await _usuarioRepository.SaveAsync(usuario);

            //Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal("Kevin Perfecto", result.Data.Nombre);
        }

        [Fact]
        public async Task SaveUsuario_When_Validator_Changes_Should_Explode()
        {
            //Arrange
            var usuario = new Usuario
            {
                Nombre = "K",
                Correo = "sinarroba",
                Contraseña = "123",
                Rol = (RolUsuarios)500
            };

            //Act
            var result = await _usuarioRepository.SaveAsync(usuario);

            //Assert
            Assert.False(result.Success);
        }


        // ---------------------------
        // UPDATE TESTS
        // ---------------------------


        [Fact]
        public async Task UpdateUsuario_When_Invalid_Should_Return_Fail()
        {
            //Arrange
            var usuario = new Usuario
            {
                Nombre = "", // inválido
                Correo = "user@test.com",
                Contraseña = "12345678",
                Rol = RolUsuarios.Cliente
            };

            //Act
            var result = await _usuarioRepository.UpdateAsync(usuario);

            //Assert
            Assert.False(result.Success);
            Assert.Contains("Nombre", result.Message);
        }

        [Fact]
        public async Task UpdateUsuario_When_Valid_Should_Return_Success()
        {
            //Arrange
            var usuario = new Usuario
            {
                Nombre = "Update Test",
                Correo = "update@test.com",
                Contraseña = "12345678",
                Rol = RolUsuarios.Cliente
            };
            await _usuarioRepository.SaveAsync(usuario);
            usuario.Nombre = "Update Test Modificado";

            //Act
            var result = await _usuarioRepository.UpdateAsync(usuario);

            //Assert
            Assert.True(result.Success);
        }

        // ---------------------------
        // DELETE TESTS
        // ---------------------------

        [Fact]
        public async Task DeleteUsuario_When_Valid_Should_Return_Success()
        {
            //Arrange
            var usuario = new Usuario
            {
                Nombre = "Delete Test",
                Correo = "delete@test.com",
                Contraseña = "12345678",
                Rol = RolUsuarios.Cliente
            };
            await _usuarioRepository.SaveAsync(usuario);

            //Act
            var result = await _usuarioRepository.DeleteAsync(usuario);

            //Assert
            Assert.True(result.Success);
        }

        [Fact]
        public async Task DeleteUsuario_When_ContextDisposed_Should_Return_Fail()
        {
            //Arrange
            var usuario = new Usuario
            {
                Nombre = "Delete Exception",
                Correo = "del@test.com",
                Contraseña = "12345678",
                Rol = RolUsuarios.Cliente
            };
            await _usuarioRepository.SaveAsync(usuario);

            _context.Dispose();

            //Act
            var result = await _usuarioRepository.DeleteAsync(usuario);

            //Assert
            Assert.False(result.Success);
            Assert.Equal("Ocurrió un error al eliminar el usuario.", result.Message);
        }

        // ---------------------------
        // GET TESTS
        // ---------------------------

        [Fact]
        public async Task GetById_When_NotFound_Should_Return_Fail()
        {
            //Arrange
            //Act
            var result = await _usuarioRepository.GetByIdAsync(999);

            //Assert
            Assert.False(result.Success);
        }

        [Fact]
        public async Task GetAll_Should_Return_List()
        {
            //Arrange
            await _usuarioRepository.SaveAsync(new Usuario
            {
                Nombre = "All Test",
                Correo = "all@test.com",
                Contraseña = "12345678",
                Rol = RolUsuarios.Cliente
            });


            //Act
            var result = await _usuarioRepository.GetAllAsync();

            //Assert
            Assert.True(result.Success);
            Assert.NotEmpty(result.Data);
        }

        [Fact]
        public async Task GetByCorreo_When_NotExists_Should_Return_Fail()
        {
            //Arrange
            //Act
            var result = await _usuarioRepository.GetByCorreoAsync("noexiste@test.com");

            //Assert
            Assert.False(result.Success);
            Assert.Equal("Usuario no encontrado", result.Message);
        }

        [Fact]
        public async Task GetByCorreo_When_Valid_Should_Return_Success()
        {
            //Arrange
            var usuario = new Usuario
            {
                Nombre = "Correo Test",
                Correo = "correo@test.com",
                Contraseña = "12345678",
                Rol = RolUsuarios.Cliente
            };
            await _usuarioRepository.SaveAsync(usuario);

            //Act
            var result = await _usuarioRepository.GetByCorreoAsync("correo@test.com");

            //Assert
            Assert.True(result.Success);
            Assert.Equal(usuario.Correo, result.Data.Correo);
        }

        [Fact]
        public async Task GetActivos_Should_Return_Only_Active_Users()
        {
            //Arrange
            var usuarioActivo = new Usuario
            {
                Nombre = "Activo",
                Correo = "activo@test.com",
                Contraseña = "12345678",
                Rol = RolUsuarios.Cliente,
                Estado = EstadoUsuario.Activo
            };

            
            var usuarioInactivo = new Usuario
            {
                Nombre = "Inactivo",
                Correo = "inactivo@test.com",
                Contraseña = "12345678",
                Rol = RolUsuarios.Cliente,
                Estado = EstadoUsuario.Inactivo
            };

            await _usuarioRepository.SaveAsync(usuarioActivo);
            await _usuarioRepository.SaveAsync(usuarioInactivo);

            //Act
            var result = await _usuarioRepository.GetActivosAsync();

            //Assert
            Assert.True(result.Success);
            Assert.Single(result.Data); // Solo 1 activo
            Assert.Equal("Activo", result.Data[0].Nombre);
        }
    }
}