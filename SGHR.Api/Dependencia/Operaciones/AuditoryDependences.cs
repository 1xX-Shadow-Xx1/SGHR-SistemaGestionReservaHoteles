using SGHR.Persistence.Interfaces.Operaciones;
using SGHR.Persistence.Repositories.EF.Operaciones;

namespace SGHR.Api.Dependencia.Operaciones
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
