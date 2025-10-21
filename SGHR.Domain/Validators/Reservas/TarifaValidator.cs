using SGHR.Domain.Base;
using SGHR.Domain.Entities.Configuration.Reservas;
using SGHR.Domain.Validators.Configuration;


namespace SGHR.Domain.Validators.Reservas
{
    public static class TarifaValidator
    {
        public static OperationResult<Tarifa> Validate(Tarifa tarifa)
        {
            var rules = new List<Func<Tarifa, (bool, string)>>
            {
                RuleHelper.PositiveDecimal<Tarifa>(u => u.Precio,"El Precio"),
                RuleHelper.RequeredNumInt<Tarifa>(u => u.IdCategoria,"El ID de la Categoria"),
                RuleHelper.MaxLength<Tarifa>(u => u.Temporada, 30, "La temporada")
            };

            return ValidatorHelper.Validate(tarifa, rules);
        }
    }
}
