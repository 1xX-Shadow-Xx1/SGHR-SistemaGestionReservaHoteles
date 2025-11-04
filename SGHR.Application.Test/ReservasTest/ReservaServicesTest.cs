
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SGHR.Application.Dtos.Configuration.Reservas.Reserva;
using SGHR.Application.Interfaces.Reservas;
using SGHR.Application.Services.Reservas;
using SGHR.Domain.Entities.Configuration.Habitaciones;
using SGHR.Domain.Entities.Configuration.Reservas;
using SGHR.Domain.Entities.Configuration.Usuers;
using SGHR.Domain.Repository;
using SGHR.Domain.Validators.ConfigurationRules.Habitaciones;
using SGHR.Domain.Validators.ConfigurationRules.Reservas;
using SGHR.Domain.Validators.ConfigurationRules.Users;
using SGHR.Persistence.Context;
using SGHR.Persistence.Interfaces.Habitaciones;
using SGHR.Persistence.Interfaces.Reservas;
using SGHR.Persistence.Interfaces.Users;
using SGHR.Persistence.Repositories.EF.Habitaciones;
using SGHR.Persistence.Repositories.EF.Reservas;
using SGHR.Persistence.Repositories.EF.Users;

namespace SGHR.Application.Test.ReservasTest
{
    public class ReservaServicesTest
    {
        private readonly IReservaRepository _reservaRepository;
        private readonly IClienteRepository _clienteRepository;
        private readonly IHabitacionRepository _habitacionRepository;
        private readonly IServicioAdicionalRepository _servicioAdicionalRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ITarifaRepository _tarifaRepository;
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly IPisoRepository _pisoRepository;
        private readonly SGHRContext _context;
        private readonly ILogger<ReservaRepository> _loggerReservaRepo;
        private readonly ILogger<ClienteRepository> _loggerClienteRepo;
        private readonly ILogger<HabitacionRepository> _loggerHabitacionRepo;
        private readonly ILogger<ServicioAdicionalRepository> _loggerServicioAdicionalRepo;
        private readonly ILogger<TarifaRepository> _loggerTarifaRepo;
        private readonly ILogger<UsuarioRepository> _loggerUsuarioRepo;
        private readonly ILogger<CategoriaRepository> _loggerCategoriaRepo;
        private readonly ILogger<PisoRepository> _loggerPisoRepo;
        private readonly ReservaValidator _reservaValidator;
        private readonly ClienteValidator _clienteValidator;
        private readonly HabitacionValidator _habitacionValidator;
        private readonly ServicioAdicionalValidator _servicioAdicionalValidator;
        private readonly UsuarioValidator _usuarioValidator;
        private readonly TarifaValidator _tarifaValidator;
        private readonly CategoriaValidator _categoriaValidator;
        private readonly PisoValidator _pisoValidator;
        private readonly IConfiguration _configuration;
        private readonly IReservaServices _reservaServices;
        private readonly ILogger<ReservaServices> _loggerServices;

        public ReservaServicesTest()
        {
            _reservaValidator = new ReservaValidator();
            _clienteValidator = new ClienteValidator();
            _habitacionValidator = new HabitacionValidator();
            _servicioAdicionalValidator = new ServicioAdicionalValidator();
            _usuarioValidator = new UsuarioValidator();
            _tarifaValidator = new TarifaValidator();
            _categoriaValidator = new CategoriaValidator();
            _pisoValidator = new PisoValidator();

            var options = new DbContextOptionsBuilder<SGHRContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new SGHRContext(options);

            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
            });

            _loggerReservaRepo = loggerFactory.CreateLogger<ReservaRepository>();
            _loggerClienteRepo = loggerFactory.CreateLogger<ClienteRepository>();
            _loggerHabitacionRepo = loggerFactory.CreateLogger<HabitacionRepository>();
            _loggerServicioAdicionalRepo = loggerFactory.CreateLogger<ServicioAdicionalRepository>();
            _loggerServices = loggerFactory.CreateLogger<ReservaServices>();
            _loggerUsuarioRepo = loggerFactory.CreateLogger<UsuarioRepository>();
            _loggerTarifaRepo = loggerFactory.CreateLogger<TarifaRepository>();
            _loggerCategoriaRepo = loggerFactory.CreateLogger<CategoriaRepository>();
            _loggerPisoRepo = loggerFactory.CreateLogger<PisoRepository>();
            _configuration = new ConfigurationBuilder().Build();

            _reservaRepository = new ReservaRepository(_context, _reservaValidator, _loggerReservaRepo);
            _clienteRepository = new ClienteRepository(_context, _clienteValidator, _loggerClienteRepo);
            _habitacionRepository = new HabitacionRepository(_context, _habitacionValidator, _loggerHabitacionRepo);
            _servicioAdicionalRepository = new ServicioAdicionalRepository(_context, _servicioAdicionalValidator, _loggerServicioAdicionalRepo);
            _usuarioRepository = new UsuarioRepository(_context,_usuarioValidator, _loggerUsuarioRepo);
            _tarifaRepository = new TarifaRepository(_context, _tarifaValidator, _loggerTarifaRepo);
            _categoriaRepository = new CategoriaRepository(_context, _categoriaValidator, _loggerCategoriaRepo);
            _pisoRepository = new PisoRepository(_context,_pisoValidator, _loggerPisoRepo);

            _reservaServices = new ReservaServices(
                _loggerServices,
                _reservaRepository,
                _usuarioRepository,
                _habitacionRepository,
                _clienteRepository,
                _tarifaRepository,
                _categoriaRepository,
                _servicioAdicionalRepository                
            );
        }

        // -------------------- CREATE TESTS --------------------

        [Fact]
        public async Task When_Reserva_is_null_CreateAsync_Should_Fail()
        {
            //Arrange
            CreateReservaDto reserva = null;

            //Act
            var result = await _reservaServices.CreateAsync(reserva);

            //Assert
            Assert.False(result.Success);
            Assert.Equal("La reserva no puede ser nula.", result.Message);
        }

        [Fact]
        public async Task When_Cliente_not_exists_CreateAsync_Should_Fail()
        {

            var pi = new Piso { NumeroPiso = 1, Descripcion = "PisoTest" };
            var piso = await _pisoRepository.SaveAsync(pi);

            var cat = new Categoria { Nombre = "Suite", Descripcion = "Categoria A" };
            var categoria = await _categoriaRepository.SaveAsync(cat);

            var hab = new Habitacion { Numero = "102", IdCategoria = categoria.Data.Id, IdPiso = piso.Data.Id, Capacidad = 5 };
            var habitacion = await _habitacionRepository.SaveAsync(hab);

            //Arrange
            var dto = new CreateReservaDto
            {
                CedulaCliente = "999-9999999-9",
                NumeroHabitacion = habitacion.Data.Numero,
                FechaInicio = DateTime.Now,
                FechaFin = DateTime.Now.AddDays(3)
            };

            //Act
            var result = await _reservaServices.CreateAsync(dto);

            //Assert
            Assert.False(result.Success);
            Assert.Equal("No existe un cliente registrado con ese numero de cedula.", result.Message);
        }

        [Fact]
        public async Task When_Reserva_is_valid_CreateAsync_Should_Succeed()
        {
            //Arrange
            var cli = new Cliente { Nombre = "Luis", Apellido = "Morales", Cedula = "003-0000000-1", Direccion = "Holamudn.sdo sokd", Telefono = "849-878-7888" };
            var cliente = await _clienteRepository.SaveAsync(cli);

            var pi = new Piso { NumeroPiso = 1, Descripcion = "PisoTest" };
            var piso = await _pisoRepository.SaveAsync(pi);

            var cat = new Categoria { Nombre = "Suite", Descripcion = "Categoria A" };
            var categoria = await _categoriaRepository.SaveAsync(cat);

            var tar = new Tarifa { IdCategoria = categoria.Data.Id, Temporada = "ot", Precio = 2000 };
            var tarifa = await _tarifaRepository.SaveAsync(tar);

            var usu = new Usuario { Nombre = "Luis", Correo = "Luis@d.s", Contraseña = "123456798" };
            var usuario = await _usuarioRepository.SaveAsync(usu);

            var hab = new Habitacion { Numero = "102", IdCategoria = categoria.Data.Id, IdPiso = piso.Data.Id, Capacidad = 5 };
            var habitacion = await _habitacionRepository.SaveAsync(hab);

            var dto = new CreateReservaDto
            {
                CedulaCliente = cliente.Data.Cedula,
                NumeroHabitacion = habitacion.Data.Numero,
                CorreoCliente = usuario.Data.Correo,
                FechaInicio = DateTime.Now,
                FechaFin = DateTime.Now.AddDays(2)
            };

            //Act
            var result = await _reservaServices.CreateAsync(dto);

            //Assert
            Assert.True(result.Success);
            Assert.Equal("Reserva registrada correctamente.", result.Message);
        }

        // -------------------- DELETE TESTS --------------------

        [Fact]
        public async Task When_Reserva_not_exists_DeleteAsync_Should_Fail()
        {
            //Arrange
            int id = 99;

            //Act
            var result = await _reservaServices.DeleteAsync(id);

            //Assert
            Assert.False(result.Success);
            Assert.Equal("Reserva no encontrada.", result.Message);
        }

        [Fact]
        public async Task When_Reserva_exists_DeleteAsync_Should_Succeed()
        {
            //Arrange
            var cli = new Cliente { Nombre = "Luis", Apellido = "Morales", Cedula = "003-0000000-1", Direccion = "Holamudn.sdo sokd", Telefono = "849-878-7888" };
            var cliente = await _clienteRepository.SaveAsync(cli);

            var pi = new Piso { NumeroPiso = 1, Descripcion = "PisoTest" };
            var piso = await _pisoRepository.SaveAsync(pi);

            var cat = new Categoria { Nombre = "Suite", Descripcion = "Categoria A" };
            var categoria = await _categoriaRepository.SaveAsync(cat);

            var tar = new Tarifa { IdCategoria = categoria.Data.Id, Temporada = "ot", Precio = 2000 };
            var tarifa = await _tarifaRepository.SaveAsync(tar);

            var usu = new Usuario { Nombre = "Luis", Correo = "Luis@d.s", Contraseña = "123456798" };
            var usuario = await _usuarioRepository.SaveAsync(usu);

            var hab = new Habitacion { Numero = "102", IdCategoria = categoria.Data.Id, IdPiso = piso.Data.Id, Capacidad = 5 };
            var habitacion = await _habitacionRepository.SaveAsync(hab);

            var reserva = new Reserva
            {
                IdCliente = cliente.Data.Id,
                IdHabitacion = habitacion.Data.Id,
                IdUsuario = usuario.Data.Id,
                FechaInicio = DateTime.Now,
                FechaFin = DateTime.Now.AddDays(1),
                CostoTotal = 100
            };
            await _reservaRepository.SaveAsync(reserva);

            //Act
            var result = await _reservaServices.DeleteAsync(reserva.Id);

            //Assert
            Assert.True(result.Success);
            Assert.Equal("Reserva eliminada exitosamente.", result.Message);
        }

        [Fact]
        public async Task When_Reserva_already_deleted_DeleteAsync_Should_Fail()
        {
            //Arrange
            var cli = new Cliente { Nombre = "Luis", Apellido = "Morales", Cedula = "003-0000000-1", Direccion = "Holamudn.sdo sokd", Telefono = "849-878-7888" };
            var cliente = await _clienteRepository.SaveAsync(cli);

            var pi = new Piso { NumeroPiso = 1, Descripcion = "PisoTest" };
            var piso = await _pisoRepository.SaveAsync(pi);

            var cat = new Categoria { Nombre = "Suite", Descripcion = "Categoria A" };
            var categoria = await _categoriaRepository.SaveAsync(cat);

            var tar = new Tarifa { IdCategoria = categoria.Data.Id, Temporada = "ot", Precio = 2000 };
            var tarifa = await _tarifaRepository.SaveAsync(tar);

            var usu = new Usuario { Nombre = "Luis", Correo = "Luis@d.s", Contraseña = "123456798" };
            var usuario = await _usuarioRepository.SaveAsync(usu);

            var hab = new Habitacion { Numero = "102", IdCategoria = categoria.Data.Id, IdPiso = piso.Data.Id, Capacidad = 5 };
            var habitacion = await _habitacionRepository.SaveAsync(hab);

            var reserva = new Reserva
            {
                Id = 1,
                IdCliente = cliente.Data.Id,
                IdHabitacion = habitacion.Data.Id,
                IdUsuario = usuario.Data.Id,
                FechaInicio = DateTime.Now,
                FechaFin = DateTime.Now.AddDays(1),
                CostoTotal = 100,
                IsDeleted = true
            };
            await _reservaRepository.SaveAsync(reserva);

            //Act
            var result = await _reservaServices.DeleteAsync(reserva.Id);

            //Assert
            Assert.False(result.Success);
            Assert.Equal("Reserva no encontrada.", result.Message);
        }

        [Fact]
        public async Task When_Reserva_is_valid_DeleteAsync_Should_Change_Eliminado_State()
        {
            //Arrange
            var cli = new Cliente { Nombre = "Luis", Apellido = "Morales", Cedula = "003-0000000-1", Direccion = "Holamudn.sdo sokd", Telefono = "849-878-7888" };
            var cliente = await _clienteRepository.SaveAsync(cli);

            var pi = new Piso { NumeroPiso = 1, Descripcion = "PisoTest" };
            var piso = await _pisoRepository.SaveAsync(pi);

            var cat = new Categoria { Nombre = "Suite", Descripcion = "Categoria A" };
            var categoria = await _categoriaRepository.SaveAsync(cat);

            var tar = new Tarifa { IdCategoria = categoria.Data.Id, Temporada = "ot", Precio = 2000 };
            var tarifa = await _tarifaRepository.SaveAsync(tar);

            var usu = new Usuario { Nombre = "Luis", Correo = "Luis@d.s", Contraseña = "123456798" };
            var usuario = await _usuarioRepository.SaveAsync(usu);

            var hab = new Habitacion { Numero = "102", IdCategoria = categoria.Data.Id, IdPiso = piso.Data.Id, Capacidad = 5 };
            var habitacion = await _habitacionRepository.SaveAsync(hab);

            var reserva = new Reserva
            {
                IdCliente = cliente.Data.Id,
                IdHabitacion = habitacion.Data.Id,
                IdUsuario = usuario.Data.Id,
                FechaInicio = DateTime.Now,
                FechaFin = DateTime.Now.AddDays(1),
                CostoTotal = 100
            };
            await _reservaRepository.SaveAsync(reserva);

            //Act
            await _reservaServices.DeleteAsync(reserva.Id);
            var deleted = await _reservaRepository.GetByIdAsync(reserva.Id, true);

            //Assert
            Assert.True(deleted.Data.IsDeleted);
        }

        // -------------------- GET TESTS --------------------

        [Fact]
        public async Task When_GetAllAsync_Should_Return_List()
        {
            //Arrange
            var cli = new Cliente { Nombre = "Luis", Apellido = "Morales", Cedula = "003-0000000-1", Direccion = "Holamudn.sdo sokd", Telefono = "849-878-7888" };
            var cliente = await _clienteRepository.SaveAsync(cli);

            var pi = new Piso { NumeroPiso = 1, Descripcion = "PisoTest" };
            var piso = await _pisoRepository.SaveAsync(pi);

            var cat = new Categoria { Nombre = "Suite", Descripcion = "Categoria A" };
            var categoria = await _categoriaRepository.SaveAsync(cat);

            var tar = new Tarifa { IdCategoria = categoria.Data.Id, Temporada = "ot", Precio = 2000 };
            var tarifa = await _tarifaRepository.SaveAsync(tar);

            var usu = new Usuario { Nombre = "Luis", Correo = "Luis@d.s", Contraseña = "123456798" };
            var usuario = await _usuarioRepository.SaveAsync(usu);

            var hab = new Habitacion { Numero = "102", IdCategoria = categoria.Data.Id, IdPiso = piso.Data.Id, Capacidad = 5 };
            var habitacion = await _habitacionRepository.SaveAsync(hab);

            var reserva = new Reserva
            {
                IdCliente = cliente.Data.Id,
                IdHabitacion = habitacion.Data.Id,
                IdUsuario = usuario.Data.Id,
                FechaInicio = DateTime.Now,
                FechaFin = DateTime.Now.AddDays(1),
                CostoTotal = 100
            };
            await _reservaRepository.SaveAsync(reserva);

            //Act
            var result = await _reservaServices.GetAllAsync();

            //Assert
            Assert.True(result.Success);
            Assert.NotEmpty(result.Data);
        }

        [Fact]
        public async Task When_GetByIdAsync_NotFound_Should_Fail()
        {
            //Arrange
            int id = 99;

            //Act
            var result = await _reservaServices.GetByIdAsync(id);

            //Assert
            Assert.False(result.Success);
            Assert.Equal("Reserva no encontrada.", result.Message);
        }

        [Fact]
        public async Task When_GetByIdAsync_Found_Should_Succeed()
        {
            //Arrange
            var cli = new Cliente { Nombre = "Luis", Apellido = "Morales", Cedula = "003-0000000-1", Direccion = "Holamudn.sdo sokd", Telefono = "849-878-7888" };
            var cliente = await _clienteRepository.SaveAsync(cli);

            var pi = new Piso { NumeroPiso = 1, Descripcion = "PisoTest" };
            var piso = await _pisoRepository.SaveAsync(pi);

            var cat = new Categoria { Nombre = "Suite", Descripcion = "Categoria A" };
            var categoria = await _categoriaRepository.SaveAsync(cat);

            var tar = new Tarifa { IdCategoria = categoria.Data.Id, Temporada = "ot", Precio = 2000 };
            var tarifa = await _tarifaRepository.SaveAsync(tar);

            var usu = new Usuario { Nombre = "Luis", Correo = "Luis@d.s", Contraseña = "123456798" };
            var usuario = await _usuarioRepository.SaveAsync(usu);

            var hab = new Habitacion { Numero = "102", IdCategoria = categoria.Data.Id, IdPiso = piso.Data.Id, Capacidad = 5 };
            var habitacion = await _habitacionRepository.SaveAsync(hab);

            var reserva = new Reserva
            {
                IdCliente = cliente.Data.Id,
                IdHabitacion = habitacion.Data.Id,
                IdUsuario = usuario.Data.Id,
                FechaInicio = DateTime.Now,
                FechaFin = DateTime.Now.AddDays(1),
                CostoTotal = 100
            };
            await _reservaRepository.SaveAsync(reserva);

            //Act
            var result = await _reservaServices.GetByIdAsync(reserva.Id);

            //Assert
            Assert.True(result.Success);
            Assert.Equal(reserva.Id, result.Data.Id);
        }

        [Fact]
        public async Task When_GetAllAsync_NoRecords_Should_Return_Empty_List()
        {
            //Arrange & Act
            var result = await _reservaServices.GetAllAsync();

            //Assert
            Assert.True(result.Success);
            Assert.Empty(result.Data);
        }

        // -------------------- UPDATE TESTS --------------------

        [Fact]
        public async Task When_Reserva_is_null_UpdateAsync_Should_Fail()
        {
            //Arrange
            UpdateReservaDto dto = null;

            //Act
            var result = await _reservaServices.UpdateAsync(dto);

            //Assert
            Assert.False(result.Success);
            Assert.Equal("La reserva no puede ser nula.", result.Message);
        }

        [Fact]
        public async Task When_Reserva_not_exists_UpdateAsync_Should_Fail()
        {
            //Arrange
            var dto = new UpdateReservaDto
            {
                Id = 99,
                FechaInicio = DateTime.Now,
                FechaFin = DateTime.Now.AddDays(2)
            };

            //Act
            var result = await _reservaServices.UpdateAsync(dto);

            //Assert
            Assert.False(result.Success);
            Assert.Equal("Reserva no encontrada.", result.Message);
        }

        [Fact]
        public async Task When_UpdateAsync_Invalid_Fechas_Should_Fail()
        {
            //Arrange
            var cli = new Cliente { Nombre = "Luis", Apellido = "Morales", Cedula = "003-0000000-1", Direccion = "Holamudn.sdo sokd", Telefono = "849-878-7888" };
            var cliente = await _clienteRepository.SaveAsync(cli);

            var pi = new Piso { NumeroPiso = 1, Descripcion = "PisoTest" };
            var piso = await _pisoRepository.SaveAsync(pi);

            var cat = new Categoria { Nombre = "Suite", Descripcion = "Categoria A" };
            var categoria = await _categoriaRepository.SaveAsync(cat);

            var tar = new Tarifa { IdCategoria = categoria.Data.Id, Temporada = "ot", Precio = 2000 };
            var tarifa = await _tarifaRepository.SaveAsync(tar);

            var usu = new Usuario { Nombre = "Luis", Correo = "Luis@d.s", Contraseña = "123456798" };
            var usuario = await _usuarioRepository.SaveAsync(usu);

            var hab = new Habitacion { Numero = "102", IdCategoria = categoria.Data.Id, IdPiso = piso.Data.Id, Capacidad = 5 };
            var habitacion = await _habitacionRepository.SaveAsync(hab);

            var reserva = new Reserva
            {
                IdCliente = cliente.Data.Id,
                IdHabitacion = habitacion.Data.Id,
                IdUsuario = usuario.Data.Id,
                FechaInicio = DateTime.Now,
                FechaFin = DateTime.Now.AddDays(1),
                CostoTotal = 100
            };
            await _reservaRepository.SaveAsync(reserva);

            var dto = new UpdateReservaDto
            {
                Id = reserva.Id,
                FechaFin = DateTime.Now.AddDays(5),
                FechaInicio = DateTime.Now.AddDays(2),
                NumeroHabitacion = habitacion.Data.Numero,
                CedulaCliente = cliente.Data.Cedula,
                CostoTotal = 100,
                CorreoCliente = usuario.Data.Correo
            };

            //Act
            var result = await _reservaServices.UpdateAsync(dto);

            //Assert
            Assert.False(result.Success);
            Assert.Equal("Las fechas de la reserva son inválidas.", result.Message);
        }

    }
}
