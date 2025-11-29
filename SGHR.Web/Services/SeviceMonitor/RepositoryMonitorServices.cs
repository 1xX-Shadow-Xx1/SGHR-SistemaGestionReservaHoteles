
using SGHR.Web.Services.SeviceMonitor.Interface.Habitaciones;
using SGHR.Web.Services.SeviceMonitor.Interface.Operaciones;
using SGHR.Web.Services.SeviceMonitor.Interface.Reservas;
using SGHR.Web.Services.SeviceMonitor.Interface.Usuarios;

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
                        // Usuarios
                        var usuario = scope.ServiceProvider.GetRequiredService<IUsuarioMemoryCheck>();
                        var cliente = scope.ServiceProvider.GetRequiredService<IClienteMemoryCheck>();

                        // Reservas
                        var tarifa = scope.ServiceProvider.GetRequiredService<ITarifaMemoryCheck>();
                        var servicioAdicional = scope.ServiceProvider.GetRequiredService<IServicioAdicionalMemoryCheck>();
                        var reserva = scope.ServiceProvider.GetRequiredService<IReservaMemoryCheck>();

                        // Operaciones
                        var reporte = scope.ServiceProvider.GetRequiredService<IReporteMemoryCheck>();
                        var pago = scope.ServiceProvider.GetRequiredService<IPagoMemoryCheck>();
                        var mantenimiento = scope.ServiceProvider.GetRequiredService<IMantenimientoMemoryCheck>();

                        // Habitaciones
                        var piso = scope.ServiceProvider.GetRequiredService<IPisoMemoryCheck>();
                        var habitacion = scope.ServiceProvider.GetRequiredService<IHabitacionMemoryCheck>();
                        var categoria = scope.ServiceProvider.GetRequiredService<ICategoriaMemoryCheck>();
                        var amenity = scope.ServiceProvider.GetRequiredService<IAmenityMemoryCheck>();

                        await usuario.CheckData();
                        await cliente.CheckData();

                        await tarifa.CheckData();
                        await servicioAdicional.CheckData();
                        await reserva.CheckData();

                        await reporte.CheckData();
                        await pago.CheckData();
                        await mantenimiento.CheckData();

                        await categoria.CheckData();
                        await amenity.CheckData();
                        await piso.CheckData();
                        await habitacion.CheckData();

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
