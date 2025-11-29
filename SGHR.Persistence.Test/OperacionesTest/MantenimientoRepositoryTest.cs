
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
    public class MantenimientoRepositoryTest
    {
        private readonly MantenimientoRepository _mantenimientoRepository;
        private readonly SGHRContext _context;
        private readonly ILogger<MantenimientoRepository> _logger;
        private readonly MantenimientoValidator _mantenimientoValidator;
        private readonly IConfiguration _configuration;

        public MantenimientoRepositoryTest()
        {
            _mantenimientoValidator = new MantenimientoValidator();

            var options = new DbContextOptionsBuilder<SGHRContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new SGHRContext(options);

            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
            });
            _logger = loggerFactory.CreateLogger<MantenimientoRepository>();


            _configuration = new ConfigurationBuilder().Build();

            _mantenimientoRepository = new MantenimientoRepository(
                _context,
                _mantenimientoValidator,
                _logger
            );
        }

        [Fact]
        public async Task SaveMantenimiento_When_IsNull_ShouldFail()
        {
            //Arrange
            Mantenimiento mantenimiento = null;

            //Act
            var result = await _mantenimientoRepository.SaveAsync(mantenimiento);

            //Assert
            Assert.False(result.Success);
            Assert.Equal("Mantenimiento no puede ser nulo.", result.Message);
        }

        [Fact]
        public async Task DeleteMantenimiento_When_Valid_ShouldSucceed()
        {
            //Arrange
            var mantenimiento = new Mantenimiento
            {
                IdPiso = 1,
                IdHabitacion = 101,
                Descripcion = "Cambio de bombillo",
                FechaInicio = DateTime.Now,
                RealizadoPor = 2,
                Estado = EstadoMantenimiento.Pendiente
            };
            var saveResult = await _mantenimientoRepository.SaveAsync(mantenimiento);

            //Act
            var deleteResult = await _mantenimientoRepository.DeleteAsync(saveResult.Data);

            //Assert
            Assert.True(deleteResult.Success);
        }

        [Fact]
        public async Task SaveMantenimiento_When_Valid_ShouldSucceed()
        {
            //Arrange
            var mantenimiento = new Mantenimiento
            {
                IdPiso = 1,
                IdHabitacion = 101,
                Descripcion = "Reparar ventana",
                FechaInicio = DateTime.Now,
                RealizadoPor = 1,
                Estado = EstadoMantenimiento.Pendiente
            };

            //Act
            var result = await _mantenimientoRepository.SaveAsync(mantenimiento);

            //Assert
            Assert.True(result.Success);
            Assert.Equal("Reparar ventana", result.Data.Descripcion);
        }

        [Fact]
        public async Task SaveMantenimiento_When_Invalid_ShouldFail()
        {
            //Arrange
            var mantenimiento = new Mantenimiento
            {
                IdPiso = -1,
                IdHabitacion = 0,
                Descripcion = new string('A', 2000), // demasiado largo
                FechaInicio = DateTime.MinValue,
                RealizadoPor = 0
            };

            //Act
            var result = await _mantenimientoRepository.SaveAsync(mantenimiento);

            //Assert
            Assert.False(result.Success);
            Assert.Equal("IdPiso inválido.", result.Message);
        }

        [Fact]
        public async Task UpdateMantenimiento_When_Valid_ShouldSucceed()
        {
            //Arrange
            var mantenimiento = new Mantenimiento
            {
                IdPiso = 1,
                IdHabitacion = 201,
                Descripcion = "Reparar puerta",
                FechaInicio = DateTime.Now,
                RealizadoPor = 2,
                Estado = EstadoMantenimiento.Pendiente
            };
            var saveResult = await _mantenimientoRepository.SaveAsync(mantenimiento);

            saveResult.Data.Descripcion = "Reparar puerta y ventana";
            saveResult.Data.Estado = EstadoMantenimiento.EnProceso;

            //Act
            var updateResult = await _mantenimientoRepository.UpdateAsync(saveResult.Data);

            //Assert
            Assert.True(updateResult.Success);
            Assert.Equal("Reparar puerta y ventana", updateResult.Data.Descripcion);
            Assert.Equal(EstadoMantenimiento.EnProceso, updateResult.Data.Estado);
        }

        [Fact]
        public async Task UpdateMantenimiento_When_Invalid_ShouldFail()
        {
            //Arrange
            var mantenimiento = new Mantenimiento
            {
                IdPiso = 1,
                IdHabitacion = 202,
                Descripcion = "Revisión luz",
                FechaInicio = DateTime.Now,
                RealizadoPor = 3,
                Estado = EstadoMantenimiento.Pendiente
            };
            var saveResult = await _mantenimientoRepository.SaveAsync(mantenimiento);

            saveResult.Data.IdHabitacion = 0;

            //Act
            var updateResult = await _mantenimientoRepository.UpdateAsync(saveResult.Data);

            //Assert
            Assert.False(updateResult.Success);
            Assert.Equal("IdHabitacion debe ser mayor a 0.", updateResult.Message);
        }

        [Fact]
        public async Task DeleteMantenimiento_ShouldSucceed()
        {
            //Arrange
            var mantenimiento = new Mantenimiento
            {
                IdPiso = 1,
                IdHabitacion = 301,
                Descripcion = "Reparar aire acondicionado",
                FechaInicio = DateTime.Now,
                RealizadoPor = 4,
                Estado = EstadoMantenimiento.Pendiente
            };
            var saveResult = await _mantenimientoRepository.SaveAsync(mantenimiento);

            //Act
            var deleteResult = await _mantenimientoRepository.DeleteAsync(saveResult.Data);

            //Assert
            Assert.True(deleteResult.Success);
        }

        [Fact]
        public async Task GetActiveMaintenances_ShouldReturnOnlyEnProceso()
        {
            //Arrange
            await _mantenimientoRepository.SaveAsync(new Mantenimiento
            {
                IdPiso = 1,
                IdHabitacion = 401,
                Descripcion = "Mantenimiento pendiente",
                FechaInicio = DateTime.Now,
                RealizadoPor = 5,
                Estado = EstadoMantenimiento.Pendiente
            });

            await _mantenimientoRepository.SaveAsync(new Mantenimiento
            {
                IdPiso = 1,
                IdHabitacion = 402,
                Descripcion = "Mantenimiento en proceso",
                FechaInicio = DateTime.Now,
                RealizadoPor = 6,
                Estado = EstadoMantenimiento.EnProceso
            });

            //Act
            var result = await _mantenimientoRepository.GetActiveMaintenancesAsync();

            //Assert
            Assert.True(result.Success);
            Assert.Single(result.Data);
            Assert.Equal(EstadoMantenimiento.EnProceso, result.Data.First().Estado);
        }

        [Fact]
        public async Task GetByHabitacion_ShouldReturnCorrectMantenimientos()
        {
            //Arrange
            await _mantenimientoRepository.SaveAsync(new Mantenimiento
            {
                IdPiso = 1,
                IdHabitacion = 501,
                Descripcion = "Mantenimiento habitación 501",
                FechaInicio = DateTime.Now,
                RealizadoPor = 7,
                Estado = EstadoMantenimiento.Pendiente
            });

            await _mantenimientoRepository.SaveAsync(new Mantenimiento
            {
                IdPiso = 1,
                IdHabitacion = 502,
                Descripcion = "Mantenimiento habitación 502",
                FechaInicio = DateTime.Now,
                RealizadoPor = 8,
                Estado = EstadoMantenimiento.Pendiente
            });

            //Act
            var result = await _mantenimientoRepository.GetByHabitacionAsync(501);

            //Assert
            Assert.True(result.Success);
            Assert.Single(result.Data);
            Assert.Equal(501, result.Data.First().IdHabitacion);
        }

        [Fact]
        public async Task GetByPiso_ShouldReturnCorrectMantenimientos()
        {
            //Arrange
            await _mantenimientoRepository.SaveAsync(new Mantenimiento
            {
                IdPiso = 2,
                IdHabitacion = 601,
                Descripcion = "Mantenimiento piso 2",
                FechaInicio = DateTime.Now,
                RealizadoPor = 9,
                Estado = EstadoMantenimiento.Pendiente
            });

            await _mantenimientoRepository.SaveAsync(new Mantenimiento
            {
                IdPiso = 3,
                IdHabitacion = 602,
                Descripcion = "Mantenimiento piso 3",
                FechaInicio = DateTime.Now,
                RealizadoPor = 10,
                Estado = EstadoMantenimiento.Pendiente
            });

            //Act
            var result = await _mantenimientoRepository.GetByPisoAsync(2);

            //Assert
            Assert.True(result.Success);
            Assert.Single(result.Data);
            Assert.Equal(2, result.Data.First().IdPiso);
        }

        [Fact]
        public async Task SaveMantenimiento_When_FechaFinBeforeFechaInicio_ShouldFail()
        {
            //Arrange
            var mantenimiento = new Mantenimiento
            {
                IdPiso = 1,
                IdHabitacion = 701,
                Descripcion = "Mantenimiento fecha incorrecta",
                FechaInicio = DateTime.Now,
                FechaFin = DateTime.Now.AddHours(-1),
                RealizadoPor = 11,
                Estado = EstadoMantenimiento.Pendiente
            };

            //Act
            var result = await _mantenimientoRepository.SaveAsync(mantenimiento);

            //Assert
            Assert.False(result.Success);
            Assert.Equal("Fecha de fin no puede ser anterior a la fecha de inicio.", result.Message);
        }

    }
}
