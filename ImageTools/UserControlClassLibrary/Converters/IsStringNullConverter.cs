using System;
using System.Globalization;
using System.Windows.Data;

namespace UserControlClassLibrary.Converters
{
    public class IsStringNullConverter : OneWayConveter, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var obj = value as string;
            return !string.IsNullOrEmpty(obj);
        }
    }
}