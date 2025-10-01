using SGHR.Domain.Base;
using SGHR.Domain.Entities.Configuration.Habitaciones;
using SGHR.Domain.Validators.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGHR.Domain.Validators.Habitaciones
{
    public static class AmenitiesValidator
    {
        public static OperationResult<Amenity> Validate(Amenity amenity)
        {

            var rules = new List<Func<Amenity, (bool, string)>>
            {
                RuleHelper.Required<Amenity>(u => u.Nombre,"El nombre"),
                RuleHelper.Required<Amenity>(u => u.Descripcion,"La descripcion"),
                RuleHelper.MaxLength<Amenity>(u => u.Descripcion,500,"La descripcion"),
                RuleHelper.MaxLength<Amenity>(u => u.Nombre,100,"El nombre"),
                RuleHelper.MinLength<Amenity>(u => u.Descripcion,50,"La descripcion"),
            };

            return ValidatorHelper.Validate(amenity, rules, "El amenity");
        }
    }
}
