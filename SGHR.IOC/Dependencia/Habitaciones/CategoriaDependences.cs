using Microsoft.Extensions.DependencyInjection;
using SGHR.Application.Interfaces.Habitaciones;
using SGHR.Application.Services.Habitaciones;
using SGHR.Persistence.Interfaces.Habitaciones;
using SGHR.Persistence.Repositories.EF.Habitaciones;

namespace SGHR.IOC.Dependencia.Habitaciones
{
    public static class CategoriaDependences
    {
        public static IServiceCollection AddCategoriaDependences(this IServiceCollection services)
        {
            services.AddScoped<ICategoriaServices, CategoriaServices>();
            services.AddScoped<ICategoriaRepository, CategoriaRepository>();

            return services;
        }
    }
}
