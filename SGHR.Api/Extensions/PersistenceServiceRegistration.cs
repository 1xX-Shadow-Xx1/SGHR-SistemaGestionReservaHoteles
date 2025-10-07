using Microsoft.EntityFrameworkCore;
using SGHR.Domain.Repository;
using SGHR.Persistence.Contex;
using SGHR.Persistence.Interfaces.Habitaciones;
using SGHR.Persistence.Interfaces.Operaciones;
using SGHR.Persistence.Interfaces.Reportes;
using SGHR.Persistence.Interfaces.Reservas;
using SGHR.Persistence.Interfaces.Users;
using SGHR.Persistence.Repositories.EF.Habitaciones;
using SGHR.Persistence.Repositories.EF.Operaciones;
using SGHR.Persistence.Repositories.EF.Reservas;
using SGHR.Persistence.Repositories.EF.Users;

namespace SGHR.Api.Builders
{
    public static class PersistenceServiceRegistration
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Configuraccion de SGHRContex
            services.AddDbContext<SGHRContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("SghrConnString")));

            // Registrar Repositorios

            //Usuarios
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<IClienteRepository, ClienteRepository>();

            //Reservas
            services.AddScoped<IReservaRepository, ReservaRepository>();
            services.AddScoped<IServicioAdicionalRepository, ServicioAdicionalRepository>();
            services.AddScoped<ITarifaRepository, TarifaRepository>();

            //Operaciones
            services.AddScoped<IAuditoryRepository, AuditoryRepository>();
            services.AddScoped<ICheckInOutRepository, CheckInOutRepository>();
            services.AddScoped<IMantenimientoRepository, MantenimientoRepository>();
            services.AddScoped<IPagoRepository, PagoRepository>();
            services.AddScoped<IReporteRepository, ReporteRepository>();

            //Habitaciones
            services.AddScoped<IAmenityRepository, AmenityRepository>();
            services.AddScoped<ICategoriaRepository, CategoriaRepository>();
            services.AddScoped<IHabitacionRepository, HabitacionRepository>();
            services.AddScoped<IPisoRepository, PisoRepository>();

            return services;
        }
    }
}
