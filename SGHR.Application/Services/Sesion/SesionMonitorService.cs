using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SGHR.Application.Interfaces.Sesion;

namespace SGHR.Application.Services.Sesion
{
    public class SesionMonitorService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<SesionMonitorService> _logger;

        public SesionMonitorService(IServiceScopeFactory scopeFactory, ILogger<SesionMonitorService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation(" SesionMonitorService iniciado");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _scopeFactory.CreateScope())
                    {
                        // Creamos el alcance y obtenemos una instancia válida de ISesionServices
                        var sesionService = scope.ServiceProvider.GetRequiredService<ISesionServices>();

                        await sesionService.CheckActivitySesionGlobalAsync();
                    }

                    _logger.LogInformation(" Revisión de sesiones ejecutada a las {Time}", DateTime.Now);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, " Error en SesionMonitorService");
                }

                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken); // intervalo de revisión
            }

            _logger.LogInformation(" SesionMonitorService detenido");
        }
    }

}
