using Microsoft.Extensions.Hosting;
using SGHR.Application.Interfaces.Sesion;

namespace SGHR.Application.Services.Sesion
{
    public class SesionMonitorService : BackgroundService
    {
        private readonly ISesionServices _sesionServices;

        public SesionMonitorService(ISesionServices sesionServices)
        {
            _sesionServices = sesionServices;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await _sesionServices.CheckActivitySesionGlobalAsync();
                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken); // revisa cada 5 minutos
            }
        }
    }

}
