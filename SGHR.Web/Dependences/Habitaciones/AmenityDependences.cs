using SGHR.Web.Data.Interfaces.Habitaciones;
using SGHR.Web.Data.Repositories.Habitaciones;
using SGHR.Web.Services.Interfaces.Habitaciones;
using SGHR.Web.Services.ServiceAPI.Habitaciones;
using SGHR.Web.Services.SeviceMonitor.Habitaciones;
using SGHR.Web.Services.SeviceMonitor.Interface.Habitaciones;

namespace SGHR.Web.Dependences.Habitaciones
{
    public static class AmenityDependences
    {
        public static IServiceCollection AddAmenityDependences(this IServiceCollection services)
        {
            services.AddScoped<IAmenityRepositoryMemory, AmenityRepositoryMemory>();
            services.AddScoped<IAmenityServiceAPI, AmenityServiceAPI>();
            services.AddScoped<IAmenityMemoryCheck, AmenityMemoryCheck>();

            return services;
        }
    }
}
