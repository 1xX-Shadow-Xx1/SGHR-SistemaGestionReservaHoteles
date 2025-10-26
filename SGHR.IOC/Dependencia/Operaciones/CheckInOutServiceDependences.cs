using SGHR.Persistence.Interfaces.Operaciones;
using SGHR.Persistence.Repositories.EF.Operaciones;
using Microsoft.Extensions.DependencyInjection;

namespace SGHR.IOC.Dependencia.Operaciones
{
    public static class CheckInOutServiceDependences
    {
        public static IServiceCollection AddCheckInOutDependences(this IServiceCollection services)
        {
            services.AddScoped<ICheckInOutRepository, CheckInOutRepository>();

            return services;
        }
    }
}
