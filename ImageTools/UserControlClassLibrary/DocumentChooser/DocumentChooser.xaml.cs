using System.Windows.Controls;

namespace UserControlClassLibrary.DocumentChooser
{
    /// <summary>
    ///     Interaction logic for DocumentChooser.xaml
    /// </summary>
    public partial class DocumentChooser : UserControl
    {
        public DocumentChooser()
        {
            InitializeComponent();
            DataContext = new DocumentChooserViewModel();
        }
    }
}