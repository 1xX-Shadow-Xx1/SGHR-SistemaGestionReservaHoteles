using SGHR.Web.Data.Interfaces.Habitaciones;
using SGHR.Web.Data.Repositories.Habitaciones;
using SGHR.Web.Services.Interfaces.Habitaciones;
using SGHR.Web.Services.ServiceAPI.Habitaciones;
using SGHR.Web.Services.SeviceMonitor.Habitaciones;
using SGHR.Web.Services.SeviceMonitor.Interface.Habitaciones;

namespace SGHR.Web.Dependences.Habitaciones
{
    public static class CategoriaDependences
    {
        public static IServiceCollection AddCategoriaDependences(this IServiceCollection services)
        {
            services.AddScoped<ICategoriaRepositoryMemory, CategoriaRepositoryMemory>();
            services.AddScoped<ICategoriaServiceAPI, CategoriaServiceAPI>();
            services.AddScoped<ICategoriaMemoryCheck, CategoriaMemoryCheck>();

            return services;
        }
    }
}
