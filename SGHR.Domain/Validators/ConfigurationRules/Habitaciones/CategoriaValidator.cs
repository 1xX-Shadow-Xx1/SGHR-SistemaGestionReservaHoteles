using SGHR.Domain.Entities.Configuration.Habitaciones;

namespace SGHR.Domain.Validators.ConfigurationRules.Habitaciones
{
    public class CategoriaValidator
    {
        public bool Validate(Categoria categoria, out string errorMessage)
        {
            if (!ValidationHelper.NotNull(categoria, "Categoría", out errorMessage)) return false;

            // Nombre
            if (!ValidationHelper.Required(categoria.Nombre, "Nombre", out errorMessage)) return false;
            if (!ValidationHelper.MaxLength(categoria.Nombre, 100, "Nombre", out errorMessage)) return false;


            // Descripción
            if (!ValidationHelper.Required(categoria.Descripcion, "Nombre", out errorMessage)) return false;
            if (!ValidationHelper.MaxLength(categoria.Descripcion, 1000, "Descripción", out errorMessage)) return false;

            // Precio
            if (!ValidationHelper.Required(categoria.Precio, "Precio", out errorMessage)) return false;
            if (!ValidationHelper.GreaterThanZero(categoria.Precio, "Precio", out errorMessage)) return false;

            errorMessage = string.Empty;
            return true;
        }
    }
}
