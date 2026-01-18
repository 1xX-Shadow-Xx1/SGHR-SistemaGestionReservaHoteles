using SGHR.Web.Data.Interfaces.Usuarios;
using SGHR.Web.Data.Repositories.Usuarios;
using SGHR.Web.Services.Interfaces.Usuarios;
using SGHR.Web.Services.ServiceAPI.Usuarios;
using SGHR.Web.Services.SeviceMonitor.Interface.Usuarios;
using SGHR.Web.Services.SeviceMonitor.Usuarios;

namespace SGHR.Web.Dependences.Usuarios
{
    public static class ClienteDependences
    {
        public static IServiceCollection AddClienteDependences(this IServiceCollection services)
        {
            services.AddScoped<IClienteRepositoryMemory, ClienteRepositoryMemory>();
            services.AddScoped<IClienteServiceAPI, ClienteServiceAPI>();
            services.AddScoped<IClienteMemoryCheck, ClienteMemoryCheck>();

            return services;
        }
    }
}
