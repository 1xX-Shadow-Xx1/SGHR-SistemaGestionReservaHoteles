using SGHR.Domain.Base;
using SGHR.Domain.Entities.Configuration.Reservas;
using SGHR.Domain.Enum.Reservas;
using SGHR.Domain.Validators.Configuration;
using SGHR.Domain.Validators.ConfigurationRules;

namespace SGHR.Domain.Validators.Reservas
{
    public static class ReservaValidator
    {
        public static OperationResult<Reserva> Validate(Reserva reserva)
        {

            var rules = new List<Func<Reserva, (bool, string)>>
            {
                RuleHelper.RequeredNumInt<Reserva>(u => u.IdHabitacion,"El numero de la habitacion"),
                RuleHelper.RequeredNumDecimal<Reserva>(u => u.CostoTotal,"El costo"),
                RuleHelper.FutureOrTodayDate<Reserva>(u => u.FechaInicio, "La fecha de inicio"),
                RuleHelper.FutureDate<Reserva>(u => u.FechaFin,"La fecha de fin"),
                EnumRuleHelper.ValidEnum<Reserva, EstadoReserva>(u => u.Estado,"El estado de la reserva")
            };

            return ValidatorHelper.Validate(reserva, rules, "La reserva");
        }
    }
}
