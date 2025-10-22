using SGHR.Application.Interfaces.Reservas;
using SGHR.Application.Interfaces.Users;
using SGHR.Application.Services.Reservas;
using SGHR.Application.Services.Users;
using SGHR.Persistence.Interfaces.Reservas;
using SGHR.Persistence.Interfaces.Users;
using SGHR.Persistence.Repositories.EF.Reservas;
using SGHR.Persistence.Repositories.EF.Users;

namespace SGHR.Api.Dependencia.Users
{
    public static class ClienteDependences
    {
        public static IServiceCollection AddClienteDependences(this IServiceCollection services)
        {
            services.AddScoped<IClienteService, ClienteService>();
            services.AddScoped<IClienteRepository, ClienteRepository>();

            return services;
        }
    }
}
