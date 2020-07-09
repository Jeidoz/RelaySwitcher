using System.Globalization;
using System.Windows.Controls;

namespace Switcher.ValidationRules
{
    public sealed class PasswordValidation : ValidationRule
    {
        private const int MinPasswordValue = 0;
        private const int MaxPasswordValue = 9999;
        private const int MaxPasswordStringLength = 4;

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            switch (value)
            {
                case int passwordInt:
                    return IsValidPassword(passwordInt) 
                        ? new ValidationResult(true, null) 
                        : new ValidationResult(false, $"Password should be in range {MinPasswordValue}-{MaxPasswordValue}");
                case string passwordString when passwordString.Length > MaxPasswordStringLength:
                    return new ValidationResult(false, $"Password can't be longer than {MaxPasswordStringLength} characters");
                case string passwordString:
                {
                    if (string.IsNullOrWhiteSpace(passwordString))
                    {
                        return new ValidationResult(false, "Password is required (use 0 for none)");
                    }
                    
                    bool isParsed = int.TryParse(passwordString, out int password);
                    if (isParsed && IsValidPassword(password))
                    {
                        return new ValidationResult(true, null);
                    }

                    return new ValidationResult(false, $"Password should be in range {MinPasswordValue}-{MaxPasswordValue}");
                }
                default:
                    return new ValidationResult(false, "Password should be a number");
            }
        }
        
        private static bool IsValidPassword(int password)
        {
            return password >= MinPasswordValue && password <= MaxPasswordValue;
        }

        public static bool IsValidPassword(string passwordString)
        {
            if (passwordString.Length > MaxPasswordStringLength)
            {
                return false;
            }
            
            return int.TryParse(passwordString, out int password) && IsValidPassword(password);
        }
    }
}