using SGHR.Application.Interfaces.Users;
using SGHR.Application.Services.Users;
using SGHR.Domain.Repository;
using SGHR.Persistence.Repositories.EF.Users;

namespace SGHR.Api.Dependencia.Users
{
    public static class UsuarioDependences
    {
        public static IServiceCollection AddUsuarioDependences(this IServiceCollection services)
        {
            services.AddScoped<IUsuarioService, UsuarioService>();
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();

            return services;
        }
        
    }
}
