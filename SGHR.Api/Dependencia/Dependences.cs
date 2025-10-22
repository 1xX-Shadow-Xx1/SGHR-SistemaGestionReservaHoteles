using SGHR.Api.Dependencia.Reservas;
using SGHR.Api.Dependencia.Users;
using SGHR.Api.Dependencia.Operaciones;
using SGHR.Api.Dependencia.Habitaciones;

namespace SGHR.Api.Builders
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

            return services;
        }
    }
}
