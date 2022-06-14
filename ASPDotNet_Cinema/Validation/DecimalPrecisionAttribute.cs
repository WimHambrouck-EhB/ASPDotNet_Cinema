using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace ASPDotNet_Cinema.Validation
{
    // bron: https://stackoverflow.com/a/62394344
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public class DecimalPrecisionAttribute : ValidationAttribute
    {
        private readonly uint _decimalPrecision;

        public DecimalPrecisionAttribute(uint decimalPrecision)
        {
            _decimalPrecision = decimalPrecision;
        }

        public override bool IsValid(object value)
        {
            return value is null || value is decimal d && HasPrecision(d, _decimalPrecision);
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