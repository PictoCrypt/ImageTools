using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
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

                var binding = new Binding {Path = new PropertyPath("Result"), Source = this};
                if (string.IsNullOrEmpty(str) || !File.Exists(str))
                {
                    result = new TextBox
                    {
                        AcceptsReturn = true,
                        IsReadOnly = true,
                        Background = Brushes.DarkGray,
                        //Text = Result,
                        TextWrapping = TextWrapping.Wrap
                    };
                    TextBoxHelper.SetWatermark(result, "Resulting content...");
                    BindingOperations.SetBinding(result, TextBox.TextProperty, binding);
                }
                else
                {
                    if (Constants.ImageExtensions.Contains(Path.GetExtension(str).Replace(".", ""),
                        StringComparer.OrdinalIgnoreCase))
                    {
                        result = new Image();
                        BindingOperations.SetBinding(result, Image.SourceProperty, binding);
                    }
                    else
                    {
                        result = new TextBox
                        {
                            AcceptsReturn = false,
                            IsReadOnly = true,
                            Background = Brushes.DarkGray,
                            //Text = Result
                        };
                        BindingOperations.SetBinding(result, TextBox.TextProperty, binding);
                    }
                }

                return result;
            }
        }
    }
}