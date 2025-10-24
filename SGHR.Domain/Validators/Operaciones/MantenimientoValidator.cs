using SGHR.Domain.Base;
using SGHR.Domain.Entities.Configuration.Operaciones;
using SGHR.Domain.Enum.Operaciones;
using SGHR.Domain.Validators.Configuration;
using SGHR.Domain.Validators.ConfigurationRules;

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
                RuleHelper.RequeredNumInt<Mantenimiento>(u => u.RealizadoPor,"El ID del Usuario"),
                RuleHelper.GreaterThanZero<Mantenimiento>(u => u.RealizadoPor,"El ID del Usuario"),
                RuleHelper.FutureDate<Mantenimiento>(u => u.FechaInicio,"La Fecha de Inicio"),
                EnumRuleHelper.ValidEnum<Mantenimiento, EstadoMantenimiento>(u => u.Estado,"El estado del mantenimiento")

            };

            return ValidatorHelper.Validate(mantenimiento, rules, "El mantenimiento");
        }
    }
}
