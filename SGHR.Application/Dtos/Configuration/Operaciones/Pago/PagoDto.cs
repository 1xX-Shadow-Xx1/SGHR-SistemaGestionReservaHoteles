
namespace SGHR.Application.Dtos.Configuration.Operaciones.Pago
{
    public class PagoDto
    {
        public int IdReserva { get; set; }
        public decimal Monto { get; set; }
        public string MetodoPago { get; set; }
        public DateTime FechaPago { get; set; } 
        public string Estado { get; set; } 
    }
}
