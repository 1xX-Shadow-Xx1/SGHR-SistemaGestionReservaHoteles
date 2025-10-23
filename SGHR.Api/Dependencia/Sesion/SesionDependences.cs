using SGHR.Application.Interfaces.Sesiones;
using SGHR.Application.Services.Sesiones;
using SGHR.Persistence.Interfaces.Sesiones;
using SGHR.Persistence.Repositories.EF.Sesiones;

namespace SGHR.Api.Dependencia.Sesion
{
    public static class SesionDependences
    {
        public static IServiceCollection AddSesionDependences(this IServiceCollection services)
        {
            services.AddScoped<ISesionServices ,SesionesServices>();
            services.AddScoped<ISesionRepository, SesionRepository>();

            return services;
        }
    }
}
