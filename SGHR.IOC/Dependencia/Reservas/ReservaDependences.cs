
using SGHR.Application.Interfaces.Reservas;
using SGHR.Application.Services.Reservas;
using SGHR.Domain.Repository;
using SGHR.Persistence.Repositories.EF.Reservas;
using Microsoft.Extensions.DependencyInjection;

namespace SGHR.IOC.Dependencia.Reservas
{
    public static class ReservaDependences
    {
        public static IServiceCollection AddReservaDependences(this IServiceCollection services)
        {
            services.AddScoped<IReservaServices, ReservaServices>();
            services.AddScoped<IReservaRepository, ReservaRepository>();

            return services;
        }
    }
}
