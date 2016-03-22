using System.Drawing;
using FunctionLib.Helper;
using FunctionLib.Steganography.LSB;

namespace ImageToolApp.ViewModels
{
    public class EncryptTabViewModel : BaseTabViewModel
    {
        private string mResultImagePath = string.Empty;
        private string mText;

        public string Text
        {
            get { return mText; }
            set
            {
                if (value.Equals(mText))
                {
                    return;
                }
                mText = value;
                OnPropertyChanged("Text");
                OnPropertyChanged("ProgressBarValue");
            }
        }

        public double ProgressBarValue
        {
            get
            {
                if (string.IsNullOrEmpty(ImagePath) || SelectedSteganographicMethod == null ||
                    string.IsNullOrEmpty(Text))
                {
                    return 0;
                }
                double max;
                using (var bitmap = new Bitmap(ImagePath))
                {
                    max = SelectedSteganographicMethod.MaxEmbeddingCount(bitmap, LsbIndicator);
                }
                var contentLength = (double) SelectedEncryptionMethod.Encode(Text, Password ?? "Test").Length;
                contentLength += ConvertHelper.Convert(contentLength.ToString()).Length;
                contentLength += Constants.TagSeperator.Length;
                var result = contentLength/max*100;
                return result;
            }
            set
            {
                //GNDN
            }
        }

        public string ResultImagePath
        {
            get { return mResultImagePath; }
            set
            {
                if (value.Equals(mResultImagePath))
                {
                    return;
                }
                mResultImagePath = value;
                OnPropertyChanged("ResultImagePath");
            }
        }
    }
}