using System.Text.RegularExpressions;

namespace SGHR.Domain.Validators
{
    public static class ValidationHelper 
    {
        public static bool NotNull<T>(T entity, string entityName, out string errorMessage)
        {
            if (entity == null)
            {
                errorMessage = $"{entityName} no puede ser nulo.";
                return false;
            }
            errorMessage = string.Empty;
            return true;
        }

        public static bool Required(decimal? value, string fieldName, out string errorMessage)
        {
            if (value == null)
            {
                errorMessage = $"{fieldName} es obligatorio.";
                return false;
            }
            errorMessage = string.Empty;
            return true;
        }

        public static bool Required(DateTime? value, string fieldName, out string errorMessage)
        {
            if (value == null || value == DateTime.MinValue)
            {
                errorMessage = $"{fieldName} es obligatorio.";
                return false;
            }
            errorMessage = string.Empty;
            return true;
        }

        public static bool Required(string? value, string fieldName, out string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                errorMessage = $"{fieldName} es obligatorio.";
                return false;
            }
            errorMessage = string.Empty;
            return true;
        }

        public static bool GreaterThanZero(int value, string fieldName, out string errorMessage)
        {
            if (value <= 0)
            {
                errorMessage = $"{fieldName} debe ser mayor a 0.";
                return false;
            }
            errorMessage = string.Empty;
            return true;
        }

        public static bool GreaterThanZero(decimal value, string fieldName, out string errorMessage)
        {
            if (value <= 0)
            {
                errorMessage = $"{fieldName} debe ser mayor a 0.";
                return false;
            }
            errorMessage = string.Empty;
            return true;
        }

        public static bool MaxLength(string? value, int maxLength, string fieldName, out string errorMessage)
        {
            if (!string.IsNullOrEmpty(value) && value.Length > maxLength)
            {
                errorMessage = $"{fieldName} excede la longitud máxima de caracteres.";
                return false;
            }
            errorMessage = string.Empty;
            return true;
        }

        public static bool MinLength(string? value, int minLength, string fieldName, out string errorMessage)
        {
            if (!string.IsNullOrEmpty(value) && value.Length < minLength)
            {
                errorMessage = $"{fieldName} debe tener al menos {minLength} caracteres.";
                return false;
            }
            errorMessage = string.Empty;
            return true;
        }

        public static bool RegexMatch(string value, string pattern, string fieldName, string errorMsg, out string errorMessage)
        {
            if (!Regex.IsMatch(value ?? "", pattern))
            {
                errorMessage = errorMsg;
                return false;
            }
            errorMessage = string.Empty;
            return true;
        }

        public static bool InList<T>(T value, IEnumerable<T> validValues, string fieldName, out string errorMessage)
        {
            if (!validValues.Contains(value))
            {
                errorMessage = $"{fieldName} contiene un valor inválido.";
                return false;
            }
            errorMessage = string.Empty;
            return true;
        }
    }
}