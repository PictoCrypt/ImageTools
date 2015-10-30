using System;
using System.IO;
using ImageToolApp.ViewModels;
using ImageToolApp.Views;
using Microsoft.Win32;

namespace ImageToolApp.Controllers
{
    public abstract class BaseTabController<TViewModel> : IBaseTabController where TViewModel : BaseTabViewModel, new()
    {
        // TODO: Tmp-Files löschen nach gebrauch?


        protected readonly TViewModel ViewModel;

        protected BaseTabController(string viewName, bool textBoxReadOnly)
        {
            View = CreateView(viewName, textBoxReadOnly);
            ViewModel = new TViewModel();
            View.DataContext = ViewModel;
            InitializeController();
        }

        private void InitializeController()
        {
            RegisterCommands();
        }

        public BaseTabView View { get; }

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