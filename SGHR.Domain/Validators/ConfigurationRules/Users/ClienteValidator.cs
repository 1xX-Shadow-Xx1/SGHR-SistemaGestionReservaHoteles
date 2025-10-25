using SGHR.Domain.Entities.Configuration.Usuers;

namespace SGHR.Domain.Validators.ConfigurationRules.Users
{
    public class ClienteValidator
    {
        public bool Validate(Cliente cliente, out string errorMessage)
        {
            if (!ValidationHelper.NotNull(cliente, "Cliente", out errorMessage)) return false;
            if (!ValidationHelper.GreaterThanZero(cliente.IdUsuario, "IdUsuario", out errorMessage)) return false;
            if (!ValidationHelper.Required(cliente.Nombre, "Nombre", out errorMessage)) return false;
            if (!ValidationHelper.MaxLength(cliente.Nombre, 100, "Nombre", out errorMessage)) return false;
            if (!ValidationHelper.Required(cliente.Apellido, "Apellido", out errorMessage)) return false;
            if (!ValidationHelper.MaxLength(cliente.Apellido, 100, "Apellido", out errorMessage)) return false;
            if (!string.IsNullOrEmpty(cliente.Telefono))
            {
                if (!ValidationHelper.MaxLength(cliente.Telefono, 50, "Telefono", out errorMessage)) return false;
                if (!ValidationHelper.RegexMatch(cliente.Telefono, @"^[0-9\s\-]+$", "Telefono", "Telefono inválido.", out errorMessage)) return false;
            }
            if (!ValidationHelper.Required(cliente.Cedula, "Cédula", out errorMessage)) return false;
            if (!ValidationHelper.MaxLength(cliente.Cedula, 15, "Cédula", out errorMessage)) return false;
            if (!ValidationHelper.RegexMatch(cliente.Cedula, @"^\d{3}-\d{7}-\d{1}$", "Cédula", "Cédula inválida.", out errorMessage)) return false;
            if (!string.IsNullOrEmpty(cliente.Direccion))
            {
                if (!ValidationHelper.MaxLength(cliente.Direccion, 255, "Dirección", out errorMessage)) return false;
            }

            errorMessage = string.Empty;
            return true;
        }
    }
}
