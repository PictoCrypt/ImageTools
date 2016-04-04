using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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

        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(SteganographicUserControl));

        public ICommand Command
        {
            get { return (ICommand) GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }
    }
}
