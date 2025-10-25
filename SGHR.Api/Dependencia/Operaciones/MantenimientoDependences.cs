using SGHR.Persistence.Interfaces.Operaciones;
using SGHR.Persistence.Repositories.EF.Operaciones;

namespace SGHR.Api.Dependencia.Operaciones
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
