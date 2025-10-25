using SGHR.Domain.Base;


namespace SGHR.Domain.Validators
{
    public abstract class ValidatorHelper
    {
        public static OperationResult<T> Validate<T>(T entity, 
            List<Func<T, (bool IsValid, string ErrorMessage)>> rules,string fieldName = "La entidad")
        {
            if (entity == null)
                return OperationResult<T>.Fail($"{fieldName} no puede ser nulo");

            var result = new OperationResult<T>
            {
                Success = true,
                Data = entity
            };

            foreach (var rule in rules)
            {
                var (isValid, errorMessage) = rule(entity);
                if (!isValid)
                {
                    result.Success = false;
                    result.Message = errorMessage;
                    return result;
                }
            }

            return result;
        }
    }
}
