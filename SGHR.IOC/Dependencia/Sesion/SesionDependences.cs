using Microsoft.Extensions.DependencyInjection;
using SGHR.Application.Interfaces.Sesion;
using SGHR.Application.Services.Sesion;
using SGHR.Persistence.Interfaces.Sesiones;
using SGHR.Persistence.Repositories.EF.Sesiones;

namespace SGHR.IOC.Dependencia.Sesion
{
    public static class SesionDependences
    {
        public static IServiceCollection AddSesionDependences(this IServiceCollection services)
        {
            services.AddScoped<ISesionRepository, SesionRepository>();
            services.AddScoped<ISesionServices, SesionServices>();
            services.AddHostedService<SesionMonitorService>();


            return services;
        }
    }
}
