using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using FunctionLib.Helper;
using ImageToolApp.ViewModels;
using ImageToolApp.Views;
using UserControlClassLibrary;

namespace ImageToolApp.Controllers
{
    public class MainController
    {
        private readonly DecryptController mDecryptController;
        private readonly EncryptController mEncryptController;
        private readonly MainWindow mView;
        private readonly MainViewModel mViewModel;

        public MainController(MainWindow mainWindow)
        {
            mView = mainWindow;
            mView.Closing += ViewOnClosing;
            mEncryptController = new EncryptController(this);
            mDecryptController = new DecryptController(this);
            mViewModel = new MainViewModel(mEncryptController.View, mDecryptController.View);
            SetupCommands();
            mView.DataContext = mViewModel;
            mView.Show();
        }

        private IBaseTabController CurrentController
        {
            get
            {
                var view = mViewModel.CurrentElement as BaseTabView;
                if (view != null && view.ViewName == "Encrypt")
                {
                    return mEncryptController;
                }
                return mDecryptController;
            }
        }

        private void ViewOnClosing(object sender, CancelEventArgs cancelEventArgs)
        {
            UnregisterEvents();
        }

        private void UnregisterEvents()
        {
            mEncryptController.UnregisterEvents();

            mView.Closing -= ViewOnClosing;
        }

        private void SetupCommands()
        {
            mViewModel.CloseAppCommand = UICommand.Regular(CloseApp);
            mViewModel.SettingsCommand = UICommand.Regular(OpenSettingsWindow);
            mViewModel.OpenImageCommand = UICommand.Regular(OpenImage);
            mViewModel.SaveImageCommand = UICommand.Regular(SaveImage);
            mViewModel.OpenTxtCommand = UICommand.Regular(OpenTxt);
            mViewModel.SaveTxtCommand = UICommand.Regular(SaveTxt);
            mViewModel.ChangedPixelsCommand = UICommand.Regular(ChangedPixels);
            mViewModel.OpenHelpCommand = UICommand.Regular(OpenHelp);
        }

        private static void OpenHelp()
        {
            var executionPath = MethodHelper.ExecutiongPath;
            Process.Start(Path.Combine(executionPath, "Help.pdf"));
        }

        private void ChangedPixels()
        {
            var controller = CurrentController as EncryptController;
            if (controller != null)
            {
                controller.ChangedPixels();
            }
        }

        private void SaveTxt()
        {
            var controller = CurrentController as DecryptController;
            if (controller != null)
            {
                controller.SaveTxt();
            }
        }

        private void OpenTxt()
        {
            var controller = CurrentController as EncryptController;
            if (controller != null)
            {
                controller.OpenTxt();
            }
        }

        private void SaveImage()
        {
            var controller = CurrentController as EncryptController;
            if (controller != null)
            {
                controller.SaveImage();
            }
        }

        private void OpenSettingsWindow()
        {
            // TODO
            /*  Open new Window where you can choose which Encryption-Algorithm you want to use.
            *   Define a standard path
            *   Define a standard Password
            */
            var settingsController = new SettingsController(mView);
            if (settingsController.OpenDialog())
            {
                mDecryptController.SettingsSaved();
                mEncryptController.SettingsSaved();
            }
        }

        private void OpenImage()
        {
            CurrentController.OpenImage();
        }

        private void CloseApp()
        {
            mView.Close();
        }
    }
}