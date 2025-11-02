using SGHR.Persistence.Interfaces.Operaciones;
using SGHR.Persistence.Repositories.EF.Operaciones;
using Microsoft.Extensions.DependencyInjection;
using SGHR.Application.Interfaces.Operaciones;
using SGHR.Application.Services.Operaciones;

namespace SGHR.IOC.Dependencia.Operaciones
{
    public static class MantenimientoDependences
    {
        public static IServiceCollection AddMantenimientoDependences(this IServiceCollection services)
        {
            services.AddScoped<IMantenimientoRepository, MantenimientoRepository>();
            services.AddScoped<IMantenimientoServices, MantenimientoServcices>();

            return services;
        }
    }
}
