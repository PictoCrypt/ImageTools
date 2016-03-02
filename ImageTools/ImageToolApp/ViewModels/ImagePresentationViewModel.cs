using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Media;
using AForge.Imaging;
using UserControlClassLibrary;
using Point = System.Windows.Point;

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

        public PointCollection LuminanceHistogramPoints
        {
            get
            {
                var bmp = new Bitmap(Image);
                var hslStatistics = new ImageStatisticsHSL(bmp);
                var values = hslStatistics.Luminance.Values;
                return GetPointCollection(values);
            }
        }

        public PointCollection RedHistogramPoints
        {
            get
            {
                var bmp = new Bitmap(Image);
                var rgbStatistics = new ImageStatistics(bmp);
                var values = rgbStatistics.Red.Values;
                return GetPointCollection(values);
            }
        }

        public PointCollection GreenHistogramPoints
        {
            get
            {
                var bmp = new Bitmap(Image);
                var rgbStatistics = new ImageStatistics(bmp);
                var values = rgbStatistics.Green.Values;
                return GetPointCollection(values);
            }
        }

        public PointCollection BlueHistogramPoints
        {
            get
            {
                var bmp = new Bitmap(Image);
                var rgbStatistics = new ImageStatistics(bmp);
                var values = rgbStatistics.Blue.Values;
                return GetPointCollection(values);
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

        private PointCollection GetPointCollection(IReadOnlyList<int> values)
        {
            var max = values.Max();

            // first point (lower-left corner)
            var points = new PointCollection {new Point(0, max)};
            // middle points
            for (var i = 0; i < values.Count; i++)
            {
                points.Add(new Point(i, max - values[i]));
            }
            // last point (lower-right corner)
            points.Add(new Point(values.Count - 1, max));
            return points;
        }
    }
}