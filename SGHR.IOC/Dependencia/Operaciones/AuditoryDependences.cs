using SGHR.Persistence.Interfaces.Operaciones;
using SGHR.Persistence.Repositories.EF.Operaciones;
using Microsoft.Extensions.DependencyInjection;

namespace SGHR.IOC.Dependencia.Operaciones
{
    public static class AuditoryDependences
    {
        public static IServiceCollection AddAuditoryDependences(this IServiceCollection services)
        {
            services.AddScoped<IAuditoryRepository, AuditoryRepository>();

            return services;
        }
    }
}
