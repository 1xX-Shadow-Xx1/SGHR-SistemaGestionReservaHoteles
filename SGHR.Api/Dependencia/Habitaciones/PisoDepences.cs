using SGHR.Application.Interfaces.Habitaciones;
using SGHR.Application.Services.Categorias;
using SGHR.Persistence.Interfaces.Habitaciones;
using SGHR.Persistence.Repositories.EF.Habitaciones;

namespace SGHR.Api.Dependencia.Habitaciones
{
    public static class PisoDepences
    {
        public static IServiceCollection AddPisoDependences(this IServiceCollection services)
        {
            services.AddScoped<IPisoService, PisoService>();
            services.AddScoped<IPisoRepository, PisoRepository>();

            return services;
        }
    }
}
