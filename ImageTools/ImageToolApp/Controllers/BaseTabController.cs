using System.Linq;
using System.Windows;
using System.Windows.Controls;
using FunctionLib.Helper;
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
                InitialDirectory = ViewModel.Settings.DefaultPath,
                Filter = ConvertHelper.GenerateFilter(ViewModel.SelectedSteganographicMethod.PossibleImageFormats)
            };

            dialog.ShowDialog();
            if (string.IsNullOrEmpty(dialog.FileName))
            {
                return;
            }

            ViewModel.ImagePath = dialog.FileName;
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
            ViewModel.EncryptionModel.Password = ViewModel.Settings.Password;
            ViewModel.EncryptionModel.Algorithm = ViewModel.Settings.SelectedEncryptionMethod;
            ViewModel.SelectedSteganographicMethod = ViewModel.Settings.SelectedSteganographicMethod;
        }
    }
}