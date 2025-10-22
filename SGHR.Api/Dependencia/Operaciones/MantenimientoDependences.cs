using SGHR.Application.Interfaces.Operaciones;
using SGHR.Application.Services.Operaciones;
using SGHR.Persistence.Interfaces.Operaciones;
using SGHR.Persistence.Repositories.EF.Operaciones;

namespace SGHR.Api.Dependencia.Operaciones
{
    public static class MantenimientoDependences
    {
        public static IServiceCollection AddMantenimientoDependences(this IServiceCollection services)
        {
            services.AddScoped<IMantenimientoService, MantenimientoService>();
            services.AddScoped<IMantenimientoRepository, MantenimientoRepository>();

            return services;
        }
    }
}
