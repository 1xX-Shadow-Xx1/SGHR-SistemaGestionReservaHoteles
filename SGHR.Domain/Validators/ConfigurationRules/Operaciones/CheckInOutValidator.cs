using SGHR.Domain.Entities.Configuration.Operaciones;

namespace SGHR.Domain.Validators.ConfigurationRules.Operaciones
{
    public class CheckInOutValidator
    {
        public bool Validate(CheckInOut check, out string errorMessage)
        {
            if (!ValidationHelper.NotNull(check, "CheckInOut", out errorMessage)) return false;

            // FK Reserva
            if (!ValidationHelper.GreaterThanZero(check.IdReserva, "IdReserva", out errorMessage)) return false;

            // Fecha CheckIn (opcional)
            if (check.FechaCheckIn.HasValue && check.FechaCheckIn == DateTime.MinValue)
            {
                errorMessage = "Fecha CheckIn inválida.";
                return false;
            }

            // Fecha CheckOut (opcional)
            if (check.FechaCheckOut.HasValue)
            {
                if (check.FechaCheckOut == DateTime.MinValue)
                {
                    errorMessage = "Fecha CheckOut inválida.";
                    return false;
                }
                if (check.FechaCheckIn.HasValue && check.FechaCheckOut < check.FechaCheckIn)
                {
                    errorMessage = "Fecha CheckOut no puede ser anterior a Fecha CheckIn.";
                    return false;
                }
            }

            // FK Usuario que atendió
            if (!ValidationHelper.GreaterThanZero(check.AtendidoPor, "AtendidoPor", out errorMessage)) return false;

            errorMessage = string.Empty;
            return true;
        }
    }
}
