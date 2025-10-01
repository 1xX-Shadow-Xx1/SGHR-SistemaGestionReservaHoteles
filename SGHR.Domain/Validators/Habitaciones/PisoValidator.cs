using SGHR.Domain.Base;
using SGHR.Domain.Entities.Configuration.Habitaciones;
using SGHR.Domain.Entities.Configuration.Usuers;
using SGHR.Domain.Validators.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGHR.Domain.Validators.Habitaciones
{
    public static class PisoValidator
    {
        public static OperationResult<Piso> Validate(Piso piso)
        {
            string numeropiso = "El numero del piso";

            var rules = new List<Func<Piso, (bool, string)>>
            {
                RuleHelper.Range<Piso>(u => u.NumeroPiso, 0,5, numeropiso),
                RuleHelper.MaxLength<Piso>(u => u.Descripcion,200,"La descripcion del piso")
            };

            return ValidatorHelper.Validate(piso, rules, "El piso");

        }
    }
}
