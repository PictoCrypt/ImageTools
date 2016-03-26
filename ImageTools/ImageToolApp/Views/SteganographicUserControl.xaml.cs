using System.Windows;
using System.Windows.Controls;

namespace ImageToolApp.Views
{
    /// <summary>
    /// Interaction logic for SteganographicUserControl.xaml
    /// </summary>
    public partial class SteganographicUserControl : UserControl
    {
        public SteganographicUserControl()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(SteganographicUserControl));

        public string Text
        {
            get { return (string) GetValue(TextProperty); }
            set { SetValue(TextProperty, value);}
        }


        public static readonly DependencyProperty EnabledProperty = DependencyProperty.Register("Enabled", typeof(bool), typeof(SteganographicUserControl));

        public bool Enabled
        {
            get { return (bool)GetValue(EnabledProperty); }
            set { SetValue(EnabledProperty, value); }
        }
    }
}
