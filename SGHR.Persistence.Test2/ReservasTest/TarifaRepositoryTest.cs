using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SGHR.Domain.Entities.Configuration.Reservas;
using SGHR.Domain.Validators.ConfigurationRules.Reservas;
using SGHR.Persistence.Context;
using SGHR.Persistence.Repositories.EF.Reservas;

namespace SGHR.Persistence.Test.ReservasTest
{
    public class TarifaRepositoryTest
    {
        private readonly TarifaRepository _tarifaRepository;
        private readonly SGHRContext _context;
        private readonly ILogger<TarifaRepository> _logger;
        private readonly TarifaValidator _tarifaValidator;
        private readonly IConfiguration _configuration;

        public TarifaRepositoryTest()
        {
            _tarifaValidator = new TarifaValidator();

            var options = new DbContextOptionsBuilder<SGHRContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new SGHRContext(options);

            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
            });
            _logger = loggerFactory.CreateLogger<TarifaRepository>();


            _configuration = new ConfigurationBuilder().Build();

            _tarifaRepository = new TarifaRepository(
                _context,
                _tarifaValidator,
                _logger
            );
        }

        [Fact]
        public async Task SaveTarifa_WhenValid_ShouldReturnSuccess()
        {
            // Arrange
            var tarifa = new Tarifa
            {
                IdCategoria = 1,
                Temporada = "Alta",
                Precio = 1000
            };

            // Act
            var result = await _tarifaRepository.SaveAsync(tarifa);

            // Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(1, await _context.Tarifa.CountAsync());
        }

        [Fact]
        public async Task SaveTarifa_WhenInvalid_ShouldReturnFail()
        {
            // Arrange
            var tarifa = new Tarifa
            {
                IdCategoria = 0, // Inválido
                Temporada = "Alta",
                Precio = 1000
            };

            // Act
            var result = await _tarifaRepository.SaveAsync(tarifa);

            // Assert
            Assert.False(result.Success);
            Assert.Contains("IdCategoria", result.Message);
        }

        [Fact]
        public async Task UpdateTarifa_WhenValid_ShouldReturnSuccess()
        {
            // Arrange
            var tarifa = new Tarifa
            {
                IdCategoria = 1,
                Temporada = "Baja",
                Precio = 500
            };

            await _tarifaRepository.SaveAsync(tarifa);
            tarifa.Precio = 750;

            // Act
            var result = await _tarifaRepository.UpdateAsync(tarifa);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(750, result.Data.Precio);
        }

        [Fact]
        public async Task UpdateTarifa_WhenInvalid_ShouldReturnFail()
        {
            // Arrange
            var tarifa = new Tarifa
            {
                IdCategoria = 0,
                Temporada = "Alta",
                Precio = 100
            };

            // Act
            var result = await _tarifaRepository.UpdateAsync(tarifa);

            // Assert
            Assert.False(result.Success);
            Assert.Contains("IdCategoria", result.Message);
        }

        [Fact]
        public async Task DeleteTarifa_WhenExists_ShouldReturnSuccess()
        {
            // Arrange
            var tarifa = new Tarifa
            {
                IdCategoria = 1,
                Temporada = "Media",
                Precio = 800
            };

            await _tarifaRepository.SaveAsync(tarifa);

            // Act
            var result = await _tarifaRepository.DeleteAsync(tarifa);

            // Assert
            Assert.True(result.Success);
            Assert.True(tarifa.IsDeleted);
        }

        [Fact]
        public async Task GetByCategoriaAsync_WhenExists_ShouldReturnList()
        {
            // Arrange
            var tarifa = new Tarifa { IdCategoria = 5, Temporada = "Alta", Precio = 1200 };
            await _tarifaRepository.SaveAsync(tarifa);

            // Act
            var result = await _tarifaRepository.GetByCategoriaAsync(5);

            // Assert
            Assert.True(result.Success);
            Assert.Single(result.Data);
            Assert.Equal(5, result.Data[0].IdCategoria);
        }

        [Fact]
        public async Task GetByTemporadaAsync_WhenExists_ShouldReturnList()
        {
            // Arrange
            var tarifa = new Tarifa { IdCategoria = 2, Temporada = "Baja", Precio = 900 };
            await _tarifaRepository.SaveAsync(tarifa);

            // Act
            var result = await _tarifaRepository.GetByTemporadaAsync("Baja");

            // Assert
            Assert.True(result.Success);
            Assert.Single(result.Data);
            Assert.Equal("Baja", result.Data[0].Temporada);
        }

        [Fact]
        public async Task GetByCategoriaAndTemporadaAsync_WhenExists_ShouldReturnTarifa()
        {
            // Arrange
            var tarifa = new Tarifa { IdCategoria = 3, Temporada = "Alta", Precio = 1500 };
            await _tarifaRepository.SaveAsync(tarifa);

            // Act
            var result = await _tarifaRepository.GetByCategoriaAndTemporadaAsync(3, "Alta");

            // Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(3, result.Data.IdCategoria);
        }

        [Fact]
        public async Task GetByCategoriaAndTemporadaAsync_WhenNotFound_ShouldReturnFail()
        {
            // Act
            var result = await _tarifaRepository.GetByCategoriaAndTemporadaAsync(999, "NoExiste");

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Tarifa no encontrada", result.Message);
        }

    }
}
