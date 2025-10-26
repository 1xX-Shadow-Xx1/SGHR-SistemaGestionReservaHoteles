using SGHR.Persistence.Interfaces.Operaciones;
using SGHR.Persistence.Repositories.EF.Operaciones;
using Microsoft.Extensions.DependencyInjection;

namespace SGHR.IOC.Dependencia.Operaciones
{
    public static class MantenimientoDependences
    {
        public static IServiceCollection AddMantenimientoDependences(this IServiceCollection services)
        {
            services.AddScoped<IMantenimientoRepository, MantenimientoRepository>();

            return services;
        }
    }
}
