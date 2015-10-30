using System;
using System.Windows;
using System.Windows.Controls;

namespace ImageToolApp.Views
{
    /// <summary>
    /// Interaction logic for BaseTabView.xaml
    /// </summary>
    public partial class BaseTabView : UserControl
    {
        public static readonly DependencyProperty ReadonlyProperty = DependencyProperty.Register(
            "TextBoxReadOnly",
            typeof(bool),
            typeof(BaseTabView),
            new PropertyMetadata(false));

        public static readonly DependencyProperty ViewNameProperty = DependencyProperty.Register(
            "ViewName",
            typeof(string),
            typeof(BaseTabView),
            new PropertyMetadata(string.Empty));

        public bool TextBoxReadOnly
        {
            get { return (bool)GetValue(ReadonlyProperty); }
            set { SetValue(ReadonlyProperty, value); }
        }

        public string ViewName
        {
            get { return (string)GetValue(ViewNameProperty); }
            set { SetValue(ViewNameProperty, value); }
        }

        public BaseTabView(string viewName, bool textBoxReadOnly)
        {
            ViewName = viewName;
            TextBoxReadOnly = textBoxReadOnly;
            InitializeComponent();
        }
    }
}
