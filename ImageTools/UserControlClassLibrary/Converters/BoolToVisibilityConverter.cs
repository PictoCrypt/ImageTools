using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace UserControlClassLibrary.Converters
{
    public class BoolToVisibilityConverter : OneWayConveter, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var obj = (bool) value;
            return obj ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}