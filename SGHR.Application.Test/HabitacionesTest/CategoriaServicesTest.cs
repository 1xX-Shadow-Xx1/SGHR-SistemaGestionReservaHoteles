

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SGHR.Application.Dtos.Configuration.Habitaciones.Categoria;
using SGHR.Application.Services.Habitaciones;
using SGHR.Domain.Entities.Configuration.Habitaciones;
using SGHR.Domain.Validators.ConfigurationRules.Habitaciones;
using SGHR.Persistence.Context;
using SGHR.Persistence.Repositories.EF.Habitaciones;

namespace SGHR.Application.Test.HabitacionesTest
{
    public class CategoriaServicesTest
    {
        private readonly SGHRContext _context;
        private readonly CategoriaServices _categoriaServices;
        private readonly CategoriaRepository _categoriaRepository;
        private readonly ILogger<CategoriaServices> _logger;

        public CategoriaServicesTest()
        {
            var options = new DbContextOptionsBuilder<SGHRContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new SGHRContext(options);

            var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
            _logger = loggerFactory.CreateLogger<CategoriaServices>();

            var categoriaValidator = new CategoriaValidator();
            var loggerCategoria = loggerFactory.CreateLogger<CategoriaRepository>();
            _categoriaRepository = new CategoriaRepository(_context, categoriaValidator, loggerCategoria);

            _categoriaServices = new CategoriaServices(_logger, _categoriaRepository);

            // Datos iniciales
            _categoriaRepository.SaveAsync(new Categoria { Nombre = "Estandar", Descripcion = "Categoria normal" }).Wait();
            _categoriaRepository.SaveAsync(new Categoria { Nombre = "Premium", Descripcion = "Categoria de lujo" }).Wait();
        }

        // ---------------------------
        // CREATEASYNC TESTS
        // ---------------------------

        [Fact]
        public async Task CreateAsync_When_NullDto_Returns_Error()
        {
            // Arrange
            CreateCategoriaDto dto = null;

            // Act
            var result = await _categoriaServices.CreateAsync(dto);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("La categoria no puede ser nula.", result.Message);
        }

        [Fact]
        public async Task CreateAsync_When_Name_Already_Exists_Returns_Error()
        {
            // Arrange
            var dto = new CreateCategoriaDto { Nombre = "Estandar", Descripcion = "Otra desc" };

            // Act
            var result = await _categoriaServices.CreateAsync(dto);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Ya existe una categoria registrado con ese nombre.", result.Message);
        }

        [Fact]
        public async Task CreateAsync_When_Valid_Returns_Success()
        {
            // Arrange
            var dto = new CreateCategoriaDto { Nombre = "Suite", Descripcion = "Categoria nueva" };

            // Act
            var result = await _categoriaServices.CreateAsync(dto);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Categoria creado exitosamente.", result.Message);
            var categoria = Assert.IsType<CategoriaDto>(result.Data);
            Assert.Equal("Suite", categoria.Nombre);
        }

        // ---------------------------
        // DELETEASYNC TESTS
        // ---------------------------

        [Fact]
        public async Task DeleteAsync_When_Id_Is_Invalid_Returns_Error()
        {
            // Arrange
            int id = 0;

            // Act
            var result = await _categoriaServices.DeleteAsync(id);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("El id ingresado no es valido.", result.Message);
        }

        [Fact]
        public async Task DeleteAsync_When_Not_Found_Returns_Error()
        {
            // Arrange
            int id = 999;

            // Act
            var result = await _categoriaServices.DeleteAsync(id);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("No existe una categoria con ese id.", result.Message);
        }

        [Fact]
        public async Task DeleteAsync_When_Valid_Returns_Success()
        {
            // Arrange
            var categoria = new Categoria { Nombre = "Temp", Descripcion = "Temporal" };
            await _categoriaRepository.SaveAsync(categoria);

            // Act
            var result = await _categoriaServices.DeleteAsync(categoria.Id);

            // Assert
            Assert.True(result.Success);
            Assert.Contains("eliminado correctamente", result.Message);
        }

        // ---------------------------
        // GETALLASYNC TESTS
        // ---------------------------

        [Fact]
        public async Task GetAllAsync_Returns_All_Categorias()
        {
            // Act
            var result = await _categoriaServices.GetAllAsync();

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Se obtuvieron las categorias correctamente.", result.Message);
            var list = Assert.IsAssignableFrom<System.Collections.Generic.List<CategoriaDto>>(result.Data);
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
            var validator = new CategoriaValidator();
            var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
            var repo = new CategoriaRepository(context, validator, loggerFactory.CreateLogger<CategoriaRepository>());
            var service = new CategoriaServices(loggerFactory.CreateLogger<CategoriaServices>(), repo);

            // Act
            var result = await service.GetAllAsync();

            // Assert
            Assert.True(result.Success);
            var list = Assert.IsAssignableFrom<System.Collections.Generic.List<CategoriaDto>>(result.Data);
            Assert.Empty(list);
        }

        [Fact]
        public async Task GetAllAsync_Should_Return_Success_True()
        {
            // Act
            var result = await _categoriaServices.GetAllAsync();

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
            var result = await _categoriaServices.GetByIdAsync(0);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("El id ingresado no es valido.", result.Message);
        }

        [Fact]
        public async Task GetByIdAsync_When_Not_Found_Returns_Error()
        {
            // Act
            var result = await _categoriaServices.GetByIdAsync(999);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Categoria no encontrada.", result.Message);
        }

        [Fact]
        public async Task GetByIdAsync_When_Valid_Returns_Success()
        {
            // Arrange
            var categoria = await _categoriaRepository.GetAllAsync();
            var id = categoria.Data.First().Id;

            // Act
            var result = await _categoriaServices.GetByIdAsync(id);

            // Assert
            Assert.True(result.Success);
            var data = Assert.IsType<CategoriaDto>(result.Data);
            Assert.Equal(id, data.Id);
        }

        // ---------------------------
        // UPDATEASYNC TESTS
        // ---------------------------

        [Fact]
        public async Task UpdateAsync_When_NullDto_Returns_Error()
        {
            // Arrange
            UpdateCategoriaDto dto = null;

            // Act
            var result = await _categoriaServices.UpdateAsync(dto);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("El cliente no puede ser nulo.", result.Message);
        }

        [Fact]
        public async Task UpdateAsync_When_Id_Invalid_Returns_Error()
        {
            // Arrange
            var dto = new UpdateCategoriaDto { Id = 0, Nombre = "Test", Descripcion = "Test" };

            // Act
            var result = await _categoriaServices.UpdateAsync(dto);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("El id ingresado no es valido.", result.Message);
        }

        [Fact]
        public async Task UpdateAsync_When_Valid_Returns_Success()
        {
            // Arrange
            var categoria = (await _categoriaRepository.GetAllAsync()).Data.First();
            var dto = new UpdateCategoriaDto
            {
                Id = categoria.Id,
                Nombre = "Actualizada",
                Descripcion = "Modificada"
            };

            // Act
            var result = await _categoriaServices.UpdateAsync(dto);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Categoria actualizada exitosamente.", result.Message);
            var data = Assert.IsType<CategoriaDto>(result.Data);
            Assert.Equal("Actualizada", data.Nombre);
        }
    }
}

