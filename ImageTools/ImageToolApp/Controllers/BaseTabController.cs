using System.IO;
using System.Windows.Controls;
using ImageToolApp.ViewModels;
using Microsoft.Win32;

namespace ImageToolApp.Controllers
{
    public abstract class BaseTabController<TView, TViewModel> : IBaseTabController
        where TView : UserControl, new() where TViewModel : BaseTabViewModel, new()
    {
        // TODO: Tmp-Files löschen nach gebrauch?


        protected readonly TViewModel ViewModel;

        protected BaseTabController()
        {
            View = CreateView();
            ViewModel = new TViewModel();
            View.DataContext = ViewModel;
            InitializeController();
        }

        private void InitializeController()
        {
            RegisterCommands();
        }

        public TView View { get; }

        public void OpenImage()
        {
            var dialog = new OpenFileDialog
            {
                Multiselect = false,
                InitialDirectory = ViewModel.PreferencesModel.StandardPath
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

        private TView CreateView()
        {
            return new TView();
        }

        protected abstract void RegisterCommands();

        public void PreferencesSaved()
        {
            ViewModel.Password = ViewModel.PreferencesModel.Password;
            ViewModel.SelectedEncryptionMethod = ViewModel.PreferencesModel.SelectedEncryptionMethod;
            ViewModel.SelectedSteganographicMethod = ViewModel.PreferencesModel.SelectedSteganographicMethod;
        }
    }
}