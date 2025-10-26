using SGHR.Application.Interfaces.Usuarios;
using SGHR.Application.Services.Usuarios;
using SGHR.Domain.Repository;
using SGHR.Persistence.Repositories.EF.Users;

namespace SGHR.Api.Dependencia.Users
{
    public static class UsuarioDependences
    {
        public static IServiceCollection AddUsuarioDependences(this IServiceCollection services)
        {
            services.AddScoped<IUsuarioServices, UsuarioServices>();
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();

            return services;
        }
        
    }
}
