using SGHR.Domain.Enum.Operaciones;
using System.ComponentModel.DataAnnotations;

namespace SGHR.Application.Dtos.Configuration.Operaciones.Pago
{
    public class RealizarPagoDto
    {
        public int IdReserva { get; set; }
        public decimal Monto { get; set; }
        public MetodoPago MetodoPago { get; set; }
    }
}
