using SGHR.Domain.Base;
using SGHR.Domain.Entities.Configuration.Operaciones;
using SGHR.Domain.Enum.Operaciones;
using SGHR.Domain.Validators.Configuration;
using SGHR.Domain.Validators.ConfigurationRules;

namespace SGHR.Domain.Validators.Operaciones
{
    public static class PagoValidator
    {
        public static OperationResult<Pago> Validate(Pago pago)
        {

            var rules = new List<Func<Pago, (bool, string)>>
            {
                RuleHelper.RequeredNumInt<Pago>(u => u.IdReserva,"El ID de la reserva"),
                RuleHelper.RequeredNumDecimal<Pago>(u => u.Monto,"El monto"),
                RuleHelper.Date<Pago>(u => u.FechaPago,"La fecha de pago"),
                RuleHelper.Required<Pago>(u => u.MetodoPago,"El metodo de pago"),
                RuleHelper.MaxLength<Pago>(u => u.MetodoPago,50,"El metodo de pago"),
                EnumRuleHelper.ValidEnum<Pago, EstadoPago>(u => u.Estado,"El estado del pago")
            };

            return ValidatorHelper.Validate(pago, rules, "El Pago");
        }
    }
}
