using SGHR.Application.Interfaces.Operaciones;
using SGHR.Application.Services.Operaciones;
using SGHR.Persistence.Interfaces.Reportes;
using SGHR.Persistence.Repositories.EF.Operaciones;
using Microsoft.Extensions.DependencyInjection;

namespace SGHR.IOC.Dependencia.Operaciones
{
    public static class PagoDependences
    {
        public static IServiceCollection AddPagoDependences(this IServiceCollection services)
        {
            services.AddScoped<IPagoRepository, PagoRepository>();
            services.AddScoped<IPagoServices, PagoServices>();

            return services;
        }
    }
}
