using SGHR.Application.Dtos.Configuration.Operaciones.Pago;
using SGHR.Application.Dtos.Configuration.Reservas.Reserva;

namespace SGHR.Web.Areas.Administrador.Models
{
    public class DashboardViewModel
    {
        public ResumenPagoDto Resumen { get; set; } = new();
        public IEnumerable<PagoDto> Pagos { get; set; } = new List<PagoDto>();
        public IEnumerable<ReservaDto> Reservas { get; set; } = new List<ReservaDto>();
    }
}
