using System;
using System.Globalization;
using System.Windows.Data;

namespace ImageToolApp.Converters
{
    public class IsStringNullConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var obj = value as string;
            return !string.IsNullOrEmpty(obj);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}