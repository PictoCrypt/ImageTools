using System.Windows.Controls;

namespace UserControlClassLibrary.PathChooser
{
    /// <summary>
    ///     Interaction logic for PathChooser.xaml
    /// </summary>
    public partial class PathChooser : UserControl
    {
        public PathChooser()
        {
            InitializeComponent();
            DataContext = new PathChooserViewModel();
        }
    }
}