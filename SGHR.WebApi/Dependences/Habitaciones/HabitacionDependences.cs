using SGHR.Web.Data.Interfaces.Habitaciones;
using SGHR.Web.Data.Repositories.Habitaciones;
using SGHR.Web.Services.Interfaces.Habitaciones;
using SGHR.Web.Services.ServiceAPI.Habitaciones;
using SGHR.Web.Services.SeviceMonitor.Habitaciones;
using SGHR.Web.Services.SeviceMonitor.Interface.Habitaciones;

namespace SGHR.Web.Dependences.Habitaciones
{
    public static class HabitacionDependences
    {
        public static IServiceCollection AddHabitacionDependences(this IServiceCollection services)
        {
            services.AddScoped<IHabitacionRepositoryMemory, HabitacionRepositoryMemory>();
            services.AddScoped<IHabitacionServiceAPI, HabitacionServiceAPI>();
            services.AddScoped<IHabitacionMemoryCheck, HabitacionMemoryCheck>();

            return services;
        }
    }
}
