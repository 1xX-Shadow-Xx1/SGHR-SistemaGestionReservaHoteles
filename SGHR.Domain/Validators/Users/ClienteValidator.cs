using SGHR.Domain.Base;
using SGHR.Domain.Entities.Configuration.Usuers;
using SGHR.Domain.Validators.Configuration;

namespace SGHR.Domain.Validators.Users
{
    public static class ClienteValidator
    {
        public static OperationResult<Cliente> Validate(Cliente cliente)
        {
            string nombre = "El nombre";
            string apellido = "El apellido";
            string cedula = "La cedula";
            string telefono = "El telefono";

            var rules = new List<Func<Cliente, (bool, string)>>
            {
                RuleHelper.Required<Cliente>(u => u.Nombre, nombre),
                RuleHelper.Required<Cliente>(u => u.Apellido, apellido),
                RuleHelper.Required<Cliente>(u => u.Cedula, cedula),
                RuleHelper.Required<Cliente>(u => u.Telefono, telefono),

                RuleHelper.MaxLength<Cliente>(u => u.Nombre,50, nombre),
                RuleHelper.MaxLength<Cliente>(u => u.Apellido,50,apellido),
                RuleHelper.MaxLength<Cliente>(u => u.Cedula,15,cedula),
                RuleHelper.MaxLength<Cliente>(u => u.Telefono,15,telefono)
            };

            return ValidatorHelper.Validate(cliente, rules, "El cliente");
        }
    }
}
