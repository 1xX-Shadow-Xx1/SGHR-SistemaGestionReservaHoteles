using SGHR.Persistence.Interfaces.Reportes;
using SGHR.Persistence.Repositories.EF.Operaciones;

namespace SGHR.Api.Dependencia.Operaciones
{
    public static class PagoDependences
    {
        public static IServiceCollection AddPagoDependences(this IServiceCollection services)
        {
            services.AddScoped<IPagoRepository, PagoRepository>();

            return services;
        }
    }
}
