using SGHR.Domain.Entities.Configuration.Operaciones;
using SGHR.Domain.Enum.Operaciones;


namespace SGHR.Domain.Validators.ConfigurationRules.Operaciones
{
    public class PagoValidator
    {
        public bool Validate(Pago pago, out string errorMessage)
        {
            if (!ValidationHelper.NotNull(pago, "Pago", out errorMessage)) return false;

            // FK Reserva
            if (!ValidationHelper.GreaterThanZero(pago.IdReserva, "IdReserva", out errorMessage)) return false;

            // Monto
            if (!ValidationHelper.GreaterThanZero(pago.Monto, "Monto", out errorMessage)) return false;

            // Método de pago
            if (!ValidationHelper.Required(pago.MetodoPago.ToString(), "Método de Pago", out errorMessage)) return false;
            var metodosValidos = new[] { MetodoPago.Efectivo, MetodoPago.TransferenciaBancaria, MetodoPago.TarjetaDebito, MetodoPago.TarjetaCredito };
            if (!ValidationHelper.InList(pago.MetodoPago, metodosValidos, "El Metodo de pago ", out errorMessage)) return false;

            // Fecha de pago
            if (pago.FechaPago == DateTime.MinValue)
            {
                errorMessage = "Fecha de pago es obligatoria.";
                return false;
            }

            // Estado (opcional)
            var estadosValidos = new [] { EstadoPago.Completado, EstadoPago.Pendiente, EstadoPago.Rechazado, EstadoPago.Parcial }; 
            if (!ValidationHelper.InList(pago.Estado, estadosValidos, "Estado", out errorMessage)) return false;

            errorMessage = string.Empty;
            return true;
        }
    }
}
