using SGHR.Domain.Base;
using SGHR.Domain.Entities.Configuration.Habitaciones;
using SGHR.Domain.Entities.Configuration.Operaciones;
using SGHR.Domain.Validators.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGHR.Domain.Validators.Habitaciones
{
    public static class CategoriaValidator
    {
        public static OperationResult<Categoria> Validate(Categoria categoria)
        {

            var rules = new List<Func<Categoria, (bool, string)>>
            {
                RuleHelper.Required<Categoria>(u => u.Nombre,"El nombre"),
                RuleHelper.Required<Categoria>(u => u.Descripcion,"La descripcion"),
                RuleHelper.MaxLength<Categoria>(u => u.Descripcion,500,"La descripcion"),
                RuleHelper.MaxLength<Categoria>(u => u.Nombre,100,"El nombre"),
                RuleHelper.MinLength<Categoria>(u => u.Descripcion,50,"La descripcion"),
            };

            return ValidatorHelper.Validate(categoria, rules, "La categoria");
        }
    }
}
