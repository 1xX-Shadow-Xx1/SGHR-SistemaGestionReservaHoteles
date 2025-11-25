using SGHR.Web.Data.Interfaces.Operaciones;
using SGHR.Web.Data.Repositories.Operaciones;
using SGHR.Web.Services.Interfaces.Operaciones;
using SGHR.Web.Services.ServiceAPI.Operaciones;
using SGHR.Web.Services.SeviceMonitor.Interface.Operaciones;
using SGHR.Web.Services.SeviceMonitor.Operaciones;

namespace SGHR.Web.Dependences.Operaciones
{
    public static class ReporteDependences
    {
        public static IServiceCollection AddReporteDependences(this IServiceCollection services)
        {
            services.AddScoped<IReporteRepositoryMemory, ReporteRepositoryMemory>();
            services.AddScoped<IReporteServiceAPI, ReporteServiceAPI>();
            services.AddScoped<IReporteMemoryCheck, ReporteMemoryCheck>();

            return services;
        }
    }
}
