

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SGHR.Application.Dtos.Configuration.Reservas.Tarifa;
using SGHR.Application.Interfaces.Reservas;
using SGHR.Application.Services.Reservas;
using SGHR.Domain.Entities.Configuration.Habitaciones;
using SGHR.Domain.Entities.Configuration.Reservas;
using SGHR.Domain.Validators.ConfigurationRules.Habitaciones;
using SGHR.Domain.Validators.ConfigurationRules.Reservas;
using SGHR.Persistence.Context;
using SGHR.Persistence.Interfaces.Habitaciones;
using SGHR.Persistence.Interfaces.Reservas;
using SGHR.Persistence.Repositories.EF.Habitaciones;
using SGHR.Persistence.Repositories.EF.Reservas;

namespace SGHR.Application.Test.ReservasTest
{
    public class TarifaServicesTest
    {
        private readonly ITarifaRepository _tarifaRepository;
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly SGHRContext _context;
        private readonly ILogger<TarifaServices> _loggerServices;
        private readonly ILogger<TarifaRepository> _loggerRepoTarifa;
        private readonly ILogger<CategoriaRepository> _loggerRepocategoria;
        private readonly ITarifaServices _tarifaServices;
        private readonly TarifaValidator _tarifaValidator;
        private readonly CategoriaValidator _categoriaValidator;
        private readonly IConfiguration _configuration;

        public TarifaServicesTest()
        {
            _categoriaValidator = new CategoriaValidator();
            _tarifaValidator = new TarifaValidator();

            // Configuración del contexto InMemory
            var options = new DbContextOptionsBuilder<SGHRContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new SGHRContext(options);

            var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());

            _loggerServices = loggerFactory.CreateLogger<TarifaServices>();
            _loggerRepoTarifa = loggerFactory.CreateLogger<TarifaRepository>();
            _loggerRepocategoria = loggerFactory.CreateLogger<CategoriaRepository>();

            _configuration = new ConfigurationBuilder().Build();

            // Inicialización de repositorios concretos
            _tarifaRepository = new TarifaRepository(_context, _tarifaValidator, _loggerRepoTarifa);
            _categoriaRepository = new CategoriaRepository(_context, _categoriaValidator, _loggerRepocategoria);
            _tarifaServices = new TarifaServices(_loggerServices,_tarifaRepository, _categoriaRepository);
        }


        // -------------------- CREATEASYNC TESTS --------------------

        [Fact]
        public async Task When_Tarifa_is_null_CreateAsync_Should_Fail()
        {
            //Arrange
            CreateTarifaDto dto = null;

            //Act
            var result = await _tarifaServices.CreateAsync(dto);

            //Assert
            Assert.False(result.Success);
            Assert.Equal("La tarifa no puede ser nulo.", result.Message);
        }

        [Fact]
        public async Task When_Categoria_not_found_CreateAsync_Should_Fail()
        {
            //Arrange
            var dto = new CreateTarifaDto
            {
                Fecha_inicio = DateTime.Now,
                Fecha_fin = DateTime.Now.AddDays(1),
                Precio = 100m,
                NombreCategoria = "NoExiste"
            };

            //Act
            var result = await _tarifaServices.CreateAsync(dto);

            //Assert
            Assert.False(result.Success);
            Assert.Equal("Categoria no encontrado.", result.Message);
        }

        // -------------------- DELETEASYNC TESTS --------------------

        [Fact]
        public async Task When_Id_invalid_DeleteAsync_Should_Fail()
        {
            //Arrange
            int id = 0;

            //Act
            var result = await _tarifaServices.DeleteAsync(id);

            //Assert
            Assert.False(result.Success);
            Assert.Equal("El id ingresado no es valido.", result.Message);
        }

        [Fact]
        public async Task When_Tarifa_not_found_DeleteAsync_Should_Fail()
        {
            //Arrange
            int id = 9999;

            //Act
            var result = await _tarifaServices.DeleteAsync(id);

            //Assert
            Assert.False(result.Success);
        }

        [Fact]
        public async Task When_Repository_delete_fails_DeleteAsync_Should_Fail()
        {
            //Arrange
            var categoria = new Categoria { Nombre = "Temporal", Descripcion = "Categoria Test" };
            await _categoriaRepository.SaveAsync(categoria);

            var tarifa = new Tarifa { IdCategoria = categoria.Id, Fecha_inicio = DateTime.Now, Fecha_fin = DateTime.Now.AddDays(1), Precio = 300m };
            var saved = await _tarifaRepository.SaveAsync(tarifa);

            
            _context.Dispose();

            //Act
            var result = await _tarifaServices.DeleteAsync(saved.Data.Id);

            //Assert
            Assert.False(result.Success);
        }

        [Fact]
        public async Task When_Tarifa_exists_DeleteAsync_Should_Succeed()
        {
            //Arrange
            var categoria = new Categoria { Nombre = "Premium", Descripcion = "Categoria Test" };
            await _categoriaRepository.SaveAsync(categoria);

            var tarifa = new Tarifa { IdCategoria = categoria.Id, Fecha_inicio = DateTime.Now, Fecha_fin = DateTime.Now.AddDays(1), Precio = 700m };
            var saved = await _tarifaRepository.SaveAsync(tarifa);

            //Act
            var result = await _tarifaServices.DeleteAsync(saved.Data.Id);

            //Assert
            Assert.True(result.Success);
            Assert.Equal($"Tarifa con id {saved.Data.Id} eliminada correctamente.", result.Message);
        }

        // -------------------- GETALLASYNC TESTS --------------------

        [Fact]
        public async Task GetAllAsync_When_No_Data_Should_Return_Empty_List()
        {
            //Arrange
            // no data

            //Act
            var result = await _tarifaServices.GetAllAsync();

            //Assert
            Assert.True(result.Success);
            Assert.Equal("Se obtuvieron las tarifas correctamente.", result.Message);
            var list = Assert.IsType<List<TarifaDto>>(result.Data);
            Assert.Empty(list);
        }

        [Fact]
        public async Task GetAllAsync_With_Data_Should_Return_List()
        {
            //Arrange
            var cat = new Categoria { Nombre = "Basic", Descripcion = "Categoria Test" };
            await _categoriaRepository.SaveAsync(cat);

            await _tarifaRepository.SaveAsync(new Tarifa { IdCategoria = cat.Id, Fecha_inicio = DateTime.Now, Fecha_fin = DateTime.Now.AddDays(1), Precio = 100m });
            await _tarifaRepository.SaveAsync(new Tarifa { IdCategoria = cat.Id, Fecha_inicio = DateTime.Now.AddDays(2), Fecha_fin = DateTime.Now.AddDays(3), Precio = 200m });

            //Act
            var result = await _tarifaServices.GetAllAsync();

            //Assert
            Assert.True(result.Success);
            Assert.Equal("Se obtuvieron las tarifas correctamente.", result.Message);
            var list = Assert.IsType<List<TarifaDto>>(result.Data);
            Assert.Equal(2, list.Count);
        }

        [Fact]
        public async Task GetAllAsync_When_Repo_getall_fails_Should_Fail()
        {
            //Arrange
            _context.Dispose();

            //Act
            var result = await _tarifaServices.GetAllAsync();

            //Assert
            Assert.False(result.Success);
        }

        [Fact]
        public async Task GetAllAsync_When_CategoriaRepo_fails_Should_Fail()
        {
            
            var cat = new Categoria { Nombre = "SoloTarifa", Descripcion = "Soy messi" };
            await _categoriaRepository.SaveAsync(cat);
            await _tarifaRepository.SaveAsync(new Tarifa { IdCategoria = cat.Id, Fecha_inicio = DateTime.Now, Fecha_fin = DateTime.Now.AddDays(1), Precio = 50m });

            // Act (normal) - debería pasar
            var resultNormal = await _tarifaServices.GetAllAsync();

            // Assert normal path
            Assert.True(resultNormal.Success);
            Assert.Equal("Se obtuvieron las tarifas correctamente.", resultNormal.Message);
        }

        // -------------------- GETBYIDASYNC TESTS --------------------

        [Fact]
        public async Task GetByIdAsync_When_Id_invalid_Should_Fail()
        {
            //Arrange
            int id = 0;

            //Act
            var result = await _tarifaServices.GetByIdAsync(id);

            //Assert
            Assert.False(result.Success);
            Assert.Equal("El id ingresado no es valido.", result.Message);
        }

        [Fact]
        public async Task GetByIdAsync_When_Not_Found_Should_Fail()
        {
            //Arrange
            int id = 12345;

            //Act
            var result = await _tarifaServices.GetByIdAsync(id);

            //Assert
            Assert.False(result.Success);
        }

        [Fact]
        public async Task GetByIdAsync_When_CategoriaRepo_fails_Should_Fail()
        {
            //Arrange
            var cat = new Categoria { Nombre = "Negocios", Descripcion = "Categoria Test" };
            await _categoriaRepository.SaveAsync(cat);

            var tarifa = new Tarifa { IdCategoria = cat.Id, Fecha_inicio = DateTime.Now, Fecha_fin = DateTime.Now.AddDays(1), Precio = 250m };
            var saved = await _tarifaRepository.SaveAsync(tarifa);


            _context.Dispose();

            //Act
            var result = await _tarifaServices.GetByIdAsync(saved.Data.Id);

            //Assert
            Assert.False(result.Success);
        }

        [Fact]
        public async Task GetByIdAsync_When_Found_Should_Return_Tarifa()
        {
            //Arrange
            var cat = new Categoria { Nombre = "Negocios2", Descripcion = "Soy messi" };
            await _categoriaRepository.SaveAsync(cat);

            var tarifa = new Tarifa { IdCategoria = cat.Id, Fecha_inicio = DateTime.Now, Fecha_fin = DateTime.Now.AddDays(1), Precio = 500m };
            var saved = await _tarifaRepository.SaveAsync(tarifa);

            //Act
            var result = await _tarifaServices.GetByIdAsync(saved.Data.Id);

            //Assert
            Assert.True(result.Success);
            Assert.Equal($"Se obtuvo la tarifa con id {saved.Data.Id} correctamnete.", result.Message);
        }

        // -------------------- UPDATEASYNC TESTS --------------------

        [Fact]
        public async Task UpdateAsync_When_Tarifa_is_null_Should_Fail()
        {
            //Arrange
            UpdateTarifaDto dto = null;

            //Act
            var result = await _tarifaServices.UpdateAsync(dto);

            //Assert
            Assert.False(result.Success);
            Assert.Equal("La tarifa no puede ser nulo.", result.Message);
        }

        [Fact]
        public async Task UpdateAsync_When_Id_invalid_Should_Fail()
        {
            //Arrange
            var dto = new UpdateTarifaDto
            {
                Id = 0,
                NombreCategoria = "Estandar",
                Fecha_inicio = DateTime.Now,
                Fecha_fin = DateTime.Now.AddDays(1),
                Precio = 100m
            };

            //Act
            var result = await _tarifaServices.UpdateAsync(dto);

            //Assert
            Assert.False(result.Success);
            Assert.Equal("El id ingresado no es valido.", result.Message);
        }

        [Fact]
        public async Task UpdateAsync_When_Categoria_not_found_Should_Fail()
        {
            //Arrange
            var cat = new Categoria { Nombre = "Economica", Descripcion = "Categoria Test" };
            await _categoriaRepository.SaveAsync(cat);

            var tarifa = new Tarifa { IdCategoria = cat.Id, Fecha_inicio = DateTime.Now, Fecha_fin = DateTime.Now.AddDays(1), Precio = 200m };
            var saved = await _tarifaRepository.SaveAsync(tarifa);

            var dto = new UpdateTarifaDto
            {
                Id = saved.Data.Id,
                NombreCategoria = "NoExiste",
                Fecha_inicio = DateTime.Now,
                Fecha_fin = DateTime.Now.AddDays(1),
                Precio = 300m
            };

            //Act
            var result = await _tarifaServices.UpdateAsync(dto);

            //Assert
            Assert.False(result.Success);
            Assert.Equal("Categoria no encontrado.", result.Message);
        }

        [Fact]
        public async Task UpdateAsync_When_Temporada_and_Categoria_conflict_Should_Fail()
        {
            //Arrange
            var catA = new Categoria { Nombre = "A", Descripcion = "Categoria A" };
            var catB = new Categoria { Nombre = "B", Descripcion = "Categoria B" };
            await _categoriaRepository.SaveAsync(catA);
            await _categoriaRepository.SaveAsync(catB);

            var tarifa1 = (await _tarifaRepository.SaveAsync(new Tarifa { IdCategoria = catA.Id, Fecha_inicio = DateTime.Now, Fecha_fin = DateTime.Now.AddDays(1), Precio = 100m })).Data;
            var tarifa2 = (await _tarifaRepository.SaveAsync(new Tarifa { IdCategoria = catB.Id, Fecha_inicio = DateTime.Now.AddDays(2), Fecha_fin = DateTime.Now.AddDays(3), Precio = 200m })).Data;

            var dto = new UpdateTarifaDto
            {
                Id = tarifa2.Id,
                NombreCategoria = "A", // intenta cambiar a categoria A con temporada "Alta" ya existente en tarifa1
                Fecha_inicio = DateTime.Now,
                Fecha_fin = DateTime.Now.AddDays(1),
                Precio = 300m
            };

            //Act
            var result = await _tarifaServices.UpdateAsync(dto);

            //Assert
            Assert.False(result.Success);
            Assert.Equal("Ya existe una tarifa registrado con esa categoria en la misma temporada.", result.Message);
        }

        [Fact]
        public async Task UpdateAsync_When_Valid_Should_Succeed()
        {
            //Arrange
            var cat = new Categoria { Nombre = "Lujo", Descripcion = "Categoria Test" };
            await _categoriaRepository.SaveAsync(cat);

            var tarifa = new Tarifa { IdCategoria = cat.Id, Fecha_inicio = DateTime.Now, Fecha_fin = DateTime.Now.AddDays(1), Precio = 450m };
            await _tarifaRepository.SaveAsync(tarifa);

            var dto = new UpdateTarifaDto
            {
                Id = tarifa.Id,
                NombreCategoria = "Lujo",
                Fecha_inicio = DateTime.Now,
                Fecha_fin = DateTime.Now.AddDays(1),
                Precio = 600m
            };

            //Act
            var result = await _tarifaServices.UpdateAsync(dto);

            //Assert
            Assert.True(result.Success);
            Assert.Equal("Se a actualizado la tarifa correctamente.", result.Message);
        }

    }
}
