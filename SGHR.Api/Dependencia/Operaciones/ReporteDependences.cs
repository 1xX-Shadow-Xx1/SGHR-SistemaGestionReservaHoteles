using SGHR.Application.Interfaces.Operaciones;
using SGHR.Application.Services.Operaciones;
using SGHR.Persistence.Interfaces.Reportes;
using SGHR.Persistence.Repositories.EF.Operaciones;

namespace SGHR.Api.Dependencia.Operaciones
{
    public static class ReporteDependences
    {
        public static IServiceCollection AddReporteDependences(this IServiceCollection services)
        {
            services.AddScoped<IReporteService, ReporteService>();
            services.AddScoped<IReporteRepository, ReporteRepository>();

            return services;
        }
    }
}
