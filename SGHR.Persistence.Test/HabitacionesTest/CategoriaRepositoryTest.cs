
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SGHR.Domain.Entities.Configuration.Habitaciones;
using SGHR.Domain.Validators.ConfigurationRules.Habitaciones;
using SGHR.Persistence.Context;
using SGHR.Persistence.Repositories.EF.Habitaciones;

namespace SGHR.Persistence.Test.HabitacionesTest
{
    public class CategoriaRepositoryTest
    {
        private readonly CategoriaRepository _categoriaRepository;
        private readonly SGHRContext _context;
        private readonly ILogger<CategoriaRepository> _logger;
        private readonly CategoriaValidator _categoriaValidator;
        private readonly IConfiguration _configuration;

        public CategoriaRepositoryTest()
        {
            _categoriaValidator = new CategoriaValidator();

            var options = new DbContextOptionsBuilder<SGHRContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new SGHRContext(options);

            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
            });
            _logger = loggerFactory.CreateLogger<CategoriaRepository>();


            _configuration = new ConfigurationBuilder().Build();

            _categoriaRepository = new CategoriaRepository(
                _context,
                _categoriaValidator,
                _logger
            );
        }

        // ---------------------------
        // SAVE TESTS
        // ---------------------------

        [Fact]
        public async Task SaveCategoria_When_Is_Null()
        {
            // Arrange
            Categoria categoria = null;

            // Act
            var result = await _categoriaRepository.SaveAsync(categoria);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Categoría no puede ser nulo.", result.Message);
        }

        [Fact]
        public async Task SaveCategoria_When_Nombre_Is_Empty()
        {
            // Arrange
            var categoria = new Categoria
            {
                Nombre = "",
                Descripcion = "Descripción válida"
            };

            // Act
            var result = await _categoriaRepository.SaveAsync(categoria);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Nombre es obligatorio.", result.Message);
        }

        [Fact]
        public async Task SaveCategoria_When_Descripcion_Is_Empty()
        {
            // Arrange
            var categoria = new Categoria
            {
                Nombre = "Categoría sin descripción",
                Descripcion = ""
            };

            // Act
            var result = await _categoriaRepository.SaveAsync(categoria);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Nombre es obligatorio.", result.Message); // el validador usa "Nombre" en el Required de Descripción
        }

        [Fact]
        public async Task SaveCategoria_When_Nombre_Too_Long()
        {
            // Arrange
            var categoria = new Categoria
            {
                Nombre = new string('A', 101),
                Descripcion = "Texto válido"
            };

            // Act
            var result = await _categoriaRepository.SaveAsync(categoria);

            // Assert
            Assert.False(result.Success);
            Assert.Contains("Nombre excede la longitud máxima de caracteres.", result.Message);
        }

        [Fact]
        public async Task SaveCategoria_When_Descripcion_Too_Long()
        {
            // Arrange
            var categoria = new Categoria
            {
                Nombre = "Nombre válido",
                Descripcion = new string('D', 1001)
            };

            // Act
            var result = await _categoriaRepository.SaveAsync(categoria);

            // Assert
            Assert.False(result.Success);
            Assert.Contains("Descripción excede la longitud máxima de caracteres.", result.Message);
        }

        [Fact]
        public async Task SaveCategoria_When_Valid()
        {
            // Arrange
            var categoria = new Categoria
            {
                Nombre = "Suite Premium",
                Descripcion = "Habitaciones de lujo"
            };

            // Act
            var result = await _categoriaRepository.SaveAsync(categoria);

            // Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal("Suite Premium", result.Data.Nombre);
            Assert.Equal("Habitaciones de lujo", result.Data.Descripcion);
        }

        // ---------------------------
        // UPDATE TESTS
        // ---------------------------

        [Fact]
        public async Task UpdateCategoria_When_Invalid_Descripcion()
        {
            // Arrange
            var categoria = new Categoria
            {
                Nombre = "Temporal",
                Descripcion = "Temporal"
            };

            await _categoriaRepository.SaveAsync(categoria);
            categoria.Descripcion = ""; // inválido

            // Act
            var result = await _categoriaRepository.UpdateAsync(categoria);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Nombre es obligatorio.", result.Message);
        }

        [Fact]
        public async Task UpdateCategoria_When_Valid()
        {
            // Arrange
            var categoria = new Categoria
            {
                Nombre = "Familiar",
                Descripcion = "Amplia y cómoda"
            };

            var saved = await _categoriaRepository.SaveAsync(categoria);
            saved.Data.Descripcion = "Actualizada";

            // Act
            var result = await _categoriaRepository.UpdateAsync(saved.Data);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Actualizada", result.Data.Descripcion);
        }

        // ---------------------------
        // DELETE TESTS
        // ---------------------------

        [Fact]
        public async Task DeleteCategoria_When_Valid()
        {
            // Arrange
            var categoria = new Categoria
            {
                Nombre = "Estandar",
                Descripcion = "Básica"
            };

            var saved = await _categoriaRepository.SaveAsync(categoria);

            // Act
            var result = await _categoriaRepository.DeleteAsync(saved.Data);

            // Assert
            Assert.True(result.Success);
        }

        // ---------------------------
        // GET TESTS
        // ---------------------------

        [Fact]
        public async Task GetById_When_Existing_Categoria()
        {
            // Arrange
            var categoria = new Categoria
            {
                Nombre = "Deluxe",
                Descripcion = "Con vista al mar"
            };

            var saved = await _categoriaRepository.SaveAsync(categoria);

            // Act
            var result = await _categoriaRepository.GetByIdAsync(saved.Data.Id);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Deluxe", result.Data.Nombre);
            Assert.Equal("Con vista al mar", result.Data.Descripcion);
        }

        [Fact]
        public async Task GetByNombre_When_Existing_Categoria()
        {
            // Arrange
            var categoria = new Categoria
            {
                Nombre = "Premium",
                Descripcion = "Con jacuzzi"
            };

            await _categoriaRepository.SaveAsync(categoria);

            // Act
            var result = await _categoriaRepository.GetByNombreAsync("Premium");

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Premium", result.Data.Nombre);
            Assert.Equal("Con jacuzzi", result.Data.Descripcion);
        }

        [Fact]
        public async Task GetByNombre_When_Not_Exists()
        {
            // Arrange
            var nombre = "Inexistente";

            // Act
            var result = await _categoriaRepository.GetByNombreAsync(nombre);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Categoría no encontrada", result.Message);
        }

    }
}
