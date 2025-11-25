using SGHR.Web.Data.Interfaces.Operaciones;
using SGHR.Web.Data.Repositories.Operaciones;
using SGHR.Web.Services.Interfaces.Operaciones;
using SGHR.Web.Services.ServiceAPI.Operaciones;
using SGHR.Web.Services.SeviceMonitor.Interface.Operaciones;
using SGHR.Web.Services.SeviceMonitor.Operaciones;

namespace SGHR.Web.Dependences.Operaciones
{
    public static class PagoDependences
    {
        public static IServiceCollection AddPagoDependences(this IServiceCollection services)
        {
            services.AddScoped<IPagoRepositoryMemory, PagoRepositoryMemory>();
            services.AddScoped<IPagoServiceAPI, PagoServiceAPI>();
            services.AddScoped<IPagoMemoryCheck, PagoMemoryCheck>();

            return services;
        }
    }
}
