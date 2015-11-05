using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ImageToolApp.Converters
{
    public class StringToVisibilityConverter : OneWayConveter, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = value as string;
            if (result != null)
            {
                if (!string.IsNullOrEmpty(result))
                {
                    return Visibility.Visible;
                }
            }
            return Visibility.Collapsed;
        }
    }
}