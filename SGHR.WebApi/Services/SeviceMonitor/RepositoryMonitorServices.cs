
using SGHR.Web.Services.SeviceMonitor.Interface.Reservas;
using SGHR.Web.Services.SeviceMonitor.Interface.Usuarios;
using SGHR.Web.Services.SeviceTwoPlane.Usuarios;

namespace SGHR.Web.Services.SeviceMonitor
{
    public class RepositoryMonitorServices : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<RepositoryMonitorServices> _logger;

        public RepositoryMonitorServices(IServiceScopeFactory serviceScopeFactory,
                                         ILogger<RepositoryMonitorServices> logger)
        {
            _scopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation(" RepositoryMonitorServices iniciado");
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _scopeFactory.CreateScope())
                    {
                        var usuario = scope.ServiceProvider.GetRequiredService<IUsuarioMemoryCheck>();
                        var cliente = scope.ServiceProvider.GetRequiredService<IClienteMemoryCheck>();
                        var tarifa = scope.ServiceProvider.GetRequiredService<ITarifaMemoryCheck>();
                        var servicioAdicional = scope.ServiceProvider.GetRequiredService<IServicioAdicionalMemoryCheck>();
                        var reserva = scope.ServiceProvider.GetRequiredService<IReservaMemoryCheck>();

                        await usuario.CheckData();
                        await cliente.CheckData();
                        await tarifa.CheckData();
                        await servicioAdicional.CheckData();
                        await reserva.CheckData();
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error en RepositoryMonitorServices.");
                }

                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken); // intervalo de revisión
            }
        }
    }
}
