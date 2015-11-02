using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using ImageToolApp.Views;

namespace ImageToolApp.Converters
{
    public class FrameworkElementEncryptToVisibilityConverter : OneWayConveter, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var currentElement = value as BaseTabView;
            if (currentElement != null && currentElement.ViewName == "Encrypt")
            {
                return Visibility.Visible;
            }
            return Visibility.Collapsed;
        }
    }
}