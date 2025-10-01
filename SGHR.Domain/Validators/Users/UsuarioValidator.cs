using SGHR.Domain.Base;
using SGHR.Domain.Entities.Configuration.Usuers;
using SGHR.Domain.Validators.Configuration;


namespace SGHR.Domain.Validators.Users
{
    public static class UsuarioValidator
    {
        public static OperationResult<Usuario> Validate(Usuario usuario)
        {
            string nombre = "El nombre";
            string correo = "El correo";
            string contraseña= "La contraseña";

            var rules = new List<Func<Usuario, (bool, string)>>
            {
                RuleHelper.Required<Usuario>(u => u.Nombre, nombre),
                RuleHelper.Required<Usuario>(u => u.Correo, correo),
                RuleHelper.Required<Usuario>(u => u.Contrasena, contraseña),
                RuleHelper.MinLength<Usuario>(u => u.Contrasena, 6, contraseña),
                RuleHelper.MaxLength<Usuario>(u => u.Nombre, 20, nombre),
                RuleHelper.MaxLength<Usuario>(u => u.Contrasena, 50, contraseña),
                RuleHelper.MaxLength<Usuario>(u => u.Correo, 40, correo),
                RuleHelper.Email<Usuario>(u => u.Correo, correo)
            };                  
            return ValidatorHelper.Validate(usuario, rules, "El usuario");
        }
    }
}
