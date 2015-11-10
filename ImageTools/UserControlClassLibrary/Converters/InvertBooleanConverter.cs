using System;
using System.Globalization;
using System.Windows.Data;

namespace UserControlClassLibrary.Converters
{
    public class InvertBooleanConverter : OneWayConveter, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool) value;
        }
    }
}