

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SGHR.Application.Dtos.Configuration.Operaciones.Pago;
using SGHR.Application.Services.Operaciones;
using SGHR.Domain.Entities.Configuration.Operaciones;
using SGHR.Domain.Entities.Configuration.Reservas;
using SGHR.Domain.Enum.Operaciones;
using SGHR.Domain.Enum.Reservas;
using SGHR.Domain.Enum.Usuarios;
using SGHR.Domain.Validators.ConfigurationRules.Habitaciones;
using SGHR.Domain.Validators.ConfigurationRules.Operaciones;
using SGHR.Domain.Validators.ConfigurationRules.Reservas;
using SGHR.Domain.Validators.ConfigurationRules.Users;
using SGHR.Persistence.Context;
using SGHR.Persistence.Repositories.EF.Habitaciones;
using SGHR.Persistence.Repositories.EF.Operaciones;
using SGHR.Persistence.Repositories.EF.Reservas;
using SGHR.Persistence.Repositories.EF.Users;


namespace SGHR.Application.Test.OperacionesTest
{
    public class PagoServicesTest
    {
        private readonly SGHRContext _context;
        private readonly PagoServices _pagoServices;
        private readonly PagoRepository _pagoRepository;
        private readonly ReservaRepository _reservaRepository;
        private readonly HabitacionRepository _habitacionRepository;
        private readonly UsuarioRepository _usuarioRepository;
        private readonly ClienteRepository _clienteRepository;
        private readonly CategoriaRepository _categoriaRepository;
        private readonly PisoRepository _pisoRepository;

        private readonly ILogger<PagoServices> _logger;

        public PagoServicesTest()
        {
            var options = new DbContextOptionsBuilder<SGHRContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new SGHRContext(options);

            var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
            _logger = loggerFactory.CreateLogger<PagoServices>();

            var pagoValidator = new PagoValidator();
            var reservaValidator = new ReservaValidator();
            var habitacionValidator = new HabitacionValidator();
            var usuarioValidator = new UsuarioValidator();
            var clienteValidator = new ClienteValidator();
            var pisoValidator = new PisoValidator();
            var categoriaValidator = new CategoriaValidator();

            var loggerPago = loggerFactory.CreateLogger<PagoRepository>();
            var loggerReserva = loggerFactory.CreateLogger<ReservaRepository>();
            var loggerHabitacion = loggerFactory.CreateLogger<HabitacionRepository>();
            var loggerUsuario = loggerFactory.CreateLogger<UsuarioRepository>();
            var loggerCliente = loggerFactory.CreateLogger<ClienteRepository>();
            var loggerCategoria = loggerFactory.CreateLogger<CategoriaRepository>();
            var loggerPiso = loggerFactory.CreateLogger<PisoRepository>();

            _pagoRepository = new PagoRepository(_context, pagoValidator, loggerPago);
            _reservaRepository = new ReservaRepository(_context, reservaValidator, loggerReserva);
            _habitacionRepository = new HabitacionRepository(_context, habitacionValidator, loggerHabitacion);
            _usuarioRepository = new UsuarioRepository(_context, usuarioValidator, loggerUsuario);
            _clienteRepository = new ClienteRepository(_context, clienteValidator, loggerCliente);
            _categoriaRepository = new CategoriaRepository(_context, categoriaValidator, loggerCategoria);
            _pisoRepository = new PisoRepository(_context, pisoValidator, loggerPiso);

            _pagoServices = new PagoServices(
                _logger,
                _pagoRepository,
                _reservaRepository,
                _habitacionRepository,
                _usuarioRepository,
                _clienteRepository
            );
        }

        // -------------------------------------------------------------
        // TESTS PARA RealizarPagoAsync
        // -------------------------------------------------------------
        [Fact]
        public async Task RealizarPagoAsync_When_Null_Should_Return_ErrorMessage()
        {
            // Arrange
            RealizarPagoDto dto = null;

            // Act
            var result = await _pagoServices.RealizarPagoAsync(dto);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("El pago no puede ser nulo.", result.Message);
        }

        [Fact]
        public async Task RealizarPagoAsync_When_Monto_Is_Less_Than_Zero_Should_Fail()
        {
            // Arrange
            var dto = new RealizarPagoDto { IdReserva = 1, Monto = -5 };

            // Act
            var result = await _pagoServices.RealizarPagoAsync(dto);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("El monto del pago debe ser mayor a cero.", result.Message);
        }

        [Fact]
        public async Task RealizarPagoAsync_When_Reserva_Not_Found_Should_Return_Error()
        {
            // Arrange
            var dto = new RealizarPagoDto { IdReserva = 999, Monto = 100, MetodoPago = "Efectivo" };

            // Act
            var result = await _pagoServices.RealizarPagoAsync(dto);

            // Assert
            Assert.False(result.Success);
            Assert.Contains("No se encontró la reserva", result.Message);
        }

        // -------------------------------------------------------------
        // TESTS PARA ObtenerPagosAsync
        // -------------------------------------------------------------

        [Fact]
        public async Task ObtenerPagosAsync_When_Has_Data_Should_Return_List()
        {
            // Arrange
            var pago = new Pago { IdReserva = 1, Monto = 500, MetodoPago = "Tarjeta", FechaPago = DateTime.Now, Estado = EstadoPago.Completado };
            await _pagoRepository.SaveAsync(pago);

            // Act
            var result = await _pagoServices.ObtenerPagosAsync();

            // Assert
            Assert.True(result.Success);
            var lista = Assert.IsAssignableFrom<IEnumerable<PagoDto>>(result.Data);
            Assert.Single(lista);
        }

        [Fact]
        public async Task ObtenerPagosAsync_Should_Return_Ordered_By_FechaPago()
        {
            // Arrange
            await _pagoRepository.SaveAsync(new Pago { IdReserva = 1, Monto = 200, MetodoPago = "Efectivo", FechaPago = DateTime.Now.AddDays(-2), Estado = EstadoPago.Parcial });
            await _pagoRepository.SaveAsync(new Pago { IdReserva = 2, Monto = 400, MetodoPago = "Tarjeta", FechaPago = DateTime.Now, Estado = EstadoPago.Completado });

            // Act
            var result = await _pagoServices.ObtenerPagosAsync();

            // Assert
            Assert.True(result.Success);
            var lista = (IEnumerable<PagoDto>)result.Data;
            Assert.True(lista.First().FechaPago >= lista.Last().FechaPago);
        }

        // -------------------------------------------------------------
        // TESTS PARA ObtenerResumenPagosAsync
        // -------------------------------------------------------------
        [Fact]
        public async Task ObtenerResumenPagosAsync_When_No_Data_Should_Return_Message()
        {
            // Arrange & Act
            var result = await _pagoServices.ObtenerResumenPagosAsync();

            // Assert
            Assert.True(result.Success);
            Assert.Contains("Resumen de pagos obtenido correctamente.", result.Message);
        }

        [Fact]
        public async Task ObtenerResumenPagosAsync_Should_Return_Correct_Summary()
        {


            // Arrange
            await _clienteRepository.SaveAsync(new Domain.Entities.Configuration.Usuers.Cliente { Nombre = "carlos", Apellido = "test", Cedula = "123-4567897-8", Telefono = "849-887-8945", Direccion = "Auto.ven call test." });
            await _clienteRepository.SaveAsync(new Domain.Entities.Configuration.Usuers.Cliente { Nombre = "carlos", Apellido = "test", Cedula = "123-4577897-8", Telefono = "849-287-8945", Direccion = "Auto.ves call test." });
            await _usuarioRepository.SaveAsync(new Domain.Entities.Configuration.Usuers.Usuario { Nombre = "carlos", Correo = "correo@fm.s", Contraseña = "12345689", Rol = RolUsuarios.Cliente });
            await _categoriaRepository.SaveAsync(new Domain.Entities.Configuration.Habitaciones.Categoria { Nombre = "catego", Descripcion = "categoria" });
            await _pisoRepository.SaveAsync(new Domain.Entities.Configuration.Habitaciones.Piso { NumeroPiso = 1, Descripcion = "Test" });
            await _habitacionRepository.SaveAsync(new Domain.Entities.Configuration.Habitaciones.Habitacion { Numero = "A01", Capacidad = 5, IdCategoria = 1, IdPiso = 1 });
            
            await _reservaRepository.SaveAsync(new Reserva { IdCliente = 1, IdHabitacion = 1, IdUsuario = 1, FechaInicio = DateTime.Now, FechaFin = DateTime.Now.AddDays(2), CostoTotal = 100 });
            await _reservaRepository.SaveAsync(new Reserva { IdCliente = 2, IdHabitacion = 2, IdUsuario = 1, FechaInicio = DateTime.Now, FechaFin = DateTime.Now.AddDays(2), CostoTotal = 100 });
            await _pagoRepository.SaveAsync(new Pago { IdReserva = 1, Monto = 100, Estado = EstadoPago.Completado, FechaPago = DateTime.Now, MetodoPago = "Targeta" });
            await _pagoRepository.SaveAsync(new Pago { IdReserva = 2, Monto = 50, Estado = EstadoPago.Parcial, FechaPago = DateTime.Now, MetodoPago = "Targeta" });

            // Act
            var result = await _pagoServices.ObtenerResumenPagosAsync();
            // Assert
            Assert.True(result.Success);
            Assert.Equal(150,result.Data.TotalRecaudado);
            Assert.Equal(0, result.Data.TotalRechazado);
        }

        [Fact]
        public async Task ObtenerResumenPagosAsync_Should_Count_States_Correctly()
        {
            await _clienteRepository.SaveAsync(new Domain.Entities.Configuration.Usuers.Cliente { Nombre = "carlos", Apellido = "test", Cedula = "123-4567897-8", Telefono = "849-887-8945", Direccion = "Auto.ven call test." });
            await _clienteRepository.SaveAsync(new Domain.Entities.Configuration.Usuers.Cliente { Nombre = "carlos", Apellido = "test", Cedula = "123-4577897-8", Telefono = "849-287-8945", Direccion = "Auto.ves call test." });
            await _usuarioRepository.SaveAsync(new Domain.Entities.Configuration.Usuers.Usuario { Nombre = "carlos", Correo = "correo@fm.s", Contraseña = "12345689", Rol = RolUsuarios.Cliente });
            await _categoriaRepository.SaveAsync(new Domain.Entities.Configuration.Habitaciones.Categoria { Nombre = "catego", Descripcion = "categoria" });
            await _pisoRepository.SaveAsync(new Domain.Entities.Configuration.Habitaciones.Piso { NumeroPiso = 1, Descripcion = "Test" });
            await _habitacionRepository.SaveAsync(new Domain.Entities.Configuration.Habitaciones.Habitacion { Numero = "A01", Capacidad = 5, IdCategoria = 1, IdPiso = 1 });

            // Arrange
            await _reservaRepository.SaveAsync(new Reserva { IdCliente = 1, IdHabitacion = 1, IdUsuario = 1, FechaInicio = DateTime.Now, FechaFin = DateTime.Now.AddDays(2), CostoTotal = 100 });
            await _reservaRepository.SaveAsync(new Reserva { IdCliente = 2, IdHabitacion = 2, IdUsuario = 1, FechaInicio = DateTime.Now, FechaFin = DateTime.Now.AddDays(2), CostoTotal = 100 });
            // Arrange
            await _pagoRepository.SaveAsync(new Pago { IdReserva = 1, Monto = 80, Estado = EstadoPago.Pendiente, MetodoPago = "Tarjeta" });
            await _pagoRepository.SaveAsync(new Pago { IdReserva = 2, Monto = 200, Estado = EstadoPago.Rechazado, MetodoPago = "Tarjeta" });

            // Act
            var result = await _pagoServices.ObtenerResumenPagosAsync();

            // Assert
            Assert.True(result.Success);
            dynamic data = result.Data;
            Assert.Equal(1, (int)data.Pendientes);
            Assert.Equal(1, (int)data.Rechazados);
        }

        // -------------------------------------------------------------
        // TESTS PARA AnularPagoAsync
        // -------------------------------------------------------------
        [Fact]
        public async Task AnularPagoAsync_When_Pago_Not_Found_Should_Return_Error()
        {
            // Arrange
            int idPago = 999;

            // Act
            var result = await _pagoServices.AnularPagoAsync(idPago);

            // Assert
            Assert.False(result.Success);
            Assert.Contains("No se encontró el pago", result.Message);
        }

        [Fact]
        public async Task AnularPagoAsync_When_Pago_Already_Rechazado_Should_Return_Message()
        {
            // Arrange
            var pago = new Pago { IdReserva = 1, Monto = 200, MetodoPago = "Efectivo", Estado = EstadoPago.Rechazado, FechaPago = DateTime.Now };
            await _pagoRepository.SaveAsync(pago);

            // Act
            var result = await _pagoServices.AnularPagoAsync(pago.Id);

            // Assert
            Assert.False(result.Success);
            Assert.Contains("ya fue rechazado", result.Message, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public async Task AnularPagoAsync_Should_Anular_Correctly_And_Update_Reserva()
        {

            // Arrange
            await _clienteRepository.SaveAsync(new Domain.Entities.Configuration.Usuers.Cliente { Nombre = "carlos", Apellido = "test", Cedula = "123-4567897-8", Telefono = "849-887-8945", Direccion = "Auto.ven call test." });
            await _clienteRepository.SaveAsync(new Domain.Entities.Configuration.Usuers.Cliente { Nombre = "carlos", Apellido = "test", Cedula = "123-4577897-8", Telefono = "849-287-8945", Direccion = "Auto.ves call test." });
            await _usuarioRepository.SaveAsync(new Domain.Entities.Configuration.Usuers.Usuario { Nombre = "carlos", Correo = "correo@fm.s", Contraseña = "12345689", Rol = RolUsuarios.Cliente });
            await _categoriaRepository.SaveAsync(new Domain.Entities.Configuration.Habitaciones.Categoria { Nombre = "catego", Descripcion = "categoria" });
            await _pisoRepository.SaveAsync(new Domain.Entities.Configuration.Habitaciones.Piso { NumeroPiso = 1, Descripcion = "Test" });
            await _habitacionRepository.SaveAsync(new Domain.Entities.Configuration.Habitaciones.Habitacion { Numero = "A01", Capacidad = 5, IdCategoria = 1, IdPiso = 1 });

            var reserva = await _reservaRepository.SaveAsync(new Reserva { IdCliente = 1, IdHabitacion = 1, IdUsuario = 1, FechaInicio = DateTime.Now, FechaFin = DateTime.Now.AddDays(2), CostoTotal = 300, Estado = EstadoReserva.Confirmada });

            var pago = new Pago { IdReserva = reserva.Data.Id, Monto = 300, MetodoPago = "Tarjeta", Estado = EstadoPago.Completado, FechaPago = DateTime.Now };
            var pagoSaved = await _pagoRepository.SaveAsync(pago);

            // Act
            var result = await _pagoServices.AnularPagoAsync(pagoSaved.Data.Id);

            // Assert
            Assert.True(result.Success);
            Assert.Contains("anulado correctamente", result.Message, StringComparison.OrdinalIgnoreCase);
        }




    }
}
