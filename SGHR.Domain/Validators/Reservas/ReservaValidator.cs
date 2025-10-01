using SGHR.Domain.Base;
using SGHR.Domain.Entities.Configuration.Reservas;
using SGHR.Domain.Validators.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                RuleHelper.FutureDate<Reserva>(u => u.FechaInicio,"La fecha de inicio"),
                RuleHelper.FutureDate<Reserva>(u => u.FechaFin,"La fecha de fin")
            };

            return ValidatorHelper.Validate(reserva, rules, "La reserva");
        }
    }
}
