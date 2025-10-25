using SGHR.Domain.Base;
using SGHR.Domain.Entities.Configuration.Usuers;
using SGHR.Domain.Enum.Usuario;
using SGHR.Domain.Validators.Configuration;
using SGHR.Domain.Validators.ConfigurationRules;


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
                RuleHelper.Required<Usuario>(u => u.Contraseña, contraseña),
                RuleHelper.MinLength<Usuario>(u => u.Contraseña, 6, contraseña),
                RuleHelper.MaxLength<Usuario>(u => u.Nombre, 20, nombre),
                RuleHelper.MaxLength<Usuario>(u => u.Contraseña, 50, contraseña),
                RuleHelper.MaxLength<Usuario>(u => u.Correo, 40, correo),
                RuleHelper.Email<Usuario>(u => u.Correo, correo),
                EnumRuleHelper.ValidEnum<Usuario, EstadoUsuario>(u => u.Estado,"El estado del usuario")
            };                  
            return ValidatorHelper.Validate(usuario, rules, "El usuario");
        }
    }
}
