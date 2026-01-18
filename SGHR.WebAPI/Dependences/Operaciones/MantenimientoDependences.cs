using SGHR.Web.Data.Interfaces.Operaciones;
using SGHR.Web.Data.Repositories.Operaciones;
using SGHR.Web.Services.Interfaces.Operaciones;
using SGHR.Web.Services.ServiceAPI.Operaciones;
using SGHR.Web.Services.SeviceMonitor.Interface.Operaciones;
using SGHR.Web.Services.SeviceMonitor.Operaciones;

namespace SGHR.Web.Dependences.Operaciones
{
    public static class MantenimientoDependences
    {
        public static IServiceCollection AddMantenimientoDependences(this IServiceCollection services)
        {
            services.AddScoped<IMantenimientoRepositoryMemory, MantenimientoRepositoryMemory>();
            services.AddScoped<IMantenimientoServiceAPI, MantenimientoServiceAPI>();
            services.AddScoped<IMantenimientoMemoryCheck, MantenimientoMemoryCheck>();

            return services;
        }
    }
}
