using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
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

        //private void RibbonWindow_Loaded(object sender, RoutedEventArgs e)
        //{
        //    var ribbonApplicationMenu = RibbonAppMenu;
        //    if (ribbonApplicationMenu != null)
        //    {
        //        var border = ribbonApplicationMenu.Template.FindName("MainPaneBorder", ribbonApplicationMenu) as Border;
        //        if (border != null)
        //        {
        //            var grid = border.Parent as Grid;
        //            if (grid != null)
        //            {
        //                grid.ColumnDefinitions[2].Width = new GridLength(0);
        //            }
        //        }
        //    }
        //}
    }
}