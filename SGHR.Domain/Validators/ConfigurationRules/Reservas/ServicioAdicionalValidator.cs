using SGHR.Domain.Entities.Configuration.Reservas;
using SGHR.Domain.Enum.Reservas;

namespace SGHR.Domain.Validators.ConfigurationRules.Reservas
{
    public class ServicioAdicionalValidator
    {
        public bool Validate(ServicioAdicional servicio, out string errorMessage)
        {
            if (!ValidationHelper.NotNull(servicio, "Servicio Adicional", out errorMessage)) return false;

            // Nombre
            if (!ValidationHelper.Required(servicio.Nombre, "Nombre", out errorMessage)) return false;
            if (!ValidationHelper.MaxLength(servicio.Nombre, 100, "Nombre", out errorMessage)) return false;

            // Descripción (opcional)
            if (!string.IsNullOrEmpty(servicio.Descripcion))
            {
                if (!ValidationHelper.MaxLength(servicio.Descripcion, 1000, "Descripción", out errorMessage)) return false;
                // Ajusta 1000 si quieres permitir más texto
            }

            if (!ValidationHelper.GreaterThanZero(servicio.Precio, "Precio", out errorMessage)) return false;

            var estadosValidos = new [] { EstadoServicioAdicional.Activo, EstadoServicioAdicional .Inactivo}; 
            if (!ValidationHelper.InList(servicio.Estado, estadosValidos, "Estado", out errorMessage)) return false;

            errorMessage = string.Empty;
            return true;
        }
    }
}
