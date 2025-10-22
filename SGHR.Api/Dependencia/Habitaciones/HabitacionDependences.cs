using SGHR.Application.Interfaces.Habitaciones;
using SGHR.Application.Services.Categorias;
using SGHR.Domain.Repository;
using SGHR.Persistence.Interfaces.Habitaciones;
using SGHR.Persistence.Repositories.EF.Habitaciones;

namespace SGHR.Api.Dependencia.Habitaciones
{
    public static class HabitacionDependences
    {
        public static IServiceCollection AddHabitacionDependences(this IServiceCollection services)
        {
            services.AddScoped<IHabitacionService, HabitacionService>();
            services.AddScoped<IHabitacionRepository, HabitacionRepository>();

            return services;
        }
    }
}
