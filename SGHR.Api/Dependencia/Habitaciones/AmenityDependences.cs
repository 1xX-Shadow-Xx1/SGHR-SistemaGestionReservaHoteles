using SGHR.Application.Interfaces.Habitaciones;
using SGHR.Application.Services.Categorias;
using SGHR.Persistence.Interfaces.Habitaciones;
using SGHR.Persistence.Repositories.EF.Habitaciones;

namespace SGHR.Api.Dependencia.Habitaciones
{
    public static class AmenityDependences
    {
        public static IServiceCollection AddAmenityDependences(this IServiceCollection services)
        {
            services.AddScoped<IAmenityService, AmenityService>();
            services.AddScoped<IAmenityRepository, AmenityRepository>();

            return services;
        }
    }
}
