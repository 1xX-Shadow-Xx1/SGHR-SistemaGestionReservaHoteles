
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SGHR.Domain.Base;
using SGHR.Domain.Entities.Configuration.Habitaciones;
using SGHR.Domain.Enum.Habitaciones;
using SGHR.Domain.Validators.ConfigurationRules.Habitaciones;
using SGHR.Persistence.Context;
using SGHR.Persistence.Repositories.EF.Habitaciones;

namespace SGHR.Persistence.Test.HabitacionesTest
{
    public class PisoRepositoryTest
    {
        private readonly PisoRepository _pisoRepository;
        private readonly SGHRContext _context;
        private readonly ILogger<PisoRepository> _logger;
        private readonly PisoValidator _pisoValidator;
        private readonly IConfiguration _configuration;

        public PisoRepositoryTest()
        {
            _pisoValidator = new PisoValidator();

            var options = new DbContextOptionsBuilder<SGHRContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new SGHRContext(options);

            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
            });
            _logger = loggerFactory.CreateLogger<PisoRepository>();


            _configuration = new ConfigurationBuilder().Build();

            _pisoRepository = new PisoRepository(
                _context,
                _pisoValidator,
                _logger
            );
        }

        // ---------------------------
        // SAVE TESTS
        // ---------------------------

        [Fact]
        public async Task SavePiso_When_Is_Null()
        {
            // Arrange
            Piso piso = null;

            // Act
            var result = await _pisoRepository.SaveAsync(piso);
            string expectedMessage = "Piso no puede ser nulo.";

            // Assert
            Assert.IsType<OperationResult<Piso>>(result);
            Assert.Equal(expectedMessage, result.Message);
        }

        [Fact]
        public async Task SavePiso_When_NumeroPiso_Is_Zero()
        {
            // Arrange
            var piso = new Piso
            {
                NumeroPiso = 0,
                Descripcion = "Primer piso",
                Estado = EstadoPiso.Habilitado
            };

            // Act
            var result = await _pisoRepository.SaveAsync(piso);

            // Assert
            Assert.False(result.Success);
            Assert.Contains("Número de Piso", result.Message);
        }

        [Fact]
        public async Task SavePiso_When_Valid_Should_Save_Successfully()
        {
            // Arrange
            var piso = new Piso
            {
                NumeroPiso = 1,
                Descripcion = "Piso de recepción",
                Estado = EstadoPiso.Habilitado
            };

            // Act
            var result = await _pisoRepository.SaveAsync(piso);

            // Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(1, result.Data.NumeroPiso);
        }

        // ---------------------------
        // UPDATE TESTS
        // ---------------------------

        [Fact]
        public async Task UpdatePiso_When_Invalid_Should_Fail()
        {
            // Arrange
            var piso = new Piso { NumeroPiso = 0 }; // Inválido

            // Act
            var result = await _pisoRepository.UpdateAsync(piso);

            // Assert
            Assert.False(result.Success);
            Assert.Contains("Número de Piso", result.Message);
        }

        [Fact]
        public async Task UpdatePiso_When_Valid_Should_Succeed()
        {
            // Arrange
            var piso = new Piso { NumeroPiso = 2, Descripcion = "Piso 2" };
            await _pisoRepository.SaveAsync(piso);
            piso.Descripcion = "Piso 2 Renovado";

            // Act
            var result = await _pisoRepository.UpdateAsync(piso);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Piso 2 Renovado", result.Data.Descripcion);
        }

        // ---------------------------
        // GET TESTS
        // ---------------------------

        [Fact]
        public async Task GetById_When_Piso_Not_Exist_Should_Fail()
        {
            // Act
            var result = await _pisoRepository.GetByIdAsync(999);

            // Assert
            Assert.False(result.Success);
        }

        [Fact]
        public async Task GetById_When_Piso_Exist_Should_Succeed()
        {
            // Arrange
            var piso = new Piso { NumeroPiso = 3, Descripcion = "Tercer piso" };
            var saved = await _pisoRepository.SaveAsync(piso);

            // Act
            var result = await _pisoRepository.GetByIdAsync(saved.Data.Id);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(3, result.Data.NumeroPiso);
        }

        [Fact]
        public async Task GetAll_Should_Return_List()
        {
            // Arrange
            await _pisoRepository.SaveAsync(new Piso { NumeroPiso = 4, Descripcion = "Test 4" });
            await _pisoRepository.SaveAsync(new Piso { NumeroPiso = 5, Descripcion = "Test 5" });

            // Act
            var result = await _pisoRepository.GetAllAsync();

            // Assert
            Assert.True(result.Success);
            Assert.NotEmpty(result.Data);
        }

        // ---------------------------
        // DELETE TESTS
        // ---------------------------

        [Fact]
        public async Task DeletePiso_Should_Succeed()
        {
            // Arrange
            var piso = new Piso { NumeroPiso = 6 , Descripcion = "Test 6"};
            var saved = await _pisoRepository.SaveAsync(piso);

            // Act
            var result = await _pisoRepository.DeleteAsync(saved.Data);

            // Assert
            Assert.True(result.Success);
        }

        // ---------------------------
        // GET BY NUMERO TESTS
        // ---------------------------

        [Fact]
        public async Task GetByNumeroPiso_When_Exist_Should_Succeed()
        {
            // Arrange
            var piso = new Piso { NumeroPiso = 7, Descripcion = "Piso especial" };
            await _pisoRepository.SaveAsync(piso);

            // Act
            var result = await _pisoRepository.GetByNumeroPisoAsync(7);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(7, result.Data.NumeroPiso);
        }

        [Fact]
        public async Task GetByNumeroPiso_When_Not_Exist_Should_Fail()
        {
            // Act
            var result = await _pisoRepository.GetByNumeroPisoAsync(999);

            // Assert
            Assert.False(result.Success);
            Assert.Contains("No se encontró ningún piso", result.Message);
        }

    }
}
