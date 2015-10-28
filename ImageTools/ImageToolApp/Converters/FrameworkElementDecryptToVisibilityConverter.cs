using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using ImageToolApp.Views;

namespace ImageToolApp.Converters
{
    public class FrameworkElementDecryptToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var currentElement = value as DecryptView;
            if (currentElement == null)
            {
                return Visibility.Collapsed;
            }
            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}