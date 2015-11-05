using System;
using System.Globalization;
using System.Windows.Data;

namespace ImageToolApp.Converters
{
    public class EqualsLsbConverter : OneWayConveter, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.ToString().Equals("LSB"))
            {
                return true;
            }
            return false;
        }
    }
}