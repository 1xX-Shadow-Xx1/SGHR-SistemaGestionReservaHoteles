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

        [Required(ErrorMessage = "El método de pago es obligatorio.")]
        [StringLength(50, ErrorMessage = "El método de pago no puede superar los 50 caracteres.")]
        public string MetodoPago { get; set; }
    }
}
