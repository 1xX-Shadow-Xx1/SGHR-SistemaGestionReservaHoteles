namespace SGHR.Domain.Validators.Configuration
{
    public static class RuleHelper
    {

        // Validación de Campo vacio
        public static Func<T, (bool, string)> Required<T>(
         Func<T, string> selector,
         string fieldName)
        {
            return entity =>
            (
                !string.IsNullOrWhiteSpace(selector(entity)),
                $"{fieldName} es obligatorio"
            );
        }
        // Validación de Campo vacio (Numeros Int)
        public static Func<T, (bool, string)> RequeredNumInt<T>(
            Func<T, int?> selector,
            string fieldName)
        {
            return entity =>
            (
                selector(entity) != null,
                $"{fieldName} es obligatorio"
            );
        }
        // Validación de Campo vacio (Numeros Decimales)
        public static Func<T, (bool, string)> RequeredNumDecimal<T>(
            Func<T, decimal?> selector,
            string fieldName)
        {
            return entity =>
            (
                selector(entity) != null,
                $"{fieldName} es obligatorio"
            );
        }
        // Validación de Longitud Maxima
        public static Func<T, (bool, string)> MaxLength<T>(
            Func<T, string> selector,
            int max,
            string fieldName)
        {
            return entity =>
            (
                selector(entity)?.Length <= max,
                $"{fieldName} no puede tener más de {max} caracteres"
            );
        }
        // Validación de Longitud Minima
        public static Func<T, (bool, string)> MinLength<T>(
            Func<T, string> selector,
            int min,
            string fieldName)
        {
            return entity =>
            (
                selector(entity)?.Length >= min,
                $"{fieldName} debe tener al menos {min} caracteres"
            );
        }

        // Validación de email
        public static Func<T, (bool, string)> Email<T>(
            Func<T, string> selector,
            string fieldName)
        {
            return entity =>
            {
                var value = selector(entity);
                bool isValid = !string.IsNullOrWhiteSpace(value) &&
                               System.Text.RegularExpressions.Regex.IsMatch(
                                   value,
                                   @"^[^@\s]+@[^@\s]+\.[^@\s]+$");

                return (isValid, $"{fieldName} que ingreso no tiene un formato válido");
            };
        }

        // Validación para Numeros positivos
        public static Func<T, (bool, string)> GreaterThanZero<T>(
            Func<T, int> selector,
            string fieldName)
        {
            return entity =>
            (
                selector(entity) >= 0,
                $"{fieldName} debe ser mayor o igual que 0"
            );
        }

        //Validación para Numeros positivo (para decimales)
        public static Func<T, (bool, string)> PositiveDecimal<T>(
            Func<T, decimal> selector,
            string fieldName)
        {
            return entity =>
            (
                selector(entity) >= 0,
                $"{fieldName} debe ser un valor positivo"
            );
        }

        //Validación para Rango de valores
        public static Func<T, (bool, string)> Range<T>(
            Func<T, int> selector,
            int min,
            int max,
            string fieldName)
        {
            return entity =>
            {
                var value = selector(entity);
                return (value >= min && value <= max,
                    $"{fieldName} debe estar entre {min} y {max}");
            };
        }

        //Validación para Fechas Valida
        public static Func<T, (bool, string)> Date<T>(
            Func<T, DateTime> selector,
            string fieldName)
        {
            return entity =>
            (
                selector(entity) != default(DateTime),
                $"{fieldName} debe ser valida"
            );
        }

        //Validación para Fechas futura
        public static Func<T, (bool, string)> FutureDate<T>(
            Func<T, DateTime> selector,
            string fieldName)
        {
            return entity =>
            (
                selector(entity) > DateTime.Now,
                $"{fieldName} debe ser una fecha futura"
            );
        }

        //Validación para Fecha pasada
        public static Func<T, (bool, string)> PastDate<T>(
            Func<T, DateTime> selector,
            string fieldName)
        {
            return entity =>
            (
                selector(entity) < DateTime.Now,
                $"{fieldName} debe ser una fecha pasada"
            );
        }
    }
}
