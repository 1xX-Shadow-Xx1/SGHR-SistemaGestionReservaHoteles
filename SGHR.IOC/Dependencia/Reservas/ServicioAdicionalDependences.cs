using SGHR.Persistence.Interfaces.Reservas;
using SGHR.Persistence.Repositories.EF.Reservas;
using Microsoft.Extensions.DependencyInjection;

namespace SGHR.IOC.Dependencia.Reservas
{
    public static class ServicioAdicionalDependences
    {
        public static IServiceCollection AddServicioAdicionalDependences(this IServiceCollection services)
        {
            services.AddScoped<IServicioAdicionalRepository, ServicioAdicionalRepository>();

            return services;
        }
    }
}
