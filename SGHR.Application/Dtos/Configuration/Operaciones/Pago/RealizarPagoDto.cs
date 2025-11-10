using SGHR.Domain.Enum.Operaciones;
using System.ComponentModel.DataAnnotations;

namespace SGHR.Application.Dtos.Configuration.Operaciones.Pago
{
    public class RealizarPagoDto
    {
        [Required(ErrorMessage = "El ID de la reserva es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID de la reserva debe ser mayor a 0.")]
        public int IdReserva { get; set; }

        [Required(ErrorMessage = "El monto es obligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor a 0.")]
        public decimal Monto { get; set; }


        public MetodoPago MetodoPago { get; set; }
    }
}
