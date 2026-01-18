using SGHR.Web.Data.Interfaces.Habitaciones;
using SGHR.Web.Data.Repositories.Habitaciones;
using SGHR.Web.Services.Interfaces.Habitaciones;
using SGHR.Web.Services.ServiceAPI.Habitaciones;
using SGHR.Web.Services.SeviceMonitor.Habitaciones;
using SGHR.Web.Services.SeviceMonitor.Interface.Habitaciones;

namespace SGHR.Web.Dependences.Habitaciones
{
    public static class PisoDependences
    {
        public static IServiceCollection AddPisoDependences(this IServiceCollection services)
        {
            services.AddScoped<IPisoRepositoryMemory, PisoRepositoryMemory>();
            services.AddScoped<IPisoServiceAPI, PisoServiceAPI>();
            services.AddScoped<IPisoMemoryCheck, PisoMemoryCheck>();

            return services;
        }
    }
}
