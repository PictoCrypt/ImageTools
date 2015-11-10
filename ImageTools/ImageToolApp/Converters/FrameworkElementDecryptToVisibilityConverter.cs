using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using ImageToolApp.Views;
using UserControlClassLibrary.Converters;

namespace ImageToolApp.Converters
{
    public class FrameworkElementDecryptToVisibilityConverter : OneWayConveter, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var currentElement = value as DecryptTabView;
            if (currentElement != null)
            {
                return Visibility.Visible;
            }
            return Visibility.Collapsed;
        }
    }
}