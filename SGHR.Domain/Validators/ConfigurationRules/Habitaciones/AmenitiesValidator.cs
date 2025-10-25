﻿using SGHR.Domain.Entities.Configuration.Habitaciones;

namespace SGHR.Domain.Validators.ConfigurationRules.Habitaciones
{
    public class AmenitiesValidator
    {
        public bool Validate(Amenity amenity, out string errorMessage)
        {
            if (!ValidationHelper.NotNull(amenity, "Amenity", out errorMessage)) return false;

            // Nombre
            if (!ValidationHelper.Required(amenity.Nombre, "Nombre", out errorMessage)) return false;
            if (!ValidationHelper.MaxLength(amenity.Nombre, 100, "Nombre", out errorMessage)) return false;

            // Descripción (opcional)
            if (!string.IsNullOrEmpty(amenity.Descripcion))
            {
                if (!ValidationHelper.MaxLength(amenity.Descripcion, 1000, "Descripción", out errorMessage)) return false;
                // Ajusta el límite según tu necesidad
            }

            errorMessage = string.Empty;
            return true;
        }
    }
}
