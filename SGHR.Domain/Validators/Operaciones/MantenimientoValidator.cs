using SGHR.Domain.Base;
using SGHR.Domain.Entities.Configuration.Operaciones;
using SGHR.Domain.Validators.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGHR.Domain.Validators.Operaciones
{
    public static class MantenimientoValidator
    {
        public static OperationResult<Mantenimiento> Validate(Mantenimiento mantenimiento)
        {

            var rules = new List<Func<Mantenimiento, (bool, string)>>
            {
                RuleHelper.RequeredNumInt<Mantenimiento>(u => u.IdPiso,"El ID del piso"),
                RuleHelper.RequeredNumInt<Mantenimiento>(u => u.IdHabitacion, "El ID de la Habitacion"),
                RuleHelper.Required<Mantenimiento>(u => u.Descripcion, "La Descripcion"),
                RuleHelper.MaxLength<Mantenimiento>(u => u.Descripcion,200, "La Descripcion"),
                RuleHelper.MinLength<Mantenimiento>(u => u.Descripcion,50, "La Descripcion"),
                RuleHelper.Required<Mantenimiento>(u => u.Estado,"El estado"),
                RuleHelper.RequeredNumInt<Mantenimiento>(u => u.RealizadoPor,"El ID del Usuario"),
                RuleHelper.GreaterThanZero<Mantenimiento>(u => u.RealizadoPor,"El ID del Usuario"),
                RuleHelper.FutureDate<Mantenimiento>(u => u.FechaInicio,"La Fecha de Inicio")

            };

            return ValidatorHelper.Validate(mantenimiento, rules, "El mantenimiento");
        }
    }
}
