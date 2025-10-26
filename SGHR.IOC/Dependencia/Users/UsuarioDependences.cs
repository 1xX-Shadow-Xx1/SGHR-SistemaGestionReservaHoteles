using SGHR.Application.Interfaces.Usuarios;
using SGHR.Application.Services.Usuarios;
using SGHR.Domain.Repository;
using SGHR.Persistence.Repositories.EF.Users;
using Microsoft.Extensions.DependencyInjection;

namespace SGHR.IOC.Dependencia.Users
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
