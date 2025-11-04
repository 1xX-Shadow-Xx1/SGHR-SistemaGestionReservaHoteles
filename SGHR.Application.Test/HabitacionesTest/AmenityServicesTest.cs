

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SGHR.Application.Dtos.Configuration.Habitaciones.Amenity;
using SGHR.Application.Services.Habitaciones;
using SGHR.Domain.Entities.Configuration.Habitaciones;
using SGHR.Domain.Validators.ConfigurationRules.Habitaciones;
using SGHR.Persistence.Context;
using SGHR.Persistence.Repositories.EF.Habitaciones;

namespace SGHR.Application.Test.HabitacionesTest
{
    public class AmenityServicesTest
    {
        private readonly SGHRContext _context;
        private readonly AmenityServices _amenityServices;
        private readonly AmenityRepository _amenityRepository;
        private readonly ILogger<AmenityServices> _logger;

        public AmenityServicesTest()
        {
            var options = new DbContextOptionsBuilder<SGHRContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new SGHRContext(options);

            var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
            _logger = loggerFactory.CreateLogger<AmenityServices>();

            var amenityValidator = new AmenitiesValidator();
            var loggerAmenity = loggerFactory.CreateLogger<AmenityRepository>();
            _amenityRepository = new AmenityRepository(_context, amenityValidator, loggerAmenity);

            _amenityServices = new AmenityServices(_logger, _amenityRepository);

            // Datos iniciales
            _amenityRepository.SaveAsync(new Amenity { Nombre = "Piscina", Descripcion = "Piscina olímpica" }).Wait();
            _amenityRepository.SaveAsync(new Amenity { Nombre = "Gimnasio", Descripcion = "Gimnasio completo" }).Wait();
        }

        // ---------------------------
        // CREATEASYNC TESTS
        // ---------------------------

        [Fact]
        public async Task CreateAsync_When_NullDto_Returns_Error()
        {
            // Arrange
            CreateAmenityDto dto = null;

            // Act
            var result = await _amenityServices.CreateAsync(dto);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("El Amenety no puede ser nulo.", result.Message);
        }

        [Fact]
        public async Task CreateAsync_When_Name_Already_Exists_Returns_Error()
        {
            // Arrange
            var dto = new CreateAmenityDto { Nombre = "Piscina", Descripcion = "Otra descripción" };

            // Act
            var result = await _amenityServices.CreateAsync(dto);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Ya existe un Amenity con ese nombre.", result.Message);
        }

        [Fact]
        public async Task CreateAsync_When_Valid_Returns_Success()
        {
            // Arrange
            var dto = new CreateAmenityDto { Nombre = "Spa", Descripcion = "Spa relajante" };

            // Act
            var result = await _amenityServices.CreateAsync(dto);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Amenity registrado correctamente.", result.Message);
            var amenity = Assert.IsType<AmenityDto>(result.Data);
            Assert.Equal("Spa", amenity.Nombre);
        }

        // ---------------------------
        // DELETEASYNC TESTS
        // ---------------------------

        [Fact]
        public async Task DeleteAsync_When_Id_Is_Invalid_Returns_Error()
        {
            // Act
            var result = await _amenityServices.DeleteAsync(0);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("El id ingresado no es valido.", result.Message);
        }

        [Fact]
        public async Task DeleteAsync_When_Not_Found_Returns_Error()
        {
            // Act
            var result = await _amenityServices.DeleteAsync(999);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("No existe un Amenety con ese id.", result.Message);
        }

        [Fact]
        public async Task DeleteAsync_When_Valid_Returns_Success()
        {
            // Arrange
            var amenity = new Amenity { Nombre = "Cancha", Descripcion = "Cancha de tenis" };
            await _amenityRepository.SaveAsync(amenity);

            // Act
            var result = await _amenityServices.DeleteAsync(amenity.Id);

            // Assert
            Assert.True(result.Success);
            Assert.Contains("eliminado correctamente", result.Message);
        }

        // ---------------------------
        // GETALLASYNC TESTS
        // ---------------------------

        [Fact]
        public async Task GetAllAsync_Returns_All_Amenities()
        {
            // Act
            var result = await _amenityServices.GetAllAsync();

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Se obtuvieron los amenity correctamnete.", result.Message);
            var list = Assert.IsAssignableFrom<System.Collections.Generic.List<AmenityDto>>(result.Data);
            Assert.True(list.Count >= 2);
        }

        [Fact]
        public async Task GetAllAsync_When_No_Data_Returns_Empty_List()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<SGHRContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var context = new SGHRContext(options);
            var validator = new AmenitiesValidator();
            var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
            var repo = new AmenityRepository(context, validator, loggerFactory.CreateLogger<AmenityRepository>());
            var service = new AmenityServices(loggerFactory.CreateLogger<AmenityServices>(), repo);

            // Act
            var result = await service.GetAllAsync();

            // Assert
            Assert.True(result.Success);
            var list = Assert.IsAssignableFrom<System.Collections.Generic.List<AmenityDto>>(result.Data);
            Assert.Empty(list);
        }

        [Fact]
        public async Task GetAllAsync_Should_Return_Success_True()
        {
            // Act
            var result = await _amenityServices.GetAllAsync();

            // Assert
            Assert.True(result.Success);
        }

        // ---------------------------
        // GETBYIDASYNC TESTS
        // ---------------------------

        [Fact]
        public async Task GetByIdAsync_When_Id_Is_Invalid_Returns_Error()
        {
            // Act
            var result = await _amenityServices.GetByIdAsync(0);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("El id ingresado no es valido.", result.Message);
        }

        [Fact]
        public async Task GetByIdAsync_When_Not_Found_Returns_Error()
        {
            // Act
            var result = await _amenityServices.GetByIdAsync(999);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Amenity no encontrada.", result.Message);
        }

        [Fact]
        public async Task GetByIdAsync_When_Valid_Returns_Success()
        {
            // Arrange
            var amenity = await _amenityRepository.GetAllAsync();
            var id = amenity.Data.First().Id;

            // Act
            var result = await _amenityServices.GetByIdAsync(id);

            // Assert
            Assert.True(result.Success);
            var data = Assert.IsType<AmenityDto>(result.Data);
            Assert.Equal(id, data.Id);
        }

        // ---------------------------
        // UPDATEASYNC TESTS
        // ---------------------------

        [Fact]
        public async Task UpdateAsync_When_NullDto_Returns_Error()
        {
            // Arrange
            UpdateAmenityDto dto = null;

            // Act
            var result = await _amenityServices.UpdateAsync(dto);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("El Amenety no puede ser nulo.", result.Message);
        }

        [Fact]
        public async Task UpdateAsync_When_Id_Invalid_Returns_Error()
        {
            // Arrange
            var dto = new UpdateAmenityDto { Id = 0, Nombre = "Test", Descripcion = "Test" };

            // Act
            var result = await _amenityServices.UpdateAsync(dto);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("El id ingresado no es valido.", result.Message);
        }

        [Fact]
        public async Task UpdateAsync_When_Valid_Returns_Success()
        {
            // Arrange
            var amenity = (await _amenityRepository.GetAllAsync()).Data.First();
            var dto = new UpdateAmenityDto
            {
                Id = amenity.Id,
                Nombre = "Actualizado",
                Descripcion = "Modificado"
            };

            // Act
            var result = await _amenityServices.UpdateAsync(dto);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Amenety actualizado correctamente.", result.Message);
            var data = Assert.IsType<AmenityDto>(result.Data);
            Assert.Equal("Actualizado", data.Nombre);
        }
    }
}
