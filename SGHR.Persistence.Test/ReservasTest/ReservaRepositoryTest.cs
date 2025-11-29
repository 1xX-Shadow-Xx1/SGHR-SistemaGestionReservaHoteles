
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SGHR.Domain.Entities.Configuration.Reservas;
using SGHR.Domain.Enum.Reservas;
using SGHR.Domain.Validators.ConfigurationRules.Reservas;
using SGHR.Persistence.Context;
using SGHR.Persistence.Repositories.EF.Reservas;

namespace SGHR.Persistence.Test.ReservasTest
{
    public class ReservaRepositoryTest
    {
        private readonly ReservaRepository _reservaRepository;
        private readonly SGHRContext _context;
        private readonly ILogger<ReservaRepository> _logger;
        private readonly ReservaValidator _reservaValidator;
        private readonly IConfiguration _configuration;

        public ReservaRepositoryTest()
        {
            _reservaValidator = new ReservaValidator();

            var options = new DbContextOptionsBuilder<SGHRContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new SGHRContext(options);

            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
            });
            _logger = loggerFactory.CreateLogger<ReservaRepository>();


            _configuration = new ConfigurationBuilder().Build();

            _reservaRepository = new ReservaRepository(
                _context,
                _reservaValidator,
                _logger
            );
        }


        // ============================================
        // SAVEASYNC TESTS
        // ============================================

        [Fact]
        public async Task SaveReserva_When_IsNull_ShouldFail()
        {
            //Arrange
            Reserva reserva = null;

            //Act
            var result = await _reservaRepository.SaveAsync(reserva);

            //Assert
            Assert.False(result.Success);
            Assert.Equal("Reserva no puede ser nulo.", result.Message);
        }

        [Fact]
        public async Task SaveReserva_When_IdClienteIsZero_ShouldFail()
        {
            //Arrange
            var reserva = new Reserva
            {
                IdCliente = 0,
                IdHabitacion = 1,
                FechaInicio = DateTime.Now,
                FechaFin = DateTime.Now.AddDays(1),
                CostoTotal = 2000,
                Estado = EstadoReserva.Pendiente
            };

            //Act
            var result = await _reservaRepository.SaveAsync(reserva);

            //Assert
            Assert.False(result.Success);
            Assert.Contains("IdCliente", result.Message);
        }

        [Fact]
        public async Task SaveReserva_When_FechaFinBeforeFechaInicio_ShouldFail()
        {
            //Arrange
            var reserva = new Reserva
            {
                IdCliente = 1,
                IdHabitacion = 2,
                FechaInicio = DateTime.Now.AddDays(5),
                FechaFin = DateTime.Now.AddDays(3),
                CostoTotal = 1500,
                Estado = EstadoReserva.Pendiente
            };

            //Act
            var result = await _reservaRepository.SaveAsync(reserva);

            //Assert
            Assert.False(result.Success);
            Assert.Equal("Fecha de fin no puede ser anterior a la fecha de inicio.", result.Message);
        }

        [Fact]
        public async Task SaveReserva_When_Valid_ShouldSucceed()
        {
            //Arrange
            var reserva = new Reserva
            {
                IdCliente = 1,
                IdHabitacion = 1,
                FechaInicio = DateTime.Now,
                FechaFin = DateTime.Now.AddDays(2),
                CostoTotal = 2500,
                Estado = EstadoReserva.Pendiente
            };

            //Act
            var result = await _reservaRepository.SaveAsync(reserva);

            //Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(1, await _context.Reservas.CountAsync());
        }

        [Fact]
        public async Task SaveReserva_When_DatabaseFails_ShouldHandleError()
        {
            //Arrange
            _context.Dispose();

            var reserva = new Reserva
            {
                IdCliente = 1,
                IdHabitacion = 1,
                FechaInicio = DateTime.Now,
                FechaFin = DateTime.Now.AddDays(1),
                CostoTotal = 3000,
                Estado = EstadoReserva.Pendiente
            };

            //Act
            var result = await _reservaRepository.SaveAsync(reserva);

            //Assert
            Assert.False(result.Success);
            Assert.Equal("Error guardando la reserva", result.Message);
        }

        // ============================================
        // UPDATEASYNC TESTS (luego de guardar una)
        // ============================================

        [Fact]
        public async Task UpdateReserva_When_Valid_ShouldSucceed()
        {
            //Arrange
            var reserva = new Reserva
            {
                IdCliente = 1,
                IdHabitacion = 2,
                FechaInicio = DateTime.Now,
                FechaFin = DateTime.Now.AddDays(2),
                CostoTotal = 2500,
                Estado = EstadoReserva.Pendiente
            };
            await _reservaRepository.SaveAsync(reserva);

            reserva.CostoTotal = 3500;

            //Act
            var result = await _reservaRepository.UpdateAsync(reserva);

            //Assert
            Assert.True(result.Success);
            Assert.Equal(3500, result.Data.CostoTotal);
        }

        [Fact]
        public async Task UpdateReserva_When_DatabaseFails_ShouldHandleError()
        {
            //Arrange
            var reserva = new Reserva
            {
                IdCliente = 2,
                IdHabitacion = 3,
                FechaInicio = DateTime.Now,
                FechaFin = DateTime.Now.AddDays(3),
                CostoTotal = 4000,
                Estado = EstadoReserva.Pendiente
            };

            await _reservaRepository.SaveAsync(reserva);
            await _context.Database.EnsureDeletedAsync();

            //Act
            var result = await _reservaRepository.UpdateAsync(reserva);

            //Assert
            Assert.False(result.Success);
            Assert.Equal("Error actualizando la reserva", result.Message);
        }

        // ============================================
        // DELETEASYNC TESTS
        // ============================================

        [Fact]
        public async Task DeleteReserva_When_Valid_ShouldSucceed()
        {
            //Arrange
            var reserva = new Reserva
            {
                IdCliente = 3,
                IdHabitacion = 5,
                FechaInicio = DateTime.Now,
                FechaFin = DateTime.Now.AddDays(1),
                CostoTotal = 1500,
                Estado = EstadoReserva.Pendiente
            };
            await _reservaRepository.SaveAsync(reserva);

            //Act
            var result = await _reservaRepository.DeleteAsync(reserva);

            //Assert
            Assert.True(result.Success);
        }

        [Fact]
        public async Task DeleteReserva_When_DatabaseFails_ShouldHandleError()
        {
            //Arrange
            var reserva = new Reserva
            {
                IdCliente = 5,
                IdHabitacion = 10,
                FechaInicio = DateTime.Now,
                FechaFin = DateTime.Now.AddDays(1),
                CostoTotal = 2500,
                Estado = EstadoReserva.Pendiente
            };

            _context.Dispose();

            //Act
            var result = await _reservaRepository.DeleteAsync(reserva);

            //Assert
            Assert.False(result.Success);
            Assert.Equal("Error eliminando la reserva", result.Message);
        }

    }
}
