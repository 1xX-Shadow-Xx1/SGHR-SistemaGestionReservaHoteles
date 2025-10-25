using SGHR.Domain.Entities.Configuration.Reservas;


namespace SGHR.Domain.Validators.ConfigurationRules.Reservas
{
    public class TarifaValidator
    {
        public bool Validate(Tarifa tarifa, out string errorMessage)
        {
            if (!ValidationHelper.NotNull(tarifa, "Tarifa", out errorMessage)) return false;

            // FK Categoria
            if (!ValidationHelper.GreaterThanZero(tarifa.IdCategoria, "IdCategoria", out errorMessage)) return false;

            // Temporada
            if (!ValidationHelper.Required(tarifa.Temporada, "Temporada", out errorMessage)) return false;
            if (!ValidationHelper.MaxLength(tarifa.Temporada, 50, "Temporada", out errorMessage)) return false;

            // Precio
            if (!ValidationHelper.GreaterThanZero(tarifa.Precio, "Precio", out errorMessage)) return false;

            errorMessage = string.Empty;
            return true;
        }
    }
}
