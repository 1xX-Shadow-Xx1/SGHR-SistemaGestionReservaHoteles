using SGHR.Persistence.Interfaces.Reservas;
using SGHR.Persistence.Repositories.EF.Reservas;
using Microsoft.Extensions.DependencyInjection;
using SGHR.Application.Interfaces.Reservas;
using SGHR.Application.Services.Reservas;

namespace SGHR.IOC.Dependencia.Reservas
{
    public static class ServicioAdicionalDependences
    {
        public static IServiceCollection AddServicioAdicionalDependences(this IServiceCollection services)
        {
            services.AddScoped<IServicioAdicionalRepository, ServicioAdicionalRepository>();
            services.AddScoped<IServicioAdicionalServices, ServicioAdicionalServices>();

            return services;
        }
    }
}
