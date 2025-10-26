using Microsoft.Extensions.DependencyInjection;
using SGHR.Application.Interfaces;
using SGHR.Application.Services;

namespace SGHR.IOC.Dependencia.Sesion
{
    public static class AuthenticationDependences
    {
        public static IServiceCollection AddAuthenticationDependences(this IServiceCollection services)
        {
            services.AddScoped<IAuthenticationServices, AuthenticationServices>();

            return services;
        }
    }
}
