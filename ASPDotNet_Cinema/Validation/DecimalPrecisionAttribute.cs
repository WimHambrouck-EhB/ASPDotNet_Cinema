using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace ASPDotNet_Cinema.Validation
{
    // bron: https://stackoverflow.com/a/62394344
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public class DecimalPrecisionAttribute(uint decimalPrecision) : ValidationAttribute
    {

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (!(value is null || value is decimal d && HasPrecision(d, decimalPrecision)))
            {
                return new ValidationResult(ErrorMessage ?? $"The field {validationContext.DisplayName} must be a decimal number with at most {decimalPrecision} digits after the decimal point.");
            }

            return ValidationResult.Success;
        }

        private static bool HasPrecision(decimal value, uint precision)
        {
            string valueStr = value.ToString(CultureInfo.InvariantCulture);
            int indexOfDot = valueStr.IndexOf('.');
            if (indexOfDot == -1)
            {
                return true;
            }

            return valueStr.Length - indexOfDot - 1 <= precision;
        }
    }
}