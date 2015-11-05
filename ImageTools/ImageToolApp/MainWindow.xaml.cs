using System.Windows;
using MahApps.Metro.Controls;

namespace ImageToolApp
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var stack = StackPanel;
            stack.Visibility = stack.Visibility == Visibility.Visible ? Visibility.Hidden : Visibility.Visible;
        }
    }
}