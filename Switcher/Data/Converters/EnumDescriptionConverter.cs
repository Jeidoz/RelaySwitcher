using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;

namespace Switcher.Data.Converters
{
    public sealed class EnumDescriptionConverter : IValueConverter
    {
        private string GetEnumDescription(Enum enumObj)
        {
            var fieldInfo = enumObj.GetType().GetField(enumObj.ToString());

            object[] attributes = fieldInfo.GetCustomAttributes(false);

            if (attributes.Length == 0)
            {
                return enumObj.ToString();
            }

            var attribute = attributes[0] as DescriptionAttribute;
            return attribute?.Description;
        }

        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var myEnum = (Enum)value;
            return GetEnumDescription(myEnum);
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.Empty;
        }
    }
}
