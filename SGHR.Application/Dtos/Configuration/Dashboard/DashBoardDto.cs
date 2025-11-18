

using SGHR.Application.Dtos.Configuration.Operaciones.Pago;
using SGHR.Application.Dtos.Configuration.Reservas.Reserva;

namespace SGHR.Application.Dtos.Configuration.Dashboard
{
    public class DashBoardDto
    {
        public IEnumerable<ReservaDto>? Reserva { get; set; } = new List<ReservaDto>();
        public IEnumerable<PagoDto>? Pago { get; set; } = new List<PagoDto>();
        public ResumenPagoDto? ResumenPago { get; set; }
    }
}
