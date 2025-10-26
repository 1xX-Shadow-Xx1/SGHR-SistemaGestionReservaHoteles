using SGHR.Application.Interfaces;
using SGHR.Application.Services;

namespace SGHR.Api.Dependencia.Sesion
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
