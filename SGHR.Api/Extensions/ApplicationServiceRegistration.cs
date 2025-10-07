using SGHR.Application.Interfaces.Habitaciones;
using SGHR.Application.Interfaces.Operaciones;
using SGHR.Application.Interfaces.Reservas;
using SGHR.Application.Interfaces.Users;
using SGHR.Application.Services.Categorias;
using SGHR.Application.Services.Operaciones;
using SGHR.Application.Services.Reservas;
using SGHR.Application.Services.Users;

namespace SGHR.Api.Builders
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services)
        {
            // Registrar servicios de aplicacion

            //Usuarios
            services.AddScoped<IUsuarioService, UsuarioService>();
            services.AddScoped<IClienteService, ClienteService>();

            //Reservas
            services.AddScoped<IReservaService, ReservaService>();
            services.AddScoped<IServicioAdicionalService, ServicioAdicionalService>();
            services.AddScoped<ITarifaService, TarifaService>();

            //Operaciones
            services.AddScoped<IAuditoryService, AuditoryService>();
            services.AddScoped<ICheckInOutService, CheckInOutService>();
            services.AddScoped<IMantenimientoService, MantenimientoService>();
            services.AddScoped<IPagoService, PagoService>();
            services.AddScoped<IReporteService, ReporteService>();

            //Habitaciones
            services.AddScoped<IAmenityService, AmenityService>();
            services.AddScoped<ICategoriaService, CategiriaService>();
            services.AddScoped<IHabitacionService, HabitacionService>();
            services.AddScoped<IPisoService, PisoService>();

            return services;
        }
    }
}
