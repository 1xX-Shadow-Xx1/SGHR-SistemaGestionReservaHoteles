using SGHR.Persistence.Interfaces.Habitaciones;
using SGHR.Persistence.Repositories.EF.Habitaciones;

namespace SGHR.Api.Dependencia.Habitaciones
{
    public static class AmenityDependences
    {
        public static IServiceCollection AddAmenityDependences(this IServiceCollection services)
        {
            services.AddScoped<IAmenityRepository, AmenityRepository>();

            return services;
        }
    }
}
