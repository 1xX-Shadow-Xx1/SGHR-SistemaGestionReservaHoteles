using SGHR.Web.Data.Interfaces.Reservas;
using SGHR.Web.Data.Repositories.Reservas;
using SGHR.Web.Services.Interfaces.Reservas;
using SGHR.Web.Services.ServiceAPI.Reservas;
using SGHR.Web.Services.SeviceMonitor.Interface.Reservas;
using SGHR.Web.Services.SeviceMonitor.Reservas;

namespace SGHR.Web.Dependences.Reservas
{
    public static class TarifaDependences
    {
        public static IServiceCollection AddTarifaDependences(this IServiceCollection services)
        {
            services.AddScoped<ITarifaRepositoryMemory, TarifaRepositoryMemory>();
            services.AddScoped<ITarifaServiceAPI, TarifaServiceAPI>();
            services.AddScoped<ITarifaMemoryCheck, TarifaMemoryCheck>();

            return services;
        }
    }
}
