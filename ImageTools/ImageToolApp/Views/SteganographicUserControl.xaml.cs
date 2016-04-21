using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ImageToolApp.Views
{
    /// <summary>
    ///     Interaction logic for SteganographicUserControl.xaml
    /// </summary>
    public partial class SteganographicUserControl : UserControl
    {
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string),
            typeof(SteganographicUserControl));


        public static readonly DependencyProperty EnabledProperty = DependencyProperty.Register("Enabled", typeof(bool),
            typeof(SteganographicUserControl));

        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command",
            typeof(ICommand), typeof(SteganographicUserControl));

        public SteganographicUserControl()
        {
            InitializeComponent();
        }

        public string Text
        {
            get { return (string) GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public bool Enabled
        {
            get { return (bool) GetValue(EnabledProperty); }
            set { SetValue(EnabledProperty, value); }
        }

        public ICommand Command
        {
            get { return (ICommand) GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }
    }
}