using System.Windows;
using ImageToolApp.Models;
using ImageToolApp.Views;

namespace ImageToolApp.Controllers
{
    public class MainController
    {
        private readonly Window mView;
        private readonly MainViewModel mViewModel;
        private readonly EncryptController mEncryptController;
        private readonly DecryptController mDecryptController;

        public MainController(Window mainWindow)
        {
            mView = mainWindow;

            mEncryptController = new EncryptController();
            mDecryptController = new DecryptController();
            mViewModel = new MainViewModel(mEncryptController.View, mDecryptController.View);
            SetupCommands();
            mView.DataContext = mViewModel;
            mView.Show();
        }

        public IBaseTabController CurrentController
        {
            get
            {
                var view = mViewModel.CurrentElement as EncryptView;
                if (view != null)
                {
                    return mEncryptController;
                }
                return mDecryptController;
            }
        }

        private void SetupCommands()
        {
            mViewModel.CloseAppCommand = UICommand.Regular(CloseApp);
            mViewModel.PreferencesCommand = UICommand.Regular(OpenPreferencesWindow);
            mViewModel.OpenImageCommand = UICommand.Regular(OpenImage);
            mViewModel.SaveImageCommand = UICommand.Regular(SaveImage);
            mViewModel.OpenTxtCommand = UICommand.Regular(OpenTxt);
            mViewModel.SaveTxtCommand = UICommand.Regular(SaveTxt);
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

        private void OpenPreferencesWindow()
        {
            /*  Open new Window where you can choose which Encryption-Algorithm you want to use.
            *   Define a standard path
            *   Define a standard Password
            */
            var preferncesController = new PreferencesController();
            if (preferncesController.OpenDialog())
            {
                mEncryptController.InitializeCryptings();
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