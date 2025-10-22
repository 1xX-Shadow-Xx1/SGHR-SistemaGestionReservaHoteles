using SGHR.Application.Interfaces.Reservas;
using SGHR.Application.Services.Reservas;
using SGHR.Domain.Repository;
using SGHR.Persistence.Interfaces.Reservas;
using SGHR.Persistence.Repositories.EF.Reservas;

namespace SGHR.Api.Dependencia.Reservas
{
    public static class ServicioAdicionalDependences
    {
        public static IServiceCollection AddServicioAdicionalDependences(this IServiceCollection services)
        {
            services.AddScoped<IServicioAdicionalService, ServicioAdicionalService>();
            services.AddScoped<IServicioAdicionalRepository, ServicioAdicionalRepository>();

            return services;
        }
    }
}
