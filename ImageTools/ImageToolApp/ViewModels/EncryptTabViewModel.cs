using System.Drawing;
using FunctionLib.Helper;

namespace ImageToolApp.ViewModels
{
    public class EncryptTabViewModel : BaseTabViewModel
    {
        private string mResult = string.Empty;
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
                OnPropertyChanged("CanTabActionExecuted");
            }
        }

        public override bool CanTabActionExecuted
        {
            get { return base.CanTabActionExecuted && !string.IsNullOrEmpty(Text) && ProgressBarValue <= 100; }
        }

        public double ProgressBarValue
        {
            get
            {
                if (string.IsNullOrEmpty(ImagePath) || SteganographicModel.Algorithm == null ||
                    string.IsNullOrEmpty(Text))
                {
                    return 0;
                }
                double max;
                using (var bitmap = new Bitmap(ImagePath))
                {
                    max = SteganographicModel.Algorithm.MaxEmbeddingCount(bitmap, SteganographicModel.LsbIndicator);
                }
                var contentLength =
                    (double) CryptionModel.Algorithm.Encode(Text, CryptionModel.Password ?? "Test").Length;
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
    }
}