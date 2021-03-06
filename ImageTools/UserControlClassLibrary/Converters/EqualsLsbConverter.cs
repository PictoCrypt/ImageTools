﻿using System;
using System.Globalization;
using System.Windows.Data;
using FunctionLib.Steganography.LSB;

namespace UserControlClassLibrary.Converters
{
    public class EqualsLsbConverter : OneWayConveter, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is LsbAlgorithmBase;
        }
    }
}