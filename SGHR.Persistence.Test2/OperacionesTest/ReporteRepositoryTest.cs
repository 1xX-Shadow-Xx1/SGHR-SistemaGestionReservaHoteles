
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SGHR.Domain.Entities.Configuration.Reportes;
using SGHR.Domain.Validators.ConfigurationRules.Operaciones;
using SGHR.Persistence.Context;
using SGHR.Persistence.Repositories.EF.Operaciones;

namespace SGHR.Persistence.Test.OperacionesTest
{
    public class ReporteRepositoryTest
    {
        private readonly ReporteRepository _reporteRepository;
        private readonly SGHRContext _context;
        private readonly ILogger<ReporteRepository> _logger;
        private readonly ReporteValidator _reporteValidator;
        private readonly IConfiguration _configuration;

        public ReporteRepositoryTest()
        {
            _reporteValidator = new ReporteValidator();

            var options = new DbContextOptionsBuilder<SGHRContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new SGHRContext(options);

            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
            });
            _logger = loggerFactory.CreateLogger<ReporteRepository>();


            _configuration = new ConfigurationBuilder().Build();

            _reporteRepository = new ReporteRepository(
                _context,
                _reporteValidator,
                _logger
            );
        }

        [Fact]
        public async Task SaveReporte_When_IsNull_ShouldFail()
        {
            //Arrange
            Reporte reporte = null;

            //Act
            var result = await _reporteRepository.SaveAsync(reporte);

            //Assert
            Assert.False(result.Success);
            Assert.Equal("Reporte no puede ser nulo.", result.Message);
        }

        [Fact]
        public async Task SaveReporte_When_Valid_ShouldSucceed()
        {
            //Arrange
            var reporte = new Reporte
            {
                TipoReporte = "Ventas",
                FechaGeneracion = DateTime.Now,
                GeneradoPor = 1,
                RutaArchivo = "reporte_ventas.pdf"
            };

            //Act
            var result = await _reporteRepository.SaveAsync(reporte);

            //Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal("Ventas", result.Data.TipoReporte);
        }

        [Fact]
        public async Task SaveReporte_When_InvalidTipoReporte_ShouldFail()
        {
            //Arrange
            var reporte = new Reporte
            {
                TipoReporte = "",
                FechaGeneracion = DateTime.Now,
                GeneradoPor = 1,
                RutaArchivo = "reporte_ventas.pdf"
            };

            //Act
            var result = await _reporteRepository.SaveAsync(reporte);

            //Assert
            Assert.False(result.Success);
            Assert.Equal("TipoReporte es obligatorio.", result.Message);
        }

        [Fact]
        public async Task UpdateReporte_When_Valid_ShouldSucceed()
        {
            //Arrange
            var reporte = new Reporte
            {
                TipoReporte = "Ingresos",
                FechaGeneracion = DateTime.Now,
                GeneradoPor = 2,
                RutaArchivo = "ingresos.pdf"
            };
            await _context.Reportes.AddAsync(reporte);
            await _context.SaveChangesAsync();

            reporte.TipoReporte = "IngresosActualizados";

            //Act
            var result = await _reporteRepository.UpdateAsync(reporte);

            //Assert
            Assert.True(result.Success);
            Assert.Equal("IngresosActualizados", result.Data.TipoReporte);
        }

        [Fact]
        public async Task UpdateReporte_When_Invalid_ShouldFail()
        {
            //Arrange
            var reporte = new Reporte
            {
                TipoReporte = "",
                FechaGeneracion = DateTime.Now,
                GeneradoPor = 1
            };

            //Act
            var result = await _reporteRepository.UpdateAsync(reporte);

            //Assert
            Assert.False(result.Success);
            Assert.Equal("TipoReporte es obligatorio.", result.Message);
        }

        [Fact]
        public async Task DeleteReporte_When_Valid_ShouldSucceed()
        {
            //Arrange
            var reporte = new Reporte
            {
                TipoReporte = "Prueba",
                FechaGeneracion = DateTime.Now,
                GeneradoPor = 1,
                RutaArchivo = "prueba.pdf"
            };
            await _context.Reportes.AddAsync(reporte);
            await _context.SaveChangesAsync();

            //Act
            var result = await _reporteRepository.DeleteAsync(reporte);

            //Assert
            Assert.True(result.Success);
        }

        [Fact]
        public async Task DeleteReporte_When_DatabaseFails_ShouldHandleError()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();
            var reporte = new Reporte
            {
                TipoReporte = "Error",
                FechaGeneracion = DateTime.Now,
                GeneradoPor = 1
            };

            //Act
            var result = await _reporteRepository.DeleteAsync(reporte);

            //Assert
            Assert.False(result.Success);
            Assert.Equal("Error eliminando el reporte", result.Message);
        }

        [Fact]
        public async Task GetByFechaAsync_When_Valid_ShouldReturnReportes()
        {
            //Arrange
            var reporte1 = new Reporte
            {
                TipoReporte = "Ventas",
                FechaGeneracion = DateTime.Now.AddDays(-1),
                GeneradoPor = 1,
                RutaArchivo = "ventas.pdf"
            };
            var reporte2 = new Reporte
            {
                TipoReporte = "Compras",
                FechaGeneracion = DateTime.Now,
                GeneradoPor = 1,
                RutaArchivo = "compras.pdf"
            };

            await _context.Reportes.AddRangeAsync(reporte1, reporte2);
            await _context.SaveChangesAsync();

            var fechaInicio = DateTime.Now.AddDays(-2);
            var fechaFin = DateTime.Now.AddDays(1);

            //Act
            var result = await _reporteRepository.GetByFechaAsync(fechaInicio, fechaFin);

            //Assert
            Assert.True(result.Success);
            Assert.Equal(2, result.Data.Count);
        }

        [Fact]
        public async Task GetByTipoAsync_When_Valid_ShouldReturnReportes()
        {
            //Arrange
            var reporte = new Reporte
            {
                TipoReporte = "Finanzas",
                FechaGeneracion = DateTime.Now,
                GeneradoPor = 1,
                RutaArchivo = "finanzas.pdf"
            };
            await _context.Reportes.AddAsync(reporte);
            await _context.SaveChangesAsync();

            //Act
            var result = await _reporteRepository.GetByTipoAsync("Finanzas");

            //Assert
            Assert.True(result.Success);
            Assert.Single(result.Data);
        }

        [Fact]
        public async Task GetByUsuarioAsync_When_Valid_ShouldReturnReportes()
        {
            //Arrange
            var reporte = new Reporte
            {
                TipoReporte = "Operaciones",
                FechaGeneracion = DateTime.Now,
                GeneradoPor = 99,
                RutaArchivo = "ops.pdf"
            };
            await _context.Reportes.AddAsync(reporte);
            await _context.SaveChangesAsync();

            //Act
            var result = await _reporteRepository.GetByUsuarioAsync(99);

            //Assert
            Assert.True(result.Success);
            Assert.Single(result.Data);
            Assert.Equal("Operaciones", result.Data.First().TipoReporte);
        }

    }
}
