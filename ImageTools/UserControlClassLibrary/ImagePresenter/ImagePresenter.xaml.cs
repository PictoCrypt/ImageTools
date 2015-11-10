using System.Windows;
using System.Windows.Controls;

namespace UserControlClassLibrary.ImagePresenter
{
    /// <summary>
    /// Interaction logic for ImagePresenter.xaml
    /// </summary>
    public partial class ImagePresenter : UserControl
    {
        public ImagePresenter()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty ImageSourceProperty = DependencyProperty.Register(
            "ImageSource",
            typeof(string),
            typeof(ImagePresenter),
            new PropertyMetadata(false));

        public string ImageSource
        {
            get { return (string)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }
    }
}
