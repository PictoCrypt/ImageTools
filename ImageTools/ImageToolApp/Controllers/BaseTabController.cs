using System.Linq;
using System.Windows;
using System.Windows.Controls;
using FunctionLib.Helper;
using ImageToolApp.ViewModels;
using MahApps.Metro.Controls;
using Microsoft.Win32;
using UserControlClassLibrary;

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

            if (mImageExpander != null)
            {
                mImageExpander = View.FindChildren<Expander>().FirstOrDefault(x => x.Name == "ImageExpander");
                mImageExpander.Expanded += ImageExpanderEvent;
                mImageExpander.Collapsed += ImageExpanderEvent;
            }
        }

        public TView View { get; }

        public void OpenImage()
        {
            var dialog = new OpenFileDialog
            {
                Multiselect = false,
                InitialDirectory = ViewModel.Settings.DefaultPath,
                Filter = ConvertHelper.GenerateFilter(ViewModel.SteganographicModel.Algorithm.PossibleImageFormats)
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
            ViewModel.SteganographicModel.TabActionCommand = ActionCommand;
        }

        private TView CreateView()
        {
            return new TView();
        }

        protected abstract UICommand ActionCommand { get; }

        public void SettingsSaved()
        {
            ViewModel.CryptionModel.Password = ViewModel.Settings.Password;
            ViewModel.CryptionModel.Algorithm = ViewModel.Settings.SelectedEncryptionMethod;
            ViewModel.SteganographicModel.Algorithm = ViewModel.Settings.SelectedSteganographicMethod;
        }
    }
}