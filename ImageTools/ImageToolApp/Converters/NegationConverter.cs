using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using UserControlClassLibrary.Converters;

namespace ImageToolApp.Converters
{
    public class NegationConverter : OneWayConveter, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility)
            {
                var val = (Visibility) value;
                switch (val)
                {
                    case Visibility.Visible:
                        return Visibility.Collapsed;
                    case Visibility.Hidden:
                    case Visibility.Collapsed:
                        return Visibility.Visible;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            return null;
        }
    }
}