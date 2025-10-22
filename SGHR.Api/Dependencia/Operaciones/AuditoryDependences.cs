using SGHR.Application.Interfaces.Habitaciones;
using SGHR.Application.Interfaces.Operaciones;
using SGHR.Application.Services.Categorias;
using SGHR.Application.Services.Operaciones;
using SGHR.Persistence.Interfaces.Habitaciones;
using SGHR.Persistence.Interfaces.Operaciones;
using SGHR.Persistence.Repositories.EF.Habitaciones;
using SGHR.Persistence.Repositories.EF.Operaciones;

namespace SGHR.Api.Dependencia.Operaciones
{
    public static class AuditoryDependences
    {
        public static IServiceCollection AddAuditoryDependences(this IServiceCollection services)
        {
            services.AddScoped<IAuditoryService, AuditoryService>();
            services.AddScoped<IAuditoryRepository, AuditoryRepository>();

            return services;
        }
    }
}
