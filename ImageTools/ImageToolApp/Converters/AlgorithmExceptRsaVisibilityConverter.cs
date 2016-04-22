using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using FunctionLib.Cryptography;
using UserControlClassLibrary.Converters;

namespace ImageToolApp.Converters
{
    public class AlgorithmExceptRsaVisibilityConverter : OneWayConveter, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var selectedItem = value as RsaAlgorithm;
            return selectedItem == null ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}