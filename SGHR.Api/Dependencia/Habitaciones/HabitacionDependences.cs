using SGHR.Application.Interfaces.Habitaciones;
using SGHR.Application.Services.Habitaciones;
using SGHR.Domain.Repository;
using SGHR.Persistence.Repositories.EF.Habitaciones;

namespace SGHR.Api.Dependencia.Habitaciones
{
    public static class HabitacionDependences
    {
        public static IServiceCollection AddHabitacionDependences(this IServiceCollection services)
        {
            services.AddScoped<IHabitacionServices, HabitacionServices>();
            services.AddScoped<IHabitacionRepository, HabitacionRepository>();

            return services;
        }
    }
}
