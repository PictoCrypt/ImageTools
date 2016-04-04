using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using FunctionLib.Helper;
using ImageToolApp.Models;
using ImageToolApp.ViewModels;
using ImageToolApp.Views;
using UserControlClassLibrary;

namespace ImageToolApp.Controllers
{
    public class MainController
    {
        private readonly AnalysisTabController mAnalysisTabController;
        private readonly BenchmarkTabController mBenchmarkTabController;
        private readonly DecryptTabController mDecryptTabController;
        private readonly EncryptTabController mEncryptTabController;
        private readonly MainWindow mView;
        private readonly MainViewModel mViewModel;
        private Settings mSettings;

        public MainController(MainWindow mainWindow)
        {
            mView = mainWindow;
            mView.Closing += ViewOnClosing;
            LoadConfig();
            mEncryptTabController = new EncryptTabController();
            mDecryptTabController = new DecryptTabController();
            mAnalysisTabController = new AnalysisTabController();
            mBenchmarkTabController = new BenchmarkTabController();
            mViewModel = new MainViewModel(mEncryptTabController.View, mDecryptTabController.View,
                mAnalysisTabController.View, mBenchmarkTabController.View);
            SetupCommands();
            mView.DataContext = mViewModel;
            mView.Show();
        }

        private IBaseTabController CurrentTabController
        {
            get
            {
                var view = mViewModel.CurrentElement as EncryptTabView;
                if (view != null)
                {
                    return mEncryptTabController;
                }
                return mDecryptTabController;
            }
        }

        private void LoadConfig()
        {
            mSettings = Settings.Instance;
        }

        private void ViewOnClosing(object sender, CancelEventArgs cancelEventArgs)
        {
            UnregisterEvents();
        }

        private void UnregisterEvents()
        {
            mEncryptTabController.UnregisterEvents();

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
            mViewModel.CancelCommand = UICommand.Regular(CancelAction);
        }

        private void CancelAction()
        {
            HandleJobController.Cancel();
        }

        private static void OpenHelp()
        {
            var executionPath = Constants.ExecutiongPath;
            Process.Start(Path.Combine(executionPath, "Help.pdf"));
        }

        private void ChangedPixels()
        {
            var controller = CurrentTabController as EncryptTabController;
            if (controller != null)
            {
                controller.ChangedPixels();
            }
        }

        private void SaveTxt()
        {
            var controller = CurrentTabController as DecryptTabController;
            if (controller != null)
            {
                controller.SaveTxt();
            }
        }

        private void OpenTxt()
        {
            var controller = CurrentTabController as EncryptTabController;
            if (controller != null)
            {
                controller.OpenTxt();
            }
        }

        private void SaveImage()
        {
            var controller = CurrentTabController as EncryptTabController;
            if (controller != null)
            {
                controller.SaveImage();
            }
        }

        private void OpenSettingsWindow()
        {
            var settingsController = new SettingsController(mView, mSettings);
            if (settingsController.OpenDialog())
            {
                mDecryptTabController.SettingsSaved();
                mEncryptTabController.SettingsSaved();
            }
        }

        private void OpenImage()
        {
            CurrentTabController.OpenImage();
        }

        private void CloseApp()
        {
            mView.Close();
        }
    }
}