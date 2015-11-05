using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using ImageToolApp.Views;

namespace ImageToolApp.Converters
{
    public class FrameworkElementDecryptToVisibilityConverter : OneWayConveter, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var currentElement = value as BaseTabView;
            if (currentElement != null && currentElement.ViewName == "Decrypt")
            {
                return Visibility.Visible;
            }
            return Visibility.Collapsed;
        }
    }
}