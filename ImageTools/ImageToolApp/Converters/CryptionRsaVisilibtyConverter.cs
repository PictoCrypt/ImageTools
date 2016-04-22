using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using FunctionLib.Cryptography;
using UserControlClassLibrary.Converters;

namespace ImageToolApp.Converters
{
    public class CryptionRsaVisilibtyConverter : OneWayConveter, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var obj = value as RsaAlgorithm;
            if (obj == null)
            {
                return Visibility.Collapsed;
            }
            return Visibility.Visible;
        }
    }
}