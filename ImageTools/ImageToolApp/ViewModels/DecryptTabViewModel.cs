using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using FunctionLib.Helper;
using MahApps.Metro.Controls;

namespace ImageToolApp.ViewModels
{
    public class DecryptTabViewModel : BaseTabViewModel
    {
        public FrameworkElement ResultContent
        {
            get
            {
                FrameworkElement result;
                var str = Result;
                if (string.IsNullOrEmpty(str) || !File.Exists(str))
                {
                    result = new TextBox
                    {
                        AcceptsReturn = true,
                        IsReadOnly = true,
                        Background = Brushes.DarkGray,
                        Text = Result,
                        TextWrapping = TextWrapping.Wrap
                    };
                    TextBoxHelper.SetWatermark(result, "Resulting content...");
                }
                else
                {
                    if (Constants.ImageExtensions.Contains(Path.GetExtension(str).Replace(".", ""),
                        StringComparer.OrdinalIgnoreCase))
                    {
                        result = new Image
                        {
                            Source = new BitmapImage(new Uri(Result))
                        };
                    }
                    else
                    {
                        result = new TextBox
                        {
                            AcceptsReturn = false,
                            IsReadOnly = true,
                            Background = Brushes.DarkGray,
                            Text = Result
                        };
                    }
                }
                return result;
            }
        }
    }
}