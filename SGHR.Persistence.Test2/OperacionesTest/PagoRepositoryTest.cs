using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SGHR.Domain.Entities.Configuration.Operaciones;
using SGHR.Domain.Enum.Operaciones;
using SGHR.Domain.Validators.ConfigurationRules.Operaciones;
using SGHR.Persistence.Context;
using SGHR.Persistence.Repositories.EF.Operaciones;

namespace SGHR.Persistence.Test.OperacionesTest
{
    public class PagoRepositoryTest
    {
        private readonly PagoRepository _pagoRepository;
        private readonly SGHRContext _context;
        private readonly ILogger<PagoRepository> _logger;
        private readonly PagoValidator _pagoValidator;
        private readonly IConfiguration _configuration;

        public PagoRepositoryTest()
        {
            _pagoValidator = new PagoValidator();

            var options = new DbContextOptionsBuilder<SGHRContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new SGHRContext(options);

            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
            });
            _logger = loggerFactory.CreateLogger<PagoRepository>();


            _configuration = new ConfigurationBuilder().Build();

            _pagoRepository = new PagoRepository(
                _context,
                _pagoValidator,
                _logger
            );
        }

        //================================================
        // Save Tests
        //================================================
        [Fact]
        public async Task SavePago_WhenIsNull_ShouldFail()
        {
            //Arrange
            Pago pago = null;

            //Act
            var result = await _pagoRepository.SaveAsync(pago);

            //Assert
            Assert.False(result.Success);
            Assert.Equal("Pago no puede ser nulo.", result.Message);
        }

        [Fact]
        public async Task SavePago_WhenInvalid_ShouldFail()
        {
            //Arrange
            var pago = new Pago
            {
                IdReserva = 0,
                Monto = 0,
                MetodoPago = "",
                FechaPago = DateTime.MinValue
            };

            //Act
            var result = await _pagoRepository.SaveAsync(pago);

            //Assert
            Assert.False(result.Success);
            Assert.NotEmpty(result.Message);
        }

        [Fact]
        public async Task SavePago_WhenValid_ShouldSucceed()
        {
            //Arrange
            var pago = new Pago
            {
                IdReserva = 1,
                Monto = 500,
                MetodoPago = "Tarjeta",
                FechaPago = DateTime.Now,
                Estado = EstadoPago.Pendiente
            };

            //Act
            var result = await _pagoRepository.SaveAsync(pago);

            //Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(500, result.Data.Monto);
        }

        //================================================
        // Update Tests
        //================================================
        [Fact]
        public async Task UpdatePago_WhenInvalid_ShouldFail()
        {
            //Arrange
            var pago = new Pago
            {
                IdReserva = 0,
                Monto = 0,
                MetodoPago = "",
                FechaPago = DateTime.MinValue
            };

            //Act
            var result = await _pagoRepository.UpdateAsync(pago);

            //Assert
            Assert.False(result.Success);
            Assert.NotEmpty(result.Message);
        }

        [Fact]
        public async Task UpdatePago_WhenNotSaved_ShouldFail()
        {
            //Arrange
            var pago = new Pago
            {
                Id = 999, // No existe en la DB
                IdReserva = 1,
                Monto = 100,
                MetodoPago = "Efectivo",
                FechaPago = DateTime.Now
            };

            //Act
            var result = await _pagoRepository.UpdateAsync(pago);

            //Assert
            Assert.False(result.Success);
        }

        [Fact]
        public async Task UpdatePago_WhenValid_ShouldSucceed()
        {
            //Arrange
            var pago = new Pago
            {
                IdReserva = 1,
                Monto = 100,
                MetodoPago = "Efectivo",
                FechaPago = DateTime.Now
            };
            var saveResult = await _pagoRepository.SaveAsync(pago);

            //Act
            saveResult.Data.Monto = 150;
            var updateResult = await _pagoRepository.UpdateAsync(saveResult.Data);

            //Assert
            Assert.True(updateResult.Success);
            Assert.Equal(150, updateResult.Data.Monto);
        }

        //================================================
        // Delete Tests
        //================================================
        [Fact]
        public async Task DeletePago_WhenNotSaved_ShouldFail()
        {
            //Arrange
            var pago = new Pago { Id = 999, IdReserva = 1, Monto = 100, MetodoPago = "Efectivo" };

            //Act
            var result = await _pagoRepository.DeleteAsync(pago);

            //Assert
            Assert.False(result.Success);
            Assert.Equal("Error al eliminar el pago.", result.Message);
        }

        [Fact]
        public async Task DeletePago_WhenSaved_ShouldSucceed()
        {
            //Arrange
            var pago = new Pago { IdReserva = 1, Monto = 100, MetodoPago = "Efectivo" };
            var saveResult = await _pagoRepository.SaveAsync(pago);

            //Act
            var result = await _pagoRepository.DeleteAsync(saveResult.Data);

            //Assert
            Assert.True(result.Success);
        }

        //================================================
        // GetById Tests
        //================================================
        [Fact]
        public async Task GetById_WhenNotExist_ShouldFail()
        {
            //Act
            var result = await _pagoRepository.GetByIdAsync(999);

            //Assert
            Assert.False(result.Success);
        }

        [Fact]
        public async Task GetById_WhenExist_ShouldSucceed()
        {
            //Arrange
            var pago = new Pago { IdReserva = 1, Monto = 200, MetodoPago = "Efectivo" };
            var saveResult = await _pagoRepository.SaveAsync(pago);

            //Act
            var result = await _pagoRepository.GetByIdAsync(saveResult.Data.Id);

            //Assert
            Assert.True(result.Success);
            Assert.Equal(200, result.Data.Monto);
        }

        //================================================
        // GetAll Tests
        //================================================
        [Fact]
        public async Task GetAll_WhenEmpty_ShouldReturnEmptyList()
        {
            //Act
            var result = await _pagoRepository.GetAllAsync();

            //Assert
            Assert.True(result.Success);
            Assert.Empty(result.Data);
        }

        [Fact]
        public async Task GetAll_WhenHaveData_ShouldReturnAll()
        {
            //Arrange
            await _pagoRepository.SaveAsync(new Pago { IdReserva = 1, Monto = 100, MetodoPago = "Efectivo" });
            await _pagoRepository.SaveAsync(new Pago { IdReserva = 2, Monto = 200, MetodoPago = "Tarjeta" });

            //Act
            var result = await _pagoRepository.GetAllAsync();

            //Assert
            Assert.True(result.Success);
            Assert.Equal(2, result.Data.Count);
        }

        //================================================
        // GetByReserva Tests
        //================================================
        [Fact]
        public async Task GetByReserva_WhenExist_ShouldReturnList()
        {
            //Arrange
            await _pagoRepository.SaveAsync(new Pago { IdReserva = 10, Monto = 100, MetodoPago = "Efectivo" });

            //Act
            var result = await _pagoRepository.GetByReservaAsync(10);

            //Assert
            Assert.True(result.Success);
            Assert.Single(result.Data);
        }

        [Fact]
        public async Task GetByReserva_WhenNotExist_ShouldReturnEmptyList()
        {
            //Act
            var result = await _pagoRepository.GetByReservaAsync(999);

            //Assert
            Assert.True(result.Success);
            Assert.Empty(result.Data);
        }

        //================================================
        // GetByFecha Tests
        //================================================
        [Fact]
        public async Task GetByFecha_WhenExist_ShouldReturnList()
        {
            //Arrange
            var today = DateTime.Today;
            await _pagoRepository.SaveAsync(new Pago { IdReserva = 1, Monto = 100, MetodoPago = "Efectivo", FechaPago = today });

            //Act
            var result = await _pagoRepository.GetByFechaAsync(today);

            //Assert
            Assert.True(result.Success);
            Assert.Single(result.Data);
        }

        [Fact]
        public async Task GetByFecha_WhenNotExist_ShouldReturnEmptyList()
        {
            //Act
            var result = await _pagoRepository.GetByFechaAsync(DateTime.Today.AddDays(-1));

            //Assert
            Assert.True(result.Success);
            Assert.Empty(result.Data);
        }

    }
}
