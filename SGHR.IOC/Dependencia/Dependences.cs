using SGHR.IOC.Dependencia.Habitaciones;
using SGHR.IOC.Dependencia.Operaciones;
using SGHR.IOC.Dependencia.Reservas;
using SGHR.IOC.Dependencia.Sesion;
using SGHR.IOC.Dependencia.Users;
using Microsoft.Extensions.DependencyInjection;

namespace SGHR.IOC.Builders
{
    public static class Dependences
    {
        public static IServiceCollection AddDependeces(this IServiceCollection services)
        {
            // Registrar servicios de aplicacion

            //Usuarios
            services = services.AddClienteDependences();
            services = services.AddUsuarioDependences();
            
            //Reservas
            services = services.AddReservaDependences();
            services = services.AddServicioAdicionalDependences();
            services = services.AddTarifaDependences();
            
            //Operaciones
            services = services.AddAuditoryDependences();
            services = services.AddCheckInOutDependences();
            services = services.AddMantenimientoDependences();
            services = services.AddPagoDependences();
            services = services.AddReporteDependences();

            //Habitaciones
            services = services.AddAmenityDependences();
            services = services.AddCategoriaDependences();
            services = services.AddHabitacionDependences();
            services = services.AddPisoDependences();

            //Sesion
            services = services.AddSesionDependences();
            services = services.AddAuthenticationDependences();


            return services;
        }
    }
}
