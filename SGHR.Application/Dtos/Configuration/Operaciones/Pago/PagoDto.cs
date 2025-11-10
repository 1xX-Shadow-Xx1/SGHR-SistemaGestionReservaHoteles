
using SGHR.Domain.Enum.Operaciones;

namespace SGHR.Application.Dtos.Configuration.Operaciones.Pago
{
    public class PagoDto
    {
        public int Id { get; set; }
        public int IdReserva { get; set; }
        public decimal Monto { get; set; }
        public MetodoPago MetodoPago { get; set; }
        public DateTime FechaPago { get; set; } 
        public string Estado { get; set; } 
    }
}
