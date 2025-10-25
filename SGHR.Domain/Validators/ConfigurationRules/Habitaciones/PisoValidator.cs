using SGHR.Domain.Entities.Configuration.Habitaciones;
using SGHR.Domain.Enum.Habitaciones;


namespace SGHR.Domain.Validators.ConfigurationRules.Habitaciones
{
    public class PisoValidator
    {
        public bool Validate(Piso piso, out string errorMessage)
        {
            if (!ValidationHelper.NotNull(piso, "Piso", out errorMessage)) return false;

            // Número de piso
            if (!ValidationHelper.GreaterThanZero(piso.NumeroPiso, "Número de Piso", out errorMessage)) return false;

            // Descripción (opcional)
            if (!string.IsNullOrEmpty(piso.Descripcion))
            {
                if (!ValidationHelper.MaxLength(piso.Descripcion, 1000, "Descripción", out errorMessage)) return false;
            }

            // Estado
            var estadosValidos = new[] { EstadoPiso.Habilitado, EstadoPiso.EnMantenimiento, EstadoPiso.Deshabilitado }; // 0=inactivo, 1=activo
            if (!ValidationHelper.InList(piso.Estado, estadosValidos, "Estado", out errorMessage)) return false;

            errorMessage = string.Empty;
            return true;
        }
    }
}
