using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ImageToolApp.ViewModels;
using ImageToolApp.Views;
using MahApps.Metro.Controls;
using Microsoft.Win32;

namespace ImageToolApp.Controllers
{
    public abstract class BaseTabController<TViewModel> : IBaseTabController where TViewModel : BaseTabViewModel, new()
    {
        // TODO: Tmp-Files löschen nach gebrauch?


        protected readonly TViewModel ViewModel;
        protected MainController MainController;

        protected BaseTabController(MainController mainController, string viewName, bool textBoxReadOnly)
        {
            MainController = mainController;
            View = CreateView(viewName, textBoxReadOnly);
            ViewModel = new TViewModel();
            View.DataContext = ViewModel;
            InitializeController();
            View.ImageExpander.Expanded += ImageExpanderEvent;
            View.ImageExpander.Collapsed += ImageExpanderEvent;
        }

        public BaseTabView View { get; }

        protected ProgressRing ProgressRing
        {
            get
            {
                var result = Application.Current.MainWindow.FindChildren<ProgressRing>().FirstOrDefault();
                return result;
            }
        }


        public void OpenImage()
        {
            var dialog = new OpenFileDialog
            {
                Multiselect = false,
                InitialDirectory = ViewModel.SettingsModel.StandardPath
            };

            dialog.ShowDialog();
            if (string.IsNullOrEmpty(dialog.FileName))
            {
                return;
            }

            var tmp = Path.ChangeExtension(Path.GetTempFileName(), Path.GetExtension(dialog.FileName));
            File.Copy(dialog.FileName, tmp);
            ViewModel.ImagePath = tmp;
        }

        private void ImageExpanderEvent(object sender, RoutedEventArgs routedEventArgs)
        {
            var expander = sender as Expander;
            ResizeImageExpanderGrid(expander, expander.IsExpanded);
        }

        private void ResizeImageExpanderGrid(UIElement expander, bool expanded)
        {
            var column = View.Grid.ColumnDefinitions[Grid.GetColumn(expander)];
            column.Width = new GridLength(1.0, expanded ? GridUnitType.Star : GridUnitType.Auto);
        }

        public virtual void UnregisterEvents()
        {
            View.ImageExpander.Expanded -= ImageExpanderEvent;
        }

        private void InitializeController()
        {
            RegisterCommands();
        }

        private BaseTabView CreateView(string buttonName, bool textBlockReadOnly)
        {
            return new BaseTabView(buttonName, textBlockReadOnly);
        }

        protected abstract void RegisterCommands();

        public void SettingsSaved()
        {
            ViewModel.Password = ViewModel.SettingsModel.Password;
            ViewModel.SelectedEncryptionMethod = ViewModel.SettingsModel.SelectedEncryptionMethod;
            ViewModel.SelectedSteganographicMethod = ViewModel.SettingsModel.SelectedSteganographicMethod;
        }
    }
}