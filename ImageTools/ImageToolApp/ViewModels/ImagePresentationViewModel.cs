using System.Drawing;
using AForge.Imaging;
using UserControlClassLibrary;

namespace ImageToolApp.ViewModels
{
    public class ImagePresentationViewModel : BaseViewModel
    {
        private string mImage;
        private UICommand mSaveCommand;
        private string mText;

        public ImagePresentationViewModel(string image, string text = "")
        {
            Image = image;
            Text = text;
        }

        public string Image
        {
            get { return mImage; }
            set
            {
                if (value.Equals(mImage))
                {
                    return;
                }
                mImage = value;
                OnPropertyChanged("Image");
            }
        }

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
            }
        }

        public int[] LuminanceHistogramPoints
        {
            get
            {
                var bmp = new Bitmap(Image);
                // Luminance
                var hslStatistics = new ImageStatisticsHSL(bmp);
                var luminanceValues = hslStatistics.Luminance.Values;
                //// RGB
                //ImageStatistics rgbStatistics = new ImageStatistics(bmp);
                //int[] redValues = rgbStatistics.Red.Values;
                //int[] greenValues = rgbStatistics.Green.Values;
                //int[] blueValues = rgbStatistics.Blue.Values;
                return luminanceValues;
            }
        }

        public UICommand SaveCommand
        {
            get { return mSaveCommand; }
            set
            {
                if (value.Equals(mSaveCommand))
                {
                    return;
                }
                mSaveCommand = value;
                OnPropertyChanged("SaveCommand");
            }
        }
    }
}