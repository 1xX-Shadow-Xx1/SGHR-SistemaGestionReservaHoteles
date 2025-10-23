using SGHR.Domain.Base;
using SGHR.Domain.Entities.Configuration.Habitaciones;
using SGHR.Domain.Validators.Configuration;


namespace SGHR.Domain.Validators.Habitaciones
{
    public static class HabitacionValidator
    {
        public static OperationResult<Habitacion> Validate(Habitacion mantenimiento)
        {

            var rules = new List<Func<Habitacion, (bool, string)>>
            {
                RuleHelper.RequeredNumInt<Habitacion>(u => u.IdPiso,"El ID del piso"),
                RuleHelper.RequeredNumInt<Habitacion>(u => u.Capacidad, "La capacidad de la habitacion"),
                RuleHelper.RequeredNumInt<Habitacion>(u => u.IdAmenity, "El ID del Amenity"),
                RuleHelper.Required<Habitacion>(u => u.Numero, "El numero de la habitacion"),
                RuleHelper.Required<Habitacion>(u => u.Estado, "El estado de la habitacion"),
                RuleHelper.GreaterThanZero<Habitacion>(u => u.Capacidad,"La capacidad de la habitacion"),
                RuleHelper.Range<Habitacion>(u => u.IdPiso,0,5,"El numero del piso")

            };

            return ValidatorHelper.Validate(mantenimiento, rules, "la habitacion");
        }
    }
}
