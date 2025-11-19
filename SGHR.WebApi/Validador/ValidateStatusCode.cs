using System.ComponentModel.DataAnnotations;

namespace SGHR.Web.Validador
{
    public class ValidateStatusCode
    {
        public ValidateStatusCode()
        {

        }

        private enum HttpStatus
        {
            Ok = 200,
            Created = 201,
            [Display(Name = "Bad Request")]
            BadRequest = 400,
            [Display(Name = "Un authorized")]
            Unauthorized = 401,
            [Display(Name = "For bidden")]
            Forbidden = 403,
            [Display(Name = "Not Found")]
            NotFound = 404,
            [Display(Name = "Internal Server Error")]
            InternalServerError = 500,
            [Display(Name = "Not Implemented")]
            NotImplemented = 501,
            [Display(Name = "Service Unavailable")]
            ServiceUnavailable = 503,
            [Display(Name = "Gateway Timeout")]
            GatewayTimeout = 504
        }

        public bool ValidatorStatus(int status, out string errorMessage)
        {
            if (Enum.IsDefined(typeof(HttpStatus), status))
            {
                var enumValue = (HttpStatus)status;

                if (enumValue == HttpStatus.Ok || enumValue == HttpStatus.Created)
                {
                    errorMessage = string.Empty;
                    return true;
                }

                if (enumValue == HttpStatus.BadRequest)
                {
                    errorMessage = string.Empty;
                    return false;
                }

                errorMessage = GetDisplayName(enumValue);
                return false;
            }

            errorMessage = $"Código de error desconocido: {status}";
            return false;
        }

        
        public static string GetDisplayName(Enum enumValue)
        {
            var attr = enumValue.GetType()
                .GetField(enumValue.ToString())
                .GetCustomAttributes(typeof(DisplayAttribute), false)
                .FirstOrDefault() as DisplayAttribute;

            return attr?.Name ?? enumValue.ToString();
        }

    }
}
