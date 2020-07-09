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
            switch (value)
            {
                case string ipString when ipString.Length > MaxIpStringLength:
                    return new ValidationResult(false, $"IP can't be longer than {MaxIpStringLength} characters");
                case string ipString when string.IsNullOrWhiteSpace(ipString):
                    return new ValidationResult(false, "IP is required");
                case string ipString:
                {
                    return IsValidIPv4(ipString) 
                        ? new ValidationResult(true, null) 
                        : new ValidationResult(false, "Not valid IP Address (IP mask is 255.255.255.255)");
                }
                default:
                    return new ValidationResult(false, "Password should be a number");
            }
        }

        private bool IsValidIPv4(string ipString)
        {
            var splitValues = ipString.Split('.');
            return splitValues.Length == 4 && splitValues.All(r => byte.TryParse(r, out byte _));
        }
    }
}