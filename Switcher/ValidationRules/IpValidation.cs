using System.Globalization;
using System.Linq;
using System.Windows.Controls;

namespace Switcher.ValidationRules
{
    public class IpValidation : ValidationRule
    {
        private const int MaxIpStringLength = 15; // "255.255.255.255".Length
        
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return value switch
            {
                string ipString when ipString.Length > MaxIpStringLength => new ValidationResult(false,
                    $"IP can't be longer than {MaxIpStringLength} characters"),
                string ipString when string.IsNullOrWhiteSpace(ipString) => new ValidationResult(false,
                    "IP is required"),
                string ipString => IsValidIPv4(ipString)
                    ? new ValidationResult(true, null)
                    : new ValidationResult(false, "Not valid IP Address (IP mask is 255.255.255.255)"),
                _ => new ValidationResult(false, "Password should be a number")
            };
        }

        private bool IsValidIPv4(string ipString)
        {
            string[] splitValues = ipString.Split('.');
            return splitValues.Length == 4 && splitValues.All(r => byte.TryParse(r, out byte _));
        }
    }
}