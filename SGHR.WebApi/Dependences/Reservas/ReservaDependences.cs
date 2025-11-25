using SGHR.Web.Data.Interfaces.Reservas;
using SGHR.Web.Data.Repositories.Reservas;
using SGHR.Web.Services.Interfaces.Reservas;
using SGHR.Web.Services.ServiceAPI.Reservas;
using SGHR.Web.Services.SeviceMonitor.Interface.Reservas;
using SGHR.Web.Services.SeviceMonitor.Reservas;

namespace SGHR.Web.Dependences.Reservas
{
    public static class ReservaDependences
    {
        public static IServiceCollection AddReservaDependences(this IServiceCollection services)
        {
            services.AddScoped<IReservaRepositoryMemory, ReservaRepositoryMemory>();
            services.AddScoped<IReservaServiceAPI, ReservaServiceAPI>();
            services.AddScoped<IReservaMemoryCheck, ReservaMemoryCheck>();

            return services;
        }
    }
}
