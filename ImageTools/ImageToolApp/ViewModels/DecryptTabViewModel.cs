using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using FunctionLib;
using FunctionLib.Enums;
using MahApps.Metro.Controls;

namespace ImageToolApp.ViewModels
{
    public class DecryptTabViewModel : BaseTabViewModel
    {
        public DecryptTabViewModel()
        {
            Result = "";
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
                FrameworkElement result = null;
                switch (SelectedResultingType)
                {
                    case ResultingType.Text:
                        result = new TextBox
                        {
                            AcceptsReturn = true,
                            IsReadOnly = true,
                            Background = Brushes.DarkGray,
                            Text = Result.ToString(),
                            TextWrapping = TextWrapping.Wrap,
                        };
                        TextBoxHelper.SetWatermark(result, "Resulting content...");
                        break;
                    case ResultingType.Image:
                        result = new Image
                        {
                            Source = new BitmapImage(new Uri(Result.ToString()))
                        };
                        break;
                    case ResultingType.Document:
                        result = new TextBox
                        {
                            AcceptsReturn = false,
                            IsReadOnly = true,
                            Background = Brushes.DarkGray,
                            Text = Result.ToString()
                        };
                        break;
                }
                return result;
            }
        }
    }
}