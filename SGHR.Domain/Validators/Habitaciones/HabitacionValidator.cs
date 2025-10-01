using SGHR.Domain.Base;
using SGHR.Domain.Entities.Configuration.Habitaciones;
using SGHR.Domain.Entities.Configuration.Operaciones;
using SGHR.Domain.Validators.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGHR.Domain.Validators.Habitaciones
{
    public static class HabitacionValidator
    {
        public static OperationResult<Habitacion> Validate(Habitacion mantenimiento)
        {

            var rules = new List<Func<Habitacion, (bool, string)>>
            {
                RuleHelper.RequeredNumInt<Habitacion>(u => u.IdPiso,"El numero del piso"),
                RuleHelper.RequeredNumInt<Habitacion>(u => u.Capacidad, "La capacidad de la habitacion"),
                RuleHelper.Required<Habitacion>(u => u.Numero, "El numero de la habitacion"),
                RuleHelper.Required<Habitacion>(u => u.Estado, "El estado de la habitacion"),
                RuleHelper.GreaterThanZero<Habitacion>(u => u.Capacidad,"La capacidad de la habitacion"),
                RuleHelper.Range<Habitacion>(u => u.IdPiso,0,5,"El numero del piso")

            };

            return ValidatorHelper.Validate(mantenimiento, rules, "la habitacion");
        }
    }
}
