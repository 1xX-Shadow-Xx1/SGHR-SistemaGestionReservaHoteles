
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SGHR.Application.Dtos.Configuration.Operaciones.Mantenimiento;
using SGHR.Application.Services.Operaciones;
using SGHR.Domain.Entities.Configuration.Habitaciones;
using SGHR.Domain.Entities.Configuration.Reservas;
using SGHR.Domain.Entities.Configuration.Usuers;
using SGHR.Domain.Enum.Habitacion;
using SGHR.Domain.Enum.Operaciones;
using SGHR.Domain.Enum.Reservas;
using SGHR.Domain.Enum.Usuarios;
using SGHR.Domain.Repository;
using SGHR.Domain.Validators.ConfigurationRules.Habitaciones;
using SGHR.Domain.Validators.ConfigurationRules.Operaciones;
using SGHR.Domain.Validators.ConfigurationRules.Reservas;
using SGHR.Domain.Validators.ConfigurationRules.Users;
using SGHR.Persistence.Context;
using SGHR.Persistence.Interfaces.Habitaciones;
using SGHR.Persistence.Interfaces.Operaciones;
using SGHR.Persistence.Interfaces.Users;
using SGHR.Persistence.Repositories.EF.Habitaciones;
using SGHR.Persistence.Repositories.EF.Operaciones;
using SGHR.Persistence.Repositories.EF.Reservas;
using SGHR.Persistence.Repositories.EF.Users;

namespace SGHR.Application.Test.OperacionesTest
{
    public class MantenimientoServicesTest
    {
        private readonly MantenimientoServcices _service;
        private readonly SGHRContext _context;

        private readonly IMantenimientoRepository _mantenimientoRepo;
        private readonly IHabitacionRepository _habitacionRepo;
        private readonly IReservaRepository _reservaRepo;
        private readonly IUsuarioRepository _usuarioRepo;
        private readonly IPisoRepository _pisoRepo;
        private readonly ICategoriaRepository _categoriaRepo;
        private readonly IClienteRepository _clienteRepo;

        public MantenimientoServicesTest()
        {
            
            var options = new DbContextOptionsBuilder<SGHRContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new SGHRContext(options);

            
            var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
            var loggerservice = loggerFactory.CreateLogger<MantenimientoServcices>();
            var loggerMantenimiento = loggerFactory.CreateLogger<MantenimientoRepository>();
            var loggerReserva = loggerFactory.CreateLogger<ReservaRepository>();
            var loggerHabitacion = loggerFactory.CreateLogger<HabitacionRepository>();
            var loggerUsuario = loggerFactory.CreateLogger<UsuarioRepository>();
            var loggerCliente = loggerFactory.CreateLogger<ClienteRepository>();
            var loggerCategoria = loggerFactory.CreateLogger<CategoriaRepository>();
            var loggerPiso = loggerFactory.CreateLogger<PisoRepository>();

           
            _mantenimientoRepo = new MantenimientoRepository(_context, new MantenimientoValidator(), loggerMantenimiento);
            _habitacionRepo = new HabitacionRepository(_context, new HabitacionValidator(), loggerHabitacion);
            _reservaRepo = new ReservaRepository(_context, new ReservaValidator(), loggerReserva);
            _usuarioRepo = new UsuarioRepository(_context, new UsuarioValidator(), loggerUsuario);
            _pisoRepo = new PisoRepository(_context, new PisoValidator(), loggerPiso);
            _categoriaRepo = new CategoriaRepository(_context, new CategoriaValidator(), loggerCategoria);
            _clienteRepo = new ClienteRepository(_context, new ClienteValidator(), loggerCliente);

           
            _service = new MantenimientoServcices(loggerservice, _mantenimientoRepo, _habitacionRepo, _reservaRepo, _usuarioRepo, _pisoRepo);

         
            _clienteRepo.SaveAsync(new Cliente { Nombre = "carlos", Apellido = "test", Cedula = "123-4567897-8", Telefono = "849-887-8945", Direccion = "Auto.ven call test." }).Wait();
            _clienteRepo.SaveAsync(new Cliente { Nombre = "carlos", Apellido = "test", Cedula = "123-4577897-8", Telefono = "849-287-8945", Direccion = "Auto.ves call test." }).Wait();
            _usuarioRepo.SaveAsync(new Usuario { Nombre = "Admin", Correo = "admin@mail.com", Contraseña = "12345689", Rol = RolUsuarios.Administrador }).Wait();
            _categoriaRepo.SaveAsync(new Categoria { Nombre = "catego", Descripcion = "categoria" }).Wait();
            _pisoRepo.SaveAsync(new Piso { NumeroPiso = 1, Descripcion = "Piso 1" }).Wait();
            _habitacionRepo.SaveAsync(new Habitacion { Numero = "H101", Estado = EstadoHabitacion.Disponible, Capacidad = 5, IdCategoria = 1, IdPiso = 1 }).Wait();
        }


        [Fact(DisplayName = "CreateAsync debe crear un mantenimiento correctamente")]
        public async Task CreateAsync_Should_Create_Mantenimiento_Successfully()
        {
            //Arrange
            var dto = new CreateMantenimientoDto
            {
                NumeroHabitacion = "H101",
                NumeroPiso = 1,
                RealizadoPor = "admin@mail.com",
                Descripcion = "Revisión de aire acondicionado",
                FechaInicio = DateTime.Now
            };

            //Act
            var result = await _service.CreateAsync(dto);


            //Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Data);

            var data = result.Data as MantenimientoDto;
            Assert.Equal("H101", data.NumeroHabitacion);
            Assert.Equal("admin@mail.com", data.RealizadoPor);
            Assert.Equal("Revisión de aire acondicionado", data.Descripcion);
        }

        [Fact(DisplayName = "CreateAsync debe fallar si la habitación está reservada")]
        public async Task CreateAsync_Should_Fail_If_Habitacion_Reservada()
        {
            //Arrange
            var cliente = (await _clienteRepo.GetByIdAsync(1)).Data;
            var usuario = (await _usuarioRepo.GetByIdAsync(1)).Data;
            var habitacion = (await _habitacionRepo.GetByIdAsync(1)).Data;
            habitacion.Estado = EstadoHabitacion.Reservada;
            await _habitacionRepo.UpdateAsync(habitacion);

            await _reservaRepo.SaveAsync(new Reserva { Id = 1, IdHabitacion = habitacion.Id, Estado = EstadoReserva.Activa, IdUsuario = usuario.Id, IdCliente = cliente.Id, CostoTotal = 500, FechaInicio = DateTime.Now, FechaFin = DateTime.Now.AddDays(2)});

            //Act
            var dto = new CreateMantenimientoDto
            {
                NumeroHabitacion = "H101",
                NumeroPiso = 1,
                RealizadoPor = "admin@mail.com",
                Descripcion = "Cambio de cortinas"
            };

            var result = await _service.CreateAsync(dto);

            //Assert
            Assert.False(result.Success);
            Assert.Contains("reservada", result.Message);
        }

        [Fact(DisplayName = "GetAllAsync debe devolver una lista de mantenimientos")]
        public async Task GetAllAsync_Should_Return_List()
        {
            //Arrange
            await _service.CreateAsync(new CreateMantenimientoDto
            {
                NumeroHabitacion = "H101",
                NumeroPiso = 1,
                RealizadoPor = "admin@mail.com",
                Descripcion = "Limpieza profunda",
                FechaInicio = DateTime.Now
            });

            //Act
            var result = await _service.GetAllAsync();

            var lista = result.Data as List<MantenimientoDto>;

            //Assert
            Assert.True(result.Success);
            Assert.Single(lista);
            var mant = lista.First();
            Assert.Equal("Limpieza profunda", mant.Descripcion);
        }

        [Fact(DisplayName = "GetByIdAsync debe devolver el mantenimiento correcto")]
        public async Task GetByIdAsync_Should_Return_Correct_Data()
        {
            //Arrange
            var createResult = await _service.CreateAsync(new CreateMantenimientoDto
            {
                NumeroHabitacion = "H101",
                NumeroPiso = 1,
                RealizadoPor = "admin@mail.com",
                Descripcion = "Pintura general"
            });

            //Act
            var created = createResult.Data as MantenimientoDto;

            var result = await _service.GetByIdAsync(created.Id);

            //Assert
            Assert.True(result.Success);
            var dto = result.Data as MantenimientoDto;
            Assert.Equal("Pintura general", dto.Descripcion);
            Assert.Equal("admin@mail.com", dto.RealizadoPor);
        }

        [Fact(DisplayName = "UpdateAsync debe actualizar correctamente")]
        public async Task UpdateAsync_Should_Update_Mantenimiento_Successfully()
        {
            //Arrange
            var createResult = await _service.CreateAsync(new CreateMantenimientoDto
            {
                NumeroHabitacion = "H101",
                NumeroPiso = 1,
                RealizadoPor = "admin@mail.com",
                Descripcion = "Mantenimiento inicial"
            });


            var created = createResult.Data as MantenimientoDto;

            //Act
            var updateDto = new UpdateMantenimientoDto
            {
                Id = created.Id,
                NumeroHabitacion = "H101",
                NumeroPiso = 1,
                RealizadoPor = "admin@mail.com",
                Descripcion = "Cambio de bombillos",
                Estado = EstadoMantenimiento.Completado.ToString(),
                FechaInicio = DateTime.Now.AddDays(-1),
                FechaFin = DateTime.Now
            };

            var result = await _service.UpdateAsync(updateDto);

            //Assert
            Assert.True(result.Success);
            var data = result.Data as MantenimientoDto;
            Assert.Equal("Cambio de bombillos", data.Descripcion);
            Assert.Equal(EstadoMantenimiento.Completado.ToString(), data.Estado);
        }

        [Fact(DisplayName = "DeleteAsync debe eliminar el mantenimiento correctamente")]
        public async Task DeleteAsync_Should_Delete_Mantenimiento_Successfully()
        {
            //Arrange
            var createResult = await _service.CreateAsync(new CreateMantenimientoDto
            {
                NumeroHabitacion = "H101",
                NumeroPiso = 1,
                RealizadoPor = "admin@mail.com",
                Descripcion = "Eliminar prueba"
            });

            //Act
            var created = createResult.Data as MantenimientoDto;
            var result = await _service.DeleteAsync(created.Id);

            //Assert
            Assert.True(result.Success);
            Assert.Contains("eliminada", result.Message, StringComparison.OrdinalIgnoreCase);
        }
    }
}
