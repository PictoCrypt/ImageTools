using System;
using System.IO;
using System.Windows;
using ImageToolApp.Models;
using Microsoft.Win32;

namespace ImageToolApp.Controllers
{
    public class MainController
    {
        private readonly Window mView;
        private readonly MainViewModel mViewModel;

        public MainController(Window mainWindow)
        {
            mView = mainWindow;

            var encryptController = new EncryptController();
            var decryptController = new DecryptController();
            mViewModel = new MainViewModel(encryptController.GetView(), decryptController.GetView());
            SetupCommands();
            mView.DataContext = mViewModel;
            mView.Show();
        }

        private void SetupCommands()
        {
            mViewModel.CloseAppCommand = UICommand.Regular(CloseApp);
            mViewModel.LoadImageCommand = UICommand.Regular(LoadImage);
            mViewModel.PreferencesCommand = UICommand.Regular(OpenPreferencesWindow);
        }

        private void OpenPreferencesWindow()
        {
            /*  Open new Window where you can choose which Encryption-Algorithm you want to use.
            *   Define a standard path
            *   Define a standard Password
            */
            throw new NotImplementedException();
        }

        private void LoadImage()
        {
            var dialog = new OpenFileDialog();
            dialog.ShowDialog();
            dialog.Multiselect = false;
            if (string.IsNullOrEmpty(dialog.FileName))
            {
                return;
            }
            mViewModel.GlobalViewModel.ImagePath = dialog.FileName;
        }

        private void CloseApp()
        {
            mView.Close();
        }
    }
}