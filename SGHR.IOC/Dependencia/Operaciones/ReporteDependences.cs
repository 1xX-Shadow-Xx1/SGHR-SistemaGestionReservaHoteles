using SGHR.Application.Interfaces.Operaciones;
using SGHR.Application.Services.Operaciones;
using SGHR.Persistence.Interfaces.Reportes;
using SGHR.Persistence.Repositories.EF.Operaciones;
using Microsoft.Extensions.DependencyInjection;

namespace SGHR.IOC.Dependencia.Operaciones
{
    public static class ReporteDependences
    {
        public static IServiceCollection AddReporteDependences(this IServiceCollection services)
        {
            services.AddScoped<IReporteServices, ReporteServices>();
            services.AddScoped<IReporteRepository, ReporteRepository>();

            return services;
        }
    }
}
