using SGHR.Application.Interfaces.Habitaciones;
using SGHR.Application.Services.Habitaciones;
using SGHR.Persistence.Interfaces.Habitaciones;
using SGHR.Persistence.Repositories.EF.Habitaciones;
using Microsoft.Extensions.DependencyInjection;

namespace SGHR.IOC.Dependencia.Habitaciones
{
    public static class PisoDepences
    {
        public static IServiceCollection AddPisoDependences(this IServiceCollection services)
        {
            services.AddScoped<IPisoServices, PisoServices>();
            services.AddScoped<IPisoRepository, PisoRepository>();

            return services;
        }
    }
}
