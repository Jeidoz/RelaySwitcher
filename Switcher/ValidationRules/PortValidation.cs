using System.Globalization;
using System.Windows.Controls;

namespace Switcher.ValidationRules
{
    public class PortValidation : ValidationRule
    {
        private const int MinPortValue = 1;
        private const int MaxPortValue = ushort.MaxValue;
        private const int MaxPortStringLength = 5;
        
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            switch (value)
            {
                case int portInt:
                    return IsValidPort(portInt) 
                        ? new ValidationResult(true, null) 
                        : new ValidationResult(false, $"Port should be in range {MinPortValue}-{MaxPortValue}");
                case string portString when portString.Length > MaxPortStringLength:
                    return new ValidationResult(false, $"Port can't be longer than {MaxPortStringLength} characters");
                case string portString:
                {
                    if (string.IsNullOrEmpty(portString))
                    {
                        return new ValidationResult(false, "Port is required");
                    }
                    
                    bool isParsed = int.TryParse(portString, out int port);
                    if (isParsed && IsValidPort(port))
                    {
                        return new ValidationResult(true, null);
                    }

                    return new ValidationResult(false, $"Port should be in range {MinPortValue}-{MaxPortValue}");
                }
                default:
                    return new ValidationResult(false, "Port should be a number");
            }
        }
        
        private bool IsValidPort(int port)
        {
            return port >= MinPortValue && port <= MaxPortValue;
        }
    }
}