

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SGHR.Application.Dtos.Configuration.Users.Usuario;
using SGHR.Application.Services.Usuarios;
using SGHR.Domain.Entities.Configuration.Usuers;
using SGHR.Domain.Enum.Usuarios;
using SGHR.Domain.Validators.ConfigurationRules.Users;
using SGHR.Persistence.Context;
using SGHR.Persistence.Repositories.EF.Users;

namespace SGHR.Application.Test.UsuariosTest
{
    public class UsuarioServisTest
    {
        private readonly UsuarioRepository _usuarioRepository;
        private readonly SGHRContext _context;
        private readonly ILogger<UsuarioRepository> _loggerRepo;
        private readonly UsuarioValidator _usuarioValidator;
        private readonly IConfiguration _configuration;
        private readonly UsuarioServices _usuarioServices;
        private readonly ILogger<UsuarioServices> _loggerServices;

        public UsuarioServisTest()
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


            _loggerRepo = loggerFactory.CreateLogger<UsuarioRepository>();
            _loggerServices = loggerFactory.CreateLogger<UsuarioServices>();

            _configuration = new ConfigurationBuilder().Build();

            _usuarioRepository = new UsuarioRepository(
                _context,
                _usuarioValidator,
                _loggerRepo
            );

            _usuarioServices = new UsuarioServices(
                _loggerServices,
                _usuarioRepository
            );

        }

        // -------------------- CREATE TESTS --------------------

        [Fact]
        public async Task When_User_is_null_CreateAsync_Should_Fail()
        {
            //Arrange
            CreateUsuarioDto usuario = null;

            //Act
            var result = await _usuarioServices.CreateAsync(usuario);

            //Assert
            Assert.False(result.Success);
            Assert.Equal("El usuario no puede ser nulo.", result.Message);
        }

        [Fact]
        public async Task When_User_email_already_exists_CreateAsync_Should_Fail()
        {
            //Arrange
            var usuario = new Usuario { Nombre = "Juan", Correo = "juan@test.com", Contraseña = "123456789", Rol = RolUsuarios.Administrador };
            await _usuarioRepository.SaveAsync(usuario);

            var nuevo = new CreateUsuarioDto { Nombre = "Pedro", Correo = "juan@test.com", Contraseña = "321123456", Rol = RolUsuarios.Administrador };

            //Act
            var result = await _usuarioServices.CreateAsync(nuevo);

            //Assert
            Assert.False(result.Success);
            Assert.Equal("Ya existe un usuario registrado con ese correo.", result.Message);
        }

        [Fact]
        public async Task When_User_is_valid_CreateAsync_Should_Succeed()
        {
            //Arrange
            var dto = new CreateUsuarioDto { Nombre = "Maria", Correo = "maria@test.com", Contraseña = "123456789", Rol = RolUsuarios.Administrador };

            //Act
            var result = await _usuarioServices.CreateAsync(dto);

            //Assert
            Assert.True(result.Success);
            Assert.Equal("Usuario registrado correctamente.", result.Message);
        }

        [Fact]
        public async Task When_Exception_occurs_CreateAsync_Should_Fail()
        {
            //Arrange
            _context.Dispose(); // provocar excepción
            var dto = new CreateUsuarioDto { Nombre = "Error", Correo = "error@test.com", Contraseña = "123456789", Rol = RolUsuarios.Administrador };

            //Act
            var result = await _usuarioServices.CreateAsync(dto);

            //Assert
            Assert.False(result.Success);
            Assert.Contains("Ocurrió un error al obtener los usuarios.", result.Message);
        }

        // -------------------- DELETE TESTS --------------------

        [Fact]
        public async Task When_Id_is_invalid_DeleteAsync_Should_Fail()
        {
            //Arrange
            int id = 0;

            //Act
            var result = await _usuarioServices.DeleteAsync(id);

            //Assert
            Assert.False(result.Success);
            Assert.Equal("El id ingresado no es valido.", result.Message);
        }

        [Fact]
        public async Task When_User_does_not_exist_DeleteAsync_Should_Fail()
        {
            //Arrange
            int id = 99;

            //Act
            var result = await _usuarioServices.DeleteAsync(id);

            //Assert
            Assert.False(result.Success);
            Assert.Equal("Usuario no encontrado.", result.Message);
        }

        [Fact]
        public async Task When_User_exists_DeleteAsync_Should_Succeed()
        {
            //Arrange
            var usuario = new Usuario { Nombre = "Carlos", Correo = "carlos@test.com", Contraseña = "123123456789", Rol = RolUsuarios.Administrador };
            await _usuarioRepository.SaveAsync(usuario);

            //Act
            var result = await _usuarioServices.DeleteAsync(usuario.Id);

            //Assert
            Assert.True(result.Success);
            Assert.Contains("eliminado correctamente", result.Message);
        }

        [Fact]
        public async Task When_Exception_occurs_DeleteAsync_Should_Fail()
        {
            //Arrange
            _context.Dispose();

            //Act
            var result = await _usuarioServices.DeleteAsync(1);

            //Assert
            Assert.False(result.Success);
            Assert.Contains("Ocurrió un error al obtener el usuario.", result.Message);
        }

        // -------------------- GET ALL TESTS --------------------

        [Fact]
        public async Task When_No_users_GetAllAsync_Should_Return_Empty()
        {
            //Arrange

            //Act
            var result = await _usuarioServices.GetAllAsync();

            //Assert
            Assert.True(result.Success);
            var lista = result.Data as List<UsuarioDto>;
            Assert.Empty(lista);
        }

        [Fact]
        public async Task When_Users_exist_GetAllAsync_Should_Succeed()
        {
            //Arrange
            await _usuarioRepository.SaveAsync(new Usuario { Nombre = "Ana", Correo = "ana@test.com", Contraseña = "123", Rol = RolUsuarios.Administrador });

            //Act
            var result = await _usuarioServices.GetAllAsync();

            //Assert
            Assert.True(result.Success);
            Assert.Equal("Se obtuvieron los usuarios correctamnete.", result.Message);
        }

        [Fact]
        public async Task When_Repository_returns_error_GetAllAsync_Should_Fail()
        {
            //Arrange
            _context.Dispose();

            //Act
            var result = await _usuarioServices.GetAllAsync();

            //Assert
            Assert.False(result.Success);
            Assert.Contains("Ocurrió un error al obtener los usuarios.", result.Message);
        }

        [Fact]
        public async Task When_List_has_data_GetAllAsync_Should_Return_List()
        {
            //Arrange
            await _usuarioRepository.SaveAsync(new Usuario { Nombre = "Leo", Correo = "leo@test.com", Contraseña = "123", Rol = RolUsuarios.Recepcionista });

            //Act
            var result = await _usuarioServices.GetAllAsync();

            //Assert
            Assert.NotNull(result.Data);
            Assert.True(result.Success);
        }

        // -------------------- GET BY ID TESTS --------------------

        [Fact]
        public async Task When_Id_is_invalid_GetByIdAsync_Should_Fail()
        {
            //Arrange
            int id = 0;

            //Act
            var result = await _usuarioServices.GetByIdAsync(id);

            //Assert
            Assert.False(result.Success);
            Assert.Equal("El id ingresado no es valido.", result.Message);
        }

        [Fact]
        public async Task When_User_does_not_exist_GetByIdAsync_Should_Fail()
        {
            //Arrange
            int id = 999;

            //Act
            var result = await _usuarioServices.GetByIdAsync(id);

            //Assert
            Assert.False(result.Success);
            Assert.Equal("Usuario no encontrado.", result.Message);
        }

        [Fact]
        public async Task When_User_exists_GetByIdAsync_Should_Succeed()
        {
            //Arrange
            var usuario = new Usuario { Nombre = "Mario", Correo = "mario@test.com", Contraseña = "123456789", Rol = RolUsuarios.Recepcionista };
            await _usuarioRepository.SaveAsync(usuario);

            //Act
            var result = await _usuarioServices.GetByIdAsync(usuario.Id);

            //Assert
            Assert.True(result.Success);
            Assert.Contains("Se obtuvo el usuario con id", result.Message);
        }

        [Fact]
        public async Task When_Exception_occurs_GetByIdAsync_Should_Fail()
        {
            //Arrange
            _context.Dispose();

            //Act
            var result = await _usuarioServices.GetByIdAsync(1);

            //Assert
            Assert.False(result.Success);
            Assert.Contains("Ocurrió un error al obtener el usuario.", result.Message);
        }

        // -------------------- UPDATE TESTS --------------------

        [Fact]
        public async Task When_UpdateDto_is_null_Should_Fail()
        {
            //Arrange
            UpdateUsuarioDto dto = null;

            //Act
            var result = await _usuarioServices.UpdateAsync(dto);

            //Assert
            Assert.False(result.Success);
            Assert.Equal("El usuario no puede ser nulo.", result.Message);
        }

        [Fact]
        public async Task When_Id_is_invalid_UpdateAsync_Should_Fail()
        {
            //Arrange
            var dto = new UpdateUsuarioDto { Id = 0 };

            //Act
            var result = await _usuarioServices.UpdateAsync(dto);

            //Assert
            Assert.False(result.Success);
            Assert.Equal("El id ingresado no es valido.", result.Message);
        }

        [Fact]
        public async Task When_User_not_found_UpdateAsync_Should_Fail()
        {
            //Arrange
            var dto = new UpdateUsuarioDto { Id = 99, Nombre = "Juan", Correo = "nuevo@test.com", Contraseña = "111456798", Rol = RolUsuarios.Recepcionista };

            //Act
            var result = await _usuarioServices.UpdateAsync(dto);

            //Assert
            Assert.False(result.Success);
            Assert.Equal("Usuario no encontrado.", result.Message);
        }

        [Fact]
        public async Task When_User_is_valid_UpdateAsync_Should_Succeed()
        {
            //Arrange
            var usuario = new Usuario { Nombre = "Roberto", Correo = "roberto@test.com", Contraseña = "123456789", Rol = RolUsuarios.Recepcionista };
            await _usuarioRepository.SaveAsync(usuario);

            var dto = new UpdateUsuarioDto
            {
                Id = usuario.Id,
                Nombre = "Roberto Actualizado",
                Correo = "roberto@test.com",
                Contraseña = "321456789",
                Rol = RolUsuarios.Recepcionista
            };

            //Act
            var result = await _usuarioServices.UpdateAsync(dto);

            //Assert
            Assert.True(result.Success);
            Assert.Equal("Usuario actualizado correctamente.", result.Message);
        }



    }
}
