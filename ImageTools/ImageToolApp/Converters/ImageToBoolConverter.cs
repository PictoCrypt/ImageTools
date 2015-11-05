using System;
using System.Globalization;
using System.Windows.Data;

namespace ImageToolApp.Converters
{
    public class ImageToBoolConverter : OneWayConveter, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null;
        }
    }
}