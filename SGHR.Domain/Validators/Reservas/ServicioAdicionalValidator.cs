using SGHR.Domain.Base;
using SGHR.Domain.Entities.Configuration.Habitaciones;
using SGHR.Domain.Entities.Configuration.Reservas;
using SGHR.Domain.Validators.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGHR.Domain.Validators.Reservas
{
    public static class ServicioAdicionalValidator
    {
        public static OperationResult<ServicioAdicional> Validate(ServicioAdicional servicioAdicional)
        {

            var rules = new List<Func<ServicioAdicional, (bool, string)>>
            {
                RuleHelper.Required<ServicioAdicional>(u => u.Nombre,"El nombre"),
                RuleHelper.MaxLength<ServicioAdicional>(u => u.Descripcion,200,"La descripcion"),
                RuleHelper.RequeredNumDecimal<ServicioAdicional>(u => u.Precio,"El precio"),
                RuleHelper.PositiveDecimal<ServicioAdicional>(u => u.Precio,"El precio")
            };

            return ValidatorHelper.Validate(servicioAdicional, rules, "El servicio adicional");
        }
    }
}
