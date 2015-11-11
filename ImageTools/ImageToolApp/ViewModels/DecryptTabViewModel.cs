using System;
using System.IO;
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
        public DecryptTabViewModel()
        {
            Result = string.Empty;
        }

        private object mResult;

        public object Result
        {
            get { return mResult; }
            set
            {
                if (value.Equals(mResult))
                {
                    return;
                }
                mResult = value;
                OnPropertyChanged("ResultContent");
            }
        }

        public FrameworkElement ResultContent
        {
            get
            {
                FrameworkElement result;
                var str = Result.ToString();
                if (string.IsNullOrEmpty(str) || !File.Exists(str))
                {
                    result = new TextBox
                    {
                        AcceptsReturn = true,
                        IsReadOnly = true,
                        Background = Brushes.DarkGray,
                        Text = Result.ToString(),
                        TextWrapping = TextWrapping.Wrap,
                    };
                    TextBoxHelper.SetWatermark(result, "Resulting content...");
                }
                else
                {
                    if (Constants.ImageExtensions.Contains(Path.GetExtension(str)))
                    {
                        result = new Image
                        {
                            Source = new BitmapImage(new Uri(Result.ToString()))
                        };
                    }
                    else
                    {
                        result = new TextBox
                        {
                            AcceptsReturn = false,
                            IsReadOnly = true,
                            Background = Brushes.DarkGray,
                            Text = Result.ToString()
                        };
                    }
                }
                return result;
            }
        }
    }
}