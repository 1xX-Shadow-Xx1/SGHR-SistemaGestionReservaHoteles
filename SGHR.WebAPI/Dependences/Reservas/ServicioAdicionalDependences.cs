using SGHR.Web.Data.Interfaces.Reservas;
using SGHR.Web.Data.Repositories.Reservas;
using SGHR.Web.Services.Interfaces.Reservas;
using SGHR.Web.Services.ServiceAPI.Reservas;
using SGHR.Web.Services.SeviceMonitor.Interface.Reservas;
using SGHR.Web.Services.SeviceMonitor.Reservas;

namespace SGHR.Web.Dependences.Reservas
{
    public static class ServicioAdicionalDependences
    {
        public static IServiceCollection AddServicioAdicionalDepnedences(this IServiceCollection services)
        {
            services.AddScoped<IServicioAdicionalRepositoryMemory, ServicioAdicionalRepositoryMemory>();
            services.AddScoped<IServicioAdicionalServiceAPI, ServicioAdicionalServiceAPI>();
            services.AddScoped<IServicioAdicionalMemoryCheck, ServicioAdicionalMemoryCheck>();

            return services;
        }
    }
}
