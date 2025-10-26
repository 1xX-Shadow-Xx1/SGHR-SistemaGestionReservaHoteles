
using SGHR.Application.Interfaces.Usuarios;
using SGHR.Application.Services.Usuarios;
using SGHR.Domain.Validators.ConfigurationRules.Habitaciones;
using SGHR.Domain.Validators.ConfigurationRules.Operaciones;
using SGHR.Domain.Validators.ConfigurationRules.Reservas;
using SGHR.Domain.Validators.ConfigurationRules.Users;
using SGHR.Persistence.Interfaces.Users;
using SGHR.Persistence.Repositories.EF.Users;
using Microsoft.Extensions.DependencyInjection;

namespace SGHR.IOC.Dependencia.Users
{
    public static class ClienteDependences
    {
        public static IServiceCollection AddClienteDependences(this IServiceCollection services)
        {
            services.AddScoped<IClienteServices, ClienteServices>();
            services.AddScoped<IClienteRepository, ClienteRepository>();

            services.AddScoped<ClienteValidator>();
            services.AddScoped<UsuarioValidator>();

            services.AddScoped<ReservaValidator>();
            services.AddScoped<PagoValidator>();
            services.AddScoped<ServicioAdicionalValidator>();

            services.AddScoped<TarifaValidator>();
            services.AddScoped<ReporteValidator>();
            services.AddScoped<MantenimientoValidator>();
            services.AddScoped<CheckInOutValidator>();
            services.AddScoped<AuditoryValidator>();

            services.AddScoped<PisoValidator>();
            services.AddScoped<AmenitiesValidator>();
            services.AddScoped<CategoriaValidator>();
            services.AddScoped<HabitacionValidator>();

            return services;
        }
    }
}
