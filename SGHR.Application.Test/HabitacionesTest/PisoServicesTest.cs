
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SGHR.Application.Dtos.Configuration.Habitaciones.Piso;
using SGHR.Application.Services.Habitaciones;
using SGHR.Domain.Entities.Configuration.Habitaciones;
using SGHR.Domain.Enum.Habitaciones;
using SGHR.Domain.Validators.ConfigurationRules.Habitaciones;
using SGHR.Persistence.Context;
using SGHR.Persistence.Repositories.EF.Habitaciones;

namespace SGHR.Application.Test.HabitacionesTest
{
    public class PisoServicesTest
    {
        private readonly SGHRContext _context;
        private readonly PisoServices _pisoServices;
        private readonly PisoRepository _pisoRepository;
        private readonly ILogger<PisoServices> _logger;

        public PisoServicesTest()
        {
            var options = new DbContextOptionsBuilder<SGHRContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new SGHRContext(options);

            var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
            _logger = loggerFactory.CreateLogger<PisoServices>();

            var pisoValidator = new PisoValidator();
            var loggerPisoRepo = loggerFactory.CreateLogger<PisoRepository>();

            _pisoRepository = new PisoRepository(_context, pisoValidator, loggerPisoRepo);

            // Inicializa el servicio con el repositorio real
            _pisoServices = new PisoServices(_logger, _pisoRepository);

            // Datos iniciales
            _pisoRepository.SaveAsync(new Piso { NumeroPiso = 1, Descripcion = "Piso 1" }).Wait();
            _pisoRepository.SaveAsync(new Piso { NumeroPiso = 2, Descripcion = "Piso 2" }).Wait();
        }

        // ---------------------------
        // CREATE TESTS
        // ---------------------------

        [Fact]
        public async Task CreateAsync_When_Dto_Is_Null_Should_Return_Error()
        {
            // Arrange
            CreatePisoDto dto = null;

            // Act
            var result = await _pisoServices.CreateAsync(dto);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("El Piso no puede ser nulo.", result.Message);
        }

        [Fact]
        public async Task CreateAsync_When_NumeroPiso_Already_Exists_Should_Fail()
        {
            // Arrange
            var dto = new CreatePisoDto { NumeroPiso = 1, Descripcion = "Duplicado" };

            // Act
            var result = await _pisoServices.CreateAsync(dto);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Ya existe un Piso con ese numero.", result.Message);
        }

        [Fact]
        public async Task CreateAsync_When_Data_Is_Valid_Should_Create_Successfully()
        {
            // Arrange
            var dto = new CreatePisoDto { NumeroPiso = 3, Descripcion = "Piso Nuevo" };

            // Act
            var result = await _pisoServices.CreateAsync(dto);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Piso registrado correctamente.", result.Message);
            Assert.NotNull(result.Data);
        }

        // ---------------------------
        // DELETE TESTS
        // ---------------------------

        [Fact]
        public async Task DeleteAsync_When_Id_Is_Invalid_Should_Return_Error()
        {
            // Arrange
            int id = 0;

            // Act
            var result = await _pisoServices.DeleteAsync(id);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("El id ingresado no es valido.", result.Message);
        }

        [Fact]
        public async Task DeleteAsync_When_Id_Not_Found_Should_Return_Error()
        {
            // Arrange
            int id = 999;

            // Act
            var result = await _pisoServices.DeleteAsync(id);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("No existe un piso con ese id.", result.Message);
        }

        [Fact]
        public async Task DeleteAsync_When_Valid_Id_Should_Delete_Successfully()
        {
            // Arrange
            var piso = (await _pisoRepository.GetAllAsync()).Data.First();

            // Act
            var result = await _pisoServices.DeleteAsync(piso.Id);

            // Assert
            Assert.True(result.Success);
            Assert.Contains("eliminado correctamente", result.Message, StringComparison.OrdinalIgnoreCase);
        }

        // ---------------------------
        // GET ALL TESTS
        // ---------------------------

        [Fact]
        public async Task GetAllAsync_Should_Return_List_Of_Pisos()
        {
            // Arrange
            // Act
            var result = await _pisoServices.GetAllAsync();

            // Assert
            Assert.True(result.Success);
            var data = Assert.IsAssignableFrom<System.Collections.Generic.List<PisoDto>>(result.Data);
            Assert.True(data.Count >= 2);
        }

        [Fact]
        public async Task GetAllAsync_When_No_Pisos_Should_Return_Empty_List()
        {
            // Arrange
            var emptyContextOptions = new DbContextOptionsBuilder<SGHRContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var emptyContext = new SGHRContext(emptyContextOptions);
            var validator = new PisoValidator();
            var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
            var repo = new PisoRepository(emptyContext, validator, loggerFactory.CreateLogger<PisoRepository>());
            var service = new PisoServices(_logger, repo);

            // Act
            var result = await service.GetAllAsync();

            // Assert
            Assert.True(result.Success);
            var data = Assert.IsAssignableFrom<System.Collections.Generic.List<PisoDto>>(result.Data);
            Assert.Empty(data);
        }

        [Fact]
        public async Task GetAllAsync_Should_Return_Success_Message()
        {
            // Arrange
            // Act
            var result = await _pisoServices.GetAllAsync();

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Se obtuvieron los pisos correctamnete.", result.Message);
        }

        // ---------------------------
        // GET BY ID TESTS
        // ---------------------------

        [Fact]
        public async Task GetByIdAsync_When_Id_Is_Invalid_Should_Return_Error()
        {
            // Arrange
            int id = -1;

            // Act
            var result = await _pisoServices.GetByIdAsync(id);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("El id ingresado no es valido.", result.Message);
        }

        [Fact]
        public async Task GetByIdAsync_When_Piso_Not_Found_Should_Return_Error()
        {
            // Arrange
            int id = 9999;

            // Act
            var result = await _pisoServices.GetByIdAsync(id);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Piso no encontrado.", result.Message);
        }

        [Fact]
        public async Task GetByIdAsync_When_Valid_Id_Should_Return_Piso()
        {
            // Arrange
            var piso = (await _pisoRepository.GetAllAsync()).Data.First();

            // Act
            var result = await _pisoServices.GetByIdAsync(piso.Id);

            // Assert
            Assert.True(result.Success);
            Assert.Equal($"Se obtuvo el piso con id {piso.Id} correctamnete.", result.Message);
            Assert.NotNull(result.Data);
        }

        // ---------------------------
        // UPDATE TESTS
        // ---------------------------

        [Fact]
        public async Task UpdateAsync_When_Dto_Is_Null_Should_Return_Error()
        {
            // Arrange
            UpdatePisoDto dto = null;

            // Act
            var result = await _pisoServices.UpdateAsync(dto);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("El Piso no puede ser nulo.", result.Message);
        }

        [Fact]
        public async Task UpdateAsync_When_Id_Invalid_Should_Return_Error()
        {
            // Arrange
            var dto = new UpdatePisoDto { Id = 0, NumeroPiso = 5, Descripcion = "Update Test", Estado = EstadoPiso.Habilitado };

            // Act
            var result = await _pisoServices.UpdateAsync(dto);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("El id ingresado no es valido.", result.Message);
        }

        [Fact]
        public async Task UpdateAsync_When_Data_Is_Valid_Should_Update_Successfully()
        {
            // Arrange
            var piso = (await _pisoRepository.GetAllAsync()).Data.First();
            var dto = new UpdatePisoDto
            {
                Id = piso.Id,
                NumeroPiso = piso.NumeroPiso + 10,
                Descripcion = "Actualizado",
                Estado = EstadoPiso.EnMantenimiento
            };

            // Act
            var result = await _pisoServices.UpdateAsync(dto);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Piso actualizado correctamente.", result.Message);
        }


    }
}
