

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SGHR.Application.Dtos.Configuration.Operaciones.Reporte;
using SGHR.Application.Services.Operaciones;
using SGHR.Domain.Entities.Configuration.Reportes;
using SGHR.Domain.Entities.Configuration.Usuers;
using SGHR.Domain.Enum.Usuarios;
using SGHR.Domain.Validators.ConfigurationRules.Operaciones;
using SGHR.Domain.Validators.ConfigurationRules.Users;
using SGHR.Persistence.Context;
using SGHR.Persistence.Repositories.EF.Operaciones;
using SGHR.Persistence.Repositories.EF.Users;

namespace SGHR.Application.Test.OperacionesTest
{
    public class ReporteServicesTest
    {
        private readonly ReporteServices _service;
        private readonly SGHRContext _context;
        private readonly ReporteRepository _reporteRepository;
        private readonly UsuarioRepository _usuarioRepository;
        private readonly ILogger<ReporteServices> _logger;
        private readonly ILogger<ReporteRepository> _loggerRepo;
        private readonly ILogger<UsuarioRepository> _usuarioRepo;
        private readonly ReporteValidator _validator;
        private readonly UsuarioValidator _usuarioValidator;

        public ReporteServicesTest()
        {
            _validator = new ReporteValidator();
            _usuarioValidator = new UsuarioValidator();

            // Crear base de datos en memoria
            var options = new DbContextOptionsBuilder<SGHRContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new SGHRContext(options);

            // Crear logger
            var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
            _logger = loggerFactory.CreateLogger<ReporteServices>();
            _loggerRepo = loggerFactory.CreateLogger<ReporteRepository>();
            _usuarioRepo = loggerFactory.CreateLogger<UsuarioRepository>();

            // Instanciar repositorios reales
            _reporteRepository = new ReporteRepository(_context, _validator, _loggerRepo);
            _usuarioRepository = new UsuarioRepository(_context, _usuarioValidator, _usuarioRepo);

            // Instanciar servicio
            _service = new ReporteServices(_logger, _reporteRepository, _usuarioRepository);
        }

        // =====================================================
        // CREATEASYNC TESTS
        // =====================================================

        [Fact]
        public async Task CreateAsync_When_Dto_Is_Null_Should_Return_Error()
        {
            // Arrange
            CreateReporteDto dto = null;

            // Act
            var result = await _service.CreateAsync(dto);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("El reporte no puede ser nulo.", result.Message);
        }

        [Fact]
        public async Task CreateAsync_When_Usuario_Not_Found_Should_Return_Error()
        {
            // Arrange
            var dto = new CreateReporteDto
            {
                TipoReporte = "Ventas",
                GeneradoPor = "inexistente@mail.com",
                RutaArchivo = "reporte.pdf"
            };

            // Act
            var result = await _service.CreateAsync(dto);

            // Assert
            Assert.False(result.Success);
            Assert.Contains("No se encontro el usuario", result.Message);
        }

        [Fact]
        public async Task CreateAsync_When_Data_Is_Valid_Should_Create_Successfully()
        {
            // Arrange
            var usuario = new Usuario
            {
                Nombre = "Kevin",
                Correo = "kevin@mail.com",
                Contraseña = "12345678",
                Rol = RolUsuarios.Cliente
            };
            await _usuarioRepository.SaveAsync(usuario);

            var dto = new CreateReporteDto
            {
                TipoReporte = "Inventario",
                GeneradoPor = "kevin@mail.com",
                RutaArchivo = "ruta.pdf"
            };

            // Act
            var result = await _service.CreateAsync(dto);

            // Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal("Se a registrado el reporte correctamente.", result.Message);
        }

        // =====================================================
        // DELETEASYNC TESTS
        // =====================================================

        [Fact]
        public async Task DeleteAsync_When_Id_Is_Invalid_Should_Return_Error()
        {
            // Arrange
            int id = 0;

            // Act
            var result = await _service.DeleteAsync(id);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("El id es invalido.", result.Message);
        }

        [Fact]
        public async Task DeleteAsync_When_Reporte_Not_Found_Should_Return_Error()
        {
            // Arrange
            int id = 99;

            // Act
            var result = await _service.DeleteAsync(id);

            // Assert
            Assert.False(result.Success);
            Assert.Contains("Reporte no encontrado.", result.Message, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public async Task DeleteAsync_When_Success_Should_Return_Success_Message()
        {
            // Arrange
            var usuario = new Usuario
            {
                Nombre = "Juan",
                Correo = "juan@mail.com",
                Contraseña = "12345678",
                Rol = RolUsuarios.Cliente
            };
            var savedUser = await _usuarioRepository.SaveAsync(usuario);

            var reporte = new Reporte
            {
                TipoReporte = "Ventas",
                GeneradoPor = savedUser.Data.Id,
                RutaArchivo = "reporte.pdf"
            };
            var savedReporte = await _reporteRepository.SaveAsync(reporte);

            // Act
            var result = await _service.DeleteAsync(savedReporte.Data.Id);

            // Assert
            Assert.True(result.Success);
            Assert.Contains("eliminada correctamente", result.Message);
        }

        // =====================================================
        // GETALLASYNC TESTS
        // =====================================================

        [Fact]
        public async Task GetAllAsync_When_No_Reportes_Should_Return_Success_With_Empty_List()
        {
            // Arrange
            // No data inserted

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.True(result.Success);
            var data = Assert.IsType<List<ReporteDto>>(result.Data);
            Assert.Empty(data);
        }

        [Fact]
        public async Task GetAllAsync_When_Reportes_Exist_Should_Return_List()
        {
            // Arrange
            var usuario = new Usuario
            {
                Nombre = "Maria",
                Correo = "maria@mail.com",
                Contraseña = "12345678",
                Rol = RolUsuarios.Cliente
            };
            var savedUser = await _usuarioRepository.SaveAsync(usuario);

            await _reporteRepository.SaveAsync(new Reporte
            {
                TipoReporte = "Finanzas",
                GeneradoPor = savedUser.Data.Id,
                RutaArchivo = "reporte.pdf"
            });

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.True(result.Success);
            var data = Assert.IsType<List<ReporteDto>>(result.Data);
            Assert.NotEmpty(data);
        }

        [Fact]
        public async Task GetAllAsync_Should_Return_Message_On_Success()
        {
            // Arrange
            var result = await _service.GetAllAsync();

            // Assert
            Assert.Equal("Se obtuvieron los reportes correctamente.", result.Message);
        }

        // =====================================================
        // GETBYIDASYNC TESTS
        // =====================================================

        [Fact]
        public async Task GetByIdAsync_When_Id_Is_Invalid_Should_Return_Error()
        {
            // Arrange
            int id = 0;

            // Act
            var result = await _service.GetByIdAsync(id);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("El id es invalido.", result.Message);
        }

        [Fact]
        public async Task GetByIdAsync_When_Not_Found_Should_Return_Error()
        {
            // Arrange
            int id = 999;

            // Act
            var result = await _service.GetByIdAsync(id);

            // Assert
            Assert.False(result.Success);
            Assert.Contains("Reporte no encontrado.", result.Message, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public async Task GetByIdAsync_When_Success_Should_Return_ReporteDto()
        {
            // Arrange
            var usuario = new Usuario
            {
                Nombre = "Carlos",
                Correo = "carlos@mail.com",
                Contraseña = "12345678",
                Rol = RolUsuarios.Cliente
            };
            var savedUser = await _usuarioRepository.SaveAsync(usuario);

            var reporte = new Reporte
            {
                TipoReporte = "Ventas",
                GeneradoPor = savedUser.Data.Id,
                RutaArchivo = "reporte.pdf"
            };
            var savedReporte = await _reporteRepository.SaveAsync(reporte);

            // Act
            var result = await _service.GetByIdAsync(savedReporte.Data.Id);

            // Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal($"Se obtuvo el reporte con id {savedReporte.Data.Id} correctamnete.", result.Message);
        }

        // =====================================================
        // UPDATEASYNC TESTS
        // =====================================================

        [Fact]
        public async Task UpdateAsync_When_Dto_Is_Null_Should_Return_Error()
        {
            // Arrange
            UpdateReporteDto dto = null;

            // Act
            var result = await _service.UpdateAsync(dto);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("El reporte no puede ser nula.", result.Message);
        }

        [Fact]
        public async Task UpdateAsync_When_Id_Is_Invalid_Should_Return_Error()
        {
            // Arrange
            var dto = new UpdateReporteDto
            {
                Id = 0,
                TipoReporte = "Inventario",
                GeneradoPor = "correo@mail.com",
                RutaArchivo = "path.pdf"
            };

            // Act
            var result = await _service.UpdateAsync(dto);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("El id es invalido.", result.Message);
        }

        [Fact]
        public async Task UpdateAsync_When_Success_Should_Update_Correctly()
        {
            // Arrange
            var usuario = new Usuario
            {
                Nombre = "Luis",
                Correo = "luis@mail.com",
                Contraseña = "12345678",
                Rol = RolUsuarios.Cliente
            };
            var savedUser = await _usuarioRepository.SaveAsync(usuario);

            var reporte = new Reporte
            {
                TipoReporte = "Inventario",
                GeneradoPor = savedUser.Data.Id,
                RutaArchivo = "old.pdf"
            };
            var savedReporte = await _reporteRepository.SaveAsync(reporte);

            var dto = new UpdateReporteDto
            {
                Id = savedReporte.Data.Id,
                TipoReporte = "Actualizado",
                GeneradoPor = "luis@mail.com",
                RutaArchivo = "nuevo.pdf"
            };

            // Act
            var result = await _service.UpdateAsync(dto);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Se a actualizado el reporte correctamente.", result.Message);
        }

    }
}
