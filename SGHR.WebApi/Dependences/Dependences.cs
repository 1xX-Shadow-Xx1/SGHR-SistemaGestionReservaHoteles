using SGHR.Web.Data;
using SGHR.Web.Dependences.Habitaciones;
using SGHR.Web.Dependences.Operaciones;
using SGHR.Web.Dependences.Reservas;
using SGHR.Web.Dependences.Usuarios;
using SGHR.Web.Services.ClienteAPIService;
using SGHR.Web.Services.ClienteAPIService.Interface;
using SGHR.Web.Services.Interfaces;
using SGHR.Web.Services.Interfaces.Authentification;
using SGHR.Web.Services.ServiceAPI;
using SGHR.Web.Services.ServiceAPI.Authentification;
using SGHR.Web.Services.SeviceMonitor;

namespace SGHR.Web.Dependences
{
    public static class Dependences
    {
        public static IServiceCollection AddDependences(this IServiceCollection services)
        {
            // Dependencias de Usuarios
            services = services.AddUsuarioDependences();
            services = services.AddClienteDependences();

            // Dependencias de Reservas
            services = services.AddReservaDependences();
            services = services.AddServicioAdicionalDepnedences();
            services = services.AddTarifaDependences();

            // Dependencias de Operaciones
            services = services.AddReporteDependences();
            services = services.AddPagoDependences();
            services = services.AddMantenimientoDependences();

            // Dependencias de Habitaciones
            services = services.AddPisoDependences();
            services = services.AddHabitacionDependences();
            services = services.AddCategoriaDependences();
            services = services.AddAmenityDependences();
            
            services.AddScoped(typeof(IClientAPI<>), typeof(ClienteAPI<>));
            services.AddScoped<IAuthentificationServiceAPI, AuthentificationServiceAPI>();
            services.AddHostedService<RepositoryMonitorServices>();
            services.AddScoped<IDashBoardServiceAPI, DashBoardServiceAPI>();

            return services;
        }
    }
}
