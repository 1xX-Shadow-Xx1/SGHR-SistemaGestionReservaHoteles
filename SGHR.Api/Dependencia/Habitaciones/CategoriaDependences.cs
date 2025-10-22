using SGHR.Application.Interfaces.Habitaciones;
using SGHR.Application.Services.Categorias;
using SGHR.Persistence.Interfaces.Habitaciones;
using SGHR.Persistence.Repositories.EF.Habitaciones;

namespace SGHR.Api.Dependencia.Habitaciones
{
    public static class CategoriaDependences
    {
        public static IServiceCollection AddCategoriaDependences(this IServiceCollection services)
        {
            services.AddScoped<ICategoriaService, CategiriaService>();
            services.AddScoped<ICategoriaRepository, CategoriaRepository>();

            return services;
        }
    }
}
