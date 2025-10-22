using SGHR.Application.Interfaces.Operaciones;
using SGHR.Application.Interfaces.Reservas;
using SGHR.Application.Services.Operaciones;
using SGHR.Application.Services.Reservas;
using SGHR.Domain.Repository;
using SGHR.Persistence.Interfaces.Reportes;
using SGHR.Persistence.Repositories.EF.Operaciones;
using SGHR.Persistence.Repositories.EF.Reservas;

namespace SGHR.Api.Dependencia.Reservas
{
    public static class ReservaDependences
    {
        public static IServiceCollection AddReservaDependences(this IServiceCollection services)
        {
            services.AddScoped<IReservaService, ReservaService>();
            services.AddScoped<IReservaRepository, ReservaRepository>();

            return services;
        }
    }
}
