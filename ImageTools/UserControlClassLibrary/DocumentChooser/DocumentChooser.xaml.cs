using System.ComponentModel;
using System.Windows;
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

        public static readonly DependencyProperty PathProperty = DependencyProperty.Register("Path", typeof(string),
    typeof(DocumentChooser), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public string Path
        {
            get { return (string)GetValue(PathProperty); }
            set { SetValue(PathProperty, value); }
        }
    }
}