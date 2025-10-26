using SGHR.Application.Interfaces.Reservas;
using SGHR.Application.Services.Reservas;
using SGHR.Persistence.Interfaces.Reservas;
using SGHR.Persistence.Repositories.EF.Reservas;
using Microsoft.Extensions.DependencyInjection;

namespace SGHR.IOC.Dependencia.Reservas
{
    public static class TarifaDependences
    {
        public static IServiceCollection AddTarifaDependences(this IServiceCollection services)
        {
            services.AddScoped<ITarifaServices, TarifaServices>();
            services.AddScoped<ITarifaRepository, TarifaRepository>();

            return services;
        }
    }
}
