using SGHR.Domain.Entities.Configuration.Reservas;
using SGHR.Domain.Enum.Reservas;

namespace SGHR.Domain.Validators.ConfigurationRules.Reservas
{
    public class ReservaValidator
    {
        public bool Validate(Reserva reserva, out string errorMessage)
        {
            if (!ValidationHelper.NotNull(reserva, "Reserva", out errorMessage)) return false;
            if (!ValidationHelper.GreaterThanZero(reserva.IdCliente, "IdCliente", out errorMessage)) return false;
            if (!ValidationHelper.GreaterThanZero(reserva.IdHabitacion, "IdHabitacion", out errorMessage)) return false;
            if (reserva.FechaInicio == DateTime.MinValue)
            {
                errorMessage = "Fecha de inicio es obligatoria.";
                return false;
            }
            if (reserva.FechaFin == DateTime.MinValue)
            {
                errorMessage = "Fecha de fin es obligatoria.";
                return false;
            }
            if (reserva.FechaFin < reserva.FechaInicio)
            {
                errorMessage = "Fecha de fin no puede ser anterior a la fecha de inicio.";
                return false;
            }
            if (!ValidationHelper.GreaterThanZero(reserva.CostoTotal, "CostoTotal", out errorMessage)) return false;
            var estadosValidos = new[] { EstadoReserva.Pendiente, EstadoReserva.Confirmada, EstadoReserva.Cancelada }; 
            if (!ValidationHelper.InList(reserva.Estado, estadosValidos, "Estado", out errorMessage))

                errorMessage = string.Empty;
            return true;
        }
    }
}
