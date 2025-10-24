namespace SGHR.Domain.Validators.ConfigurationRules
{
    public static class EnumRuleHelper
    {
        public static Func<T, (bool, string)> ValidEnum<T, TEnum>(Func<T, TEnum> selector, string fieldName)
            where TEnum : System.Enum
        {
            return entity =>
            {
                var value = selector(entity);
                bool isValid = System.Enum.IsDefined(typeof(TEnum), value);
                return (isValid, $"{fieldName} tiene un valor inválido.");
            };
        }
    }
}
