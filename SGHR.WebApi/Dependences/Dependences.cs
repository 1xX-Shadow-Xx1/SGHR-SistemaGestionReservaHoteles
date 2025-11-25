using SGHR.Web.Dependences.Reservas;
using SGHR.Web.Dependences.Usuarios;
using SGHR.Web.Services.ClienteAPIService;
using SGHR.Web.Services.ClienteAPIService.Interface;
using SGHR.Web.Services.SeviceMonitor;

namespace SGHR.Web.Dependences
{
    public static class Dependences
    {
        public static IServiceCollection AddDependences(this IServiceCollection services)
        {

            services = services.AddUsuarioDependences();
            services = services.AddClienteDependences();
            services = services.AddReservaDependences();
            services = services.AddServicioAdicionalDepnedences();
            services = services.AddTarifaDependences();
            
            services.AddScoped(typeof(IClientAPI<>), typeof(ClienteAPI<>));

            services.AddHostedService<RepositoryMonitorServices>();

            return services;
        }
    }
}
