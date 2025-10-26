using Microsoft.Extensions.DependencyInjection;
using SGHR.Persistence.Interfaces.Sesiones;
using SGHR.Persistence.Repositories.EF.Sesiones;

namespace SGHR.IOC.Dependencia.Sesion
{
    public static class SesionDependences
    {
        public static IServiceCollection AddSesionDependences(this IServiceCollection services)
        {
            services.AddScoped<ISesionRepository, SesionRepository>();

            return services;
        }
    }
}
