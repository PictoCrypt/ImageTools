using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ImageToolApp.ViewModels;
using MahApps.Metro.Controls;
using Microsoft.Win32;

namespace ImageToolApp.Controllers
{
    public abstract class BaseTabController<TViewModel, TView> : IBaseTabController
        where TViewModel : BaseTabViewModel, new()
        where TView : UserControl, new()
    {
        private readonly Expander mImageExpander;
        // TODO: Tmp-Files löschen nach gebrauch?

        protected readonly TViewModel ViewModel;

        protected BaseTabController()
        {
            View = CreateView();
            ViewModel = new TViewModel();
            View.DataContext = ViewModel;
            InitializeController();

            mImageExpander = View.FindChildren<Expander>().FirstOrDefault(x => x.Name == "ImageExpander");
            mImageExpander.Expanded += ImageExpanderEvent;
            mImageExpander.Collapsed += ImageExpanderEvent;
        }

        public TView View { get; }

        public void OpenImage()
        {
            var dialog = new OpenFileDialog
            {
                Multiselect = false,
                InitialDirectory = ViewModel.Settings.StandardPath
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
            var grid = View.FindChild<Grid>("Grid");
            var column = grid.ColumnDefinitions[Grid.GetColumn(expander)];
            column.Width = new GridLength(1.0, expanded ? GridUnitType.Star : GridUnitType.Auto);
        }

        public virtual void UnregisterEvents()
        {
            mImageExpander.Expanded -= ImageExpanderEvent;
        }

        private void InitializeController()
        {
            RegisterCommands();
        }

        private TView CreateView()
        {
            return new TView();
        }

        protected abstract void RegisterCommands();

        public void SettingsSaved()
        {
            ViewModel.Password = ViewModel.Settings.Password;
            ViewModel.SelectedEncryptionMethod = ViewModel.Settings.SelectedEncryptionMethod;
            ViewModel.SelectedSteganographicMethod = ViewModel.Settings.SelectedSteganographicMethod;
        }
    }
}