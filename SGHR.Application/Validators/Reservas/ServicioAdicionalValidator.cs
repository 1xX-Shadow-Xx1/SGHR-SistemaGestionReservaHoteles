using SGHR.Domain.Base;
using SGHR.Domain.Entities.Configuration.Reservas;
using SGHR.Domain.Enum.Reservas;
using SGHR.Domain.Validators.Configuration;
using SGHR.Domain.Validators.ConfigurationRules;

namespace SGHR.Domain.Validators.Reservas
{
    public static class ServicioAdicionalValidator
    {
        public static OperationResult<ServicioAdicional> Validate(ServicioAdicional servicioAdicional)
        {

            var rules = new List<Func<ServicioAdicional, (bool, string)>>
            {
                RuleHelper.Required<ServicioAdicional>(u => u.Nombre,"El nombre"),
                RuleHelper.MaxLength<ServicioAdicional>(u => u.Descripcion,200,"La descripcion"),
                RuleHelper.RequeredNumDecimal<ServicioAdicional>(u => u.Precio,"El precio"),
                RuleHelper.PositiveDecimal<ServicioAdicional>(u => u.Precio,"El precio"),
                EnumRuleHelper.ValidEnum<ServicioAdicional, EstadoServicioAdicional>(u => u.Estado,"El estado del servicio adicional")
            };

            return ValidatorHelper.Validate(servicioAdicional, rules, "El servicio adicional");
        }
    }
}
