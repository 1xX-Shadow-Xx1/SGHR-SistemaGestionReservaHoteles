using Microsoft.Extensions.DependencyInjection;
using SGHR.Application.Interfaces.Habitaciones;
using SGHR.Application.Services.Habitaciones;
using SGHR.Persistence.Interfaces.Habitaciones;
using SGHR.Persistence.Repositories.EF.Habitaciones;

namespace SGHR.IOC.Dependencia.Habitaciones
{
    public static class AmenityDependences
    {
        public static IServiceCollection AddAmenityDependences(this IServiceCollection services)
        {
            services.AddScoped<IAmenityRepository, AmenityRepository>();
            services.AddScoped<IAmenityServices, AmenityServices>();

            return services;
        }
    }
}
