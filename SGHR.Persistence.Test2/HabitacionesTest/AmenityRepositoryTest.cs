

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SGHR.Domain.Base;
using SGHR.Domain.Entities.Configuration.Habitaciones;
using SGHR.Domain.Validators.ConfigurationRules.Habitaciones;
using SGHR.Persistence.Context;
using SGHR.Persistence.Repositories.EF.Habitaciones;

namespace SGHR.Persistence.Test.HabitacionesTest
{
    public class AmenityRepositoryTest
    {
        private readonly AmenityRepository _amenityRepository;
        private readonly SGHRContext _context;
        private readonly ILogger<AmenityRepository> _logger;
        private readonly AmenitiesValidator _amenityValidator;
        private readonly IConfiguration _configuration;

        public AmenityRepositoryTest()
        {
            _amenityValidator = new AmenitiesValidator();

            var options = new DbContextOptionsBuilder<SGHRContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new SGHRContext(options);

            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
            });
            _logger = loggerFactory.CreateLogger<AmenityRepository>();


            _configuration = new ConfigurationBuilder().Build();

            _amenityRepository = new AmenityRepository(
                _context,
                _amenityValidator,
                _logger
            );
        }

        // ---------------------- SAVEASYNC ----------------------

        [Fact]
        public async Task SaveAmenity_When_Is_Null()
        {
            // Arrange
            Amenity amenity = null;

            // Act
            var result = await _amenityRepository.SaveAsync(amenity);
            string expectedMessage = "El amenity no puede ser nulo.";

            // Assert
            Assert.IsType<OperationResult<Amenity>>(result);
            Assert.Equal(expectedMessage, result.Message);
        }

        [Fact]
        public async Task SaveAsync_DeberiaGuardarAmenity_CuandoEsValido()
        {
            // Arrange
            var amenity = new Amenity { Nombre = "Piscina", Descripcion = "Piscina climatizada" };

            // Act
            var result = await _amenityRepository.SaveAsync(amenity);

            // Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal("Piscina", result.Data.Nombre);
        }

        [Fact]
        public async Task SaveAsync_DeberiaFallar_CuandoNombreEsNulo()
        {
            // Arrange
            var amenity = new Amenity { Nombre = null, Descripcion = "Área común" };

            // Act
            var result = await _amenityRepository.SaveAsync(amenity);

            // Assert
            Assert.False(result.Success);
            Assert.Contains("Nombre", result.Message);
        }

        // ---------------------- UPDATEASYNC ----------------------

        [Fact]
        public async Task UpdateAsync_DeberiaActualizarAmenity_CuandoEsValido()
        {
            // Arrange
            var amenity = new Amenity { Nombre = "Gimnasio", Descripcion = "Equipos modernos" };
            var saved = await _amenityRepository.SaveAsync(amenity);
            saved.Data.Descripcion = "Equipos nuevos y entrenador personal";

            // Act
            var result = await _amenityRepository.UpdateAsync(saved.Data);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Equipos nuevos y entrenador personal", result.Data.Descripcion);
        }

        [Fact]
        public async Task UpdateAsync_DeberiaFallar_CuandoNombreEsVacio()
        {
            // Arrange
            var amenity = new Amenity { Nombre = "Spa", Descripcion = "Sauna seca" };
            await _amenityRepository.SaveAsync(amenity);
            amenity.Nombre = "";

            // Act
            var result = await _amenityRepository.UpdateAsync(amenity);

            // Assert
            Assert.False(result.Success);
            Assert.Contains("Nombre", result.Message);
        }

        // ---------------------- DELETEASYNC ----------------------

        [Fact]
        public async Task DeleteAsync_DeberiaEliminarAmenity_CuandoExiste()
        {
            // Arrange
            var amenity = new Amenity { Nombre = "Cancha", Descripcion = "Cancha de tenis" };
            var saved = await _amenityRepository.SaveAsync(amenity);

            // Act
            var result = await _amenityRepository.DeleteAsync(saved.Data);

            // Assert
            Assert.True(result.Success);
            Assert.True(result.Data.IsDeleted);
        }

        // ---------------------- GETBYIDASYNC ----------------------

        [Fact]
        public async Task GetByIdAsync_DeberiaRetornarAmenity_CuandoExiste()
        {
            // Arrange
            var amenity = new Amenity { Nombre = "Restaurante", Descripcion = "Servicio 24 horas" };
            var saved = await _amenityRepository.SaveAsync(amenity);

            // Act
            var result = await _amenityRepository.GetByIdAsync(saved.Data.Id);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Restaurante", result.Data.Nombre);
        }

        [Fact]
        public async Task GetByIdAsync_DeberiaFallar_CuandoNoExiste()
        {
            // Arrange
            var inexistenteId = 999;

            // Act
            var result = await _amenityRepository.GetByIdAsync(inexistenteId);

            // Assert
            Assert.False(result.Success);
        }

        // ---------------------- GETALLASYNC ----------------------

        [Fact]
        public async Task GetAllAsync_DeberiaRetornarListaDeAmenities()
        {
            // Arrange
            _context.Amenity.Add(new Amenity { Nombre = "Bar", Descripcion = "Bar en la piscina" });
            _context.Amenity.Add(new Amenity { Nombre = "Jacuzzi", Descripcion = "Jacuzzi climatizado" });
            await _context.SaveChangesAsync();

            // Act
            var result = await _amenityRepository.GetAllAsync();

            // Assert
            Assert.True(result.Success);
            Assert.Equal(2, result.Data.Count);
        }

        // ---------------------- GETBYNOMBREASYNC ----------------------

        [Fact]
        public async Task GetByNombreAsync_DeberiaRetornarAmenity_CuandoExiste()
        {
            // Arrange
            var amenity = new Amenity { Nombre = "Jacuzzi", Descripcion = "Baños calientes" };
            await _amenityRepository.SaveAsync(amenity);

            // Act
            var result = await _amenityRepository.GetByNombreAsync("Jacuzzi");

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Jacuzzi", result.Data.Nombre);
        }

        [Fact]
        public async Task GetByNombreAsync_DeberiaFallar_CuandoNoExiste()
        {
            // Arrange
            var nombre = "Inexistente";

            // Act
            var result = await _amenityRepository.GetByNombreAsync(nombre);

            // Assert
            Assert.False(result.Success);
            Assert.Contains("no encontrado", result.Message, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public async Task GetByNombreAsync_DeberiaManejarExcepcion_CuandoContextoFalla()
        {
            // Arrange
            var amenity = new Amenity { Nombre = "Spa", Descripcion = "Sauna seca" };
            await _amenityRepository.SaveAsync(amenity);

            _context.Dispose(); // fuerza error de contexto

            // Act
            var result = await _amenityRepository.GetByNombreAsync("Spa");

            // Assert
            Assert.False(result.Success);
            Assert.Contains("error", result.Message, StringComparison.OrdinalIgnoreCase);
        }


    }
}
