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

            // Descripción (opcional)
            if (!string.IsNullOrEmpty(categoria.Descripcion))
            {
                if (!ValidationHelper.MaxLength(categoria.Descripcion, 1000, "Descripción", out errorMessage)) return false;
                // Ajusta el límite si quieres permitir más texto
            }

            errorMessage = string.Empty;
            return true;
        }
    }
}
