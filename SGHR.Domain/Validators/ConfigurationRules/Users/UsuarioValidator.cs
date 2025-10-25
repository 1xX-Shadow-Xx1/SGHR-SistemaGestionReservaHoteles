using SGHR.Domain.Entities.Configuration.Usuers;
using SGHR.Domain.Enum.Usuarios;

namespace SGHR.Domain.Validators.ConfigurationRules.Users
{
    public class UsuarioValidator
    {
        public bool Validate(Usuario usuario, out string errorMessage)
        {
            if (!ValidationHelper.NotNull(usuario, "Usuario", out errorMessage)) return false;

            if (!ValidationHelper.Required(usuario.Nombre, "Nombre", out errorMessage)) return false;
            if (!ValidationHelper.MaxLength(usuario.Nombre, 100, "Nombre", out errorMessage)) return false;
            if (!ValidationHelper.MinLength(usuario.Nombre, 5, "Nombre", out errorMessage)) return false;

            if (!ValidationHelper.Required(usuario.Correo, "Correo", out errorMessage)) return false;
            if (!ValidationHelper.MaxLength(usuario.Correo, 100, "Correo", out errorMessage)) return false;
            if (!ValidationHelper.RegexMatch(usuario.Correo, @"^[^@\s]+@[^@\s]+\.[^@\s]+$", "Correo", "Correo inválido.", out errorMessage)) return false;

            if (!ValidationHelper.Required(usuario.Contraseña, "Contraseña", out errorMessage)) return false;
            if (!ValidationHelper.MinLength(usuario.Contraseña, 8, "Contraseña", out errorMessage)) return false;

            var rolesValidos = new[] { RolUsuarios.Cliente, RolUsuarios.Recepcionista, RolUsuarios.Administrador };
            if (!ValidationHelper.InList(usuario.Rol, rolesValidos, "Rol", out errorMessage)) return false;

            errorMessage = string.Empty;
            return true;
        }
    }
}
