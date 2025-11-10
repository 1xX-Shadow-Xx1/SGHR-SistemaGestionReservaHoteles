
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SGHR.Domain.Entities.Configuration.Habitaciones;
using SGHR.Domain.Enum.Habitaciones;
using SGHR.Domain.Validators.ConfigurationRules.Habitaciones;
using SGHR.Persistence.Context;
using SGHR.Persistence.Repositories.EF.Habitaciones;

namespace SGHR.Persistence.Test.HabitacionesTest
{
    public class HabitacionRepositoryTest
    {
        private readonly HabitacionRepository _habitacionRepository;
        private readonly SGHRContext _context;
        private readonly ILogger<HabitacionRepository> _logger;
        private readonly HabitacionValidator _habitacionValidator;
        private readonly IConfiguration _configuration;

        public HabitacionRepositoryTest()
        {
            _habitacionValidator = new HabitacionValidator();

            var options = new DbContextOptionsBuilder<SGHRContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new SGHRContext(options);

            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
            });
            _logger = loggerFactory.CreateLogger<HabitacionRepository>();


            _configuration = new ConfigurationBuilder().Build();

            _habitacionRepository = new HabitacionRepository(
                _context,
                _habitacionValidator,
                _logger
            );
        }

        // ---------------------------
        // SAVE TESTS
        // ---------------------------

        [Fact]
        public async Task SaveHabitacion_When_Is_Null()
        {
            //Arrange
            Habitacion habitacion = null;

            //Act
            var result = await _habitacionRepository.SaveAsync(habitacion);

            //Assert
            Assert.False(result.Success);
            Assert.Equal("Habitación no puede ser nulo.", result.Message);
        }

        [Fact]
        public async Task SaveHabitacion_When_IdCategoria_Is_Invalid()
        {
            //Arrange
            var habitacion = new Habitacion { IdCategoria = 0, IdPiso = 1, Numero = "101", Capacidad = 2 };

            //Act
            var result = await _habitacionRepository.SaveAsync(habitacion);

            //Assert
            Assert.False(result.Success);
            Assert.Contains("IdCategoria", result.Message);
        }

        [Fact]
        public async Task SaveHabitacion_When_IdPiso_Is_Invalid()
        {
            //Arrange
            var habitacion = new Habitacion { IdCategoria = 1, IdPiso = 0, Numero = "102", Capacidad = 2 };

            //Act
            var result = await _habitacionRepository.SaveAsync(habitacion);

            //Assert
            Assert.False(result.Success);
            Assert.Contains("IdPiso", result.Message);
        }

        [Fact]
        public async Task SaveHabitacion_When_Numero_Is_NullOrEmpty()
        {
            //Arrange
            var habitacion = new Habitacion { IdCategoria = 1, IdPiso = 1, Numero = "", Capacidad = 2 };

            //Act
            var result = await _habitacionRepository.SaveAsync(habitacion);

            //Assert
            Assert.False(result.Success);
            Assert.Contains("Número", result.Message);
        }

        [Fact]
        public async Task SaveHabitacion_When_Numero_Too_Long()
        {
            //Arrange
            var habitacion = new Habitacion
            {
                IdCategoria = 1,
                IdPiso = 1,
                Numero = new string('A', 21),
                Capacidad = 2
            };

            //Act
            var result = await _habitacionRepository.SaveAsync(habitacion);

            //Assert
            Assert.False(result.Success);
            Assert.Contains("Número", result.Message);
        }

        [Fact]
        public async Task SaveHabitacion_When_Capacidad_Is_Zero()
        {
            //Arrange
            var habitacion = new Habitacion { IdCategoria = 1, IdPiso = 1, Numero = "103", Capacidad = 0 };

            //Act
            var result = await _habitacionRepository.SaveAsync(habitacion);

            //Assert
            Assert.False(result.Success);
            Assert.Contains("Capacidad", result.Message);
        }

        [Fact]
        public async Task SaveHabitacion_When_IdAmenity_Is_Negative()
        {
            //Arrange
            var habitacion = new Habitacion { IdCategoria = 1, IdPiso = 1, IdAmenity = -1, Numero = "104", Capacidad = 2 };

            //Act
            var result = await _habitacionRepository.SaveAsync(habitacion);

            //Assert
            Assert.False(result.Success);
            Assert.Contains("IdAmenity", result.Message);
        }

        [Fact]
        public async Task SaveHabitacion_When_Estado_Is_Invalid()
        {
            //Arrange
            var habitacion = new Habitacion
            {
                IdCategoria = 1,
                IdPiso = 1,
                Numero = "105",
                Capacidad = 2,
                Estado = (EstadoHabitacion)999
            };

            //Act
            var result = await _habitacionRepository.SaveAsync(habitacion);

            //Assert
            Assert.False(result.Success);
            Assert.Contains("Estado", result.Message);
        }

        [Fact]
        public async Task SaveHabitacion_When_Valid_Should_Succeed()
        {
            //Arrange
            var habitacion = new Habitacion
            {
                IdCategoria = 1,
                IdPiso = 1,
                Numero = "106",
                Capacidad = 3,
                Estado = EstadoHabitacion.Disponible
            };

            //Act
            var result = await _habitacionRepository.SaveAsync(habitacion);

            //Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal("106", result.Data.Numero);
        }

        // ---------------------------
        // UPDATE TESTS
        // ---------------------------

        [Fact]
        public async Task UpdateHabitacion_When_Invalid_Should_Fail()
        {
            //Arrange
            var habitacion = new Habitacion { IdCategoria = 0 };

            //Act
            var result = await _habitacionRepository.UpdateAsync(habitacion);

            //Assert
            Assert.False(result.Success);
            Assert.Contains("IdCategoria", result.Message);
        }

        [Fact]
        public async Task UpdateHabitacion_When_Valid_Should_Succeed()
        {
            //Arrange
            var habitacion = new Habitacion
            {
                IdCategoria = 1,
                IdPiso = 2,
                Numero = "201",
                Capacidad = 2
            };

            var saved = await _habitacionRepository.SaveAsync(habitacion);
            saved.Data.Numero = "201-Renovada";

            //Act
            var result = await _habitacionRepository.UpdateAsync(saved.Data);

            //Assert
            Assert.True(result.Success);
            Assert.Equal("201-Renovada", result.Data.Numero);
        }

        // ---------------------------
        // DELETE TESTS
        // ---------------------------

        [Fact]
        public async Task DeleteHabitacion_Should_Succeed()
        {
            //Arrange
            var habitacion = new Habitacion { IdCategoria = 1, IdPiso = 1, Numero = "301", Capacidad = 4 };
            var saved = await _habitacionRepository.SaveAsync(habitacion);

            //Act
            var result = await _habitacionRepository.DeleteAsync(saved.Data);

            //Assert
            Assert.True(result.Success);
        }

        // ---------------------------
        // GET TESTS
        // ---------------------------

        [Fact]
        public async Task GetById_When_NotExist_Should_Fail()
        {
            //Arrange
            //Act
            var result = await _habitacionRepository.GetByIdAsync(999);

            //Assert
            Assert.False(result.Success);
        }

        [Fact]
        public async Task GetById_When_Exist_Should_Succeed()
        {
            //Arrange
            var habitacion = new Habitacion
            {
                IdCategoria = 2,
                IdPiso = 2,
                Numero = "401",
                Capacidad = 3
            };
            var saved = await _habitacionRepository.SaveAsync(habitacion);

            //Act
            var result = await _habitacionRepository.GetByIdAsync(saved.Data.Id);

            //Assert
            Assert.True(result.Success);
            Assert.Equal("401", result.Data.Numero);
        }

        [Fact]
        public async Task GetAll_Should_Return_List()
        {
            //Arrange
            await _habitacionRepository.SaveAsync(new Habitacion { IdCategoria = 1, IdPiso = 1, Numero = "501", Capacidad = 2 });
            await _habitacionRepository.SaveAsync(new Habitacion { IdCategoria = 2, IdPiso = 1, Numero = "502", Capacidad = 2 });

            //Act
            var result = await _habitacionRepository.GetAllAsync();

            //Assert
            Assert.True(result.Success);
            Assert.NotEmpty(result.Data);
        }

        // ---------------------------
        // GET BY CATEGORIA TESTS
        // ---------------------------

        [Fact]
        public async Task GetByCategoria_Should_Return_Matching_Habitaciones()
        {
            //Arrange
            await _habitacionRepository.SaveAsync(new Habitacion { IdCategoria = 10, IdPiso = 1, Numero = "601", Capacidad = 2 });
            await _habitacionRepository.SaveAsync(new Habitacion { IdCategoria = 11, IdPiso = 1, Numero = "602", Capacidad = 2 });

            //Act
            var result = await _habitacionRepository.GetByCategoriaAsync(10);

            //Assert
            Assert.True(result.Success);
            Assert.Single(result.Data);
            Assert.Equal(10, result.Data.First().IdCategoria);
        }

        // ---------------------------
        // GET BY PISO TESTS
        // ---------------------------

        [Fact]
        public async Task GetByPiso_Should_Return_Matching_Habitaciones()
        {
            //Arrange
            await _habitacionRepository.SaveAsync(new Habitacion { IdCategoria = 1, IdPiso = 20, Numero = "701", Capacidad = 2 });
            await _habitacionRepository.SaveAsync(new Habitacion { IdCategoria = 1, IdPiso = 21, Numero = "702", Capacidad = 2 });

            //Act
            var result = await _habitacionRepository.GetByPisoAsync(20);

            //Assert
            Assert.True(result.Success);
            Assert.Single(result.Data);
            Assert.Equal(20, result.Data.First().IdPiso);
        }

        // ---------------------------
        // GET AVAILABLE TESTS
        // ---------------------------

        [Fact]
        public async Task GetAvailable_Should_Return_Habitaciones_Disponibles()
        {
            //Arrange
            var habitacion1 = new Habitacion { IdCategoria = 1, IdPiso = 1, Numero = "801", Capacidad = 2, Estado = EstadoHabitacion.Disponible };
            var habitacion2 = new Habitacion { IdCategoria = 1, IdPiso = 1, Numero = "802", Capacidad = 2, Estado = EstadoHabitacion.Mantenimiento };

            await _habitacionRepository.SaveAsync(habitacion1);
            await _habitacionRepository.SaveAsync(habitacion2);

            DateTime inicio = DateTime.Now;
            DateTime fin = DateTime.Now.AddDays(1);

            //Act
            var result = await _habitacionRepository.GetAvailableAsync(inicio, fin);

            //Assert
            Assert.True(result.Success);
            Assert.All(result.Data, h => Assert.Equal(EstadoHabitacion.Disponible, h.Estado));
        }
    }
}
