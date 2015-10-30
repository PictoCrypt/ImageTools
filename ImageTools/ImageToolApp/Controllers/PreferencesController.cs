using System.Windows;
using System.Windows.Forms;
using ImageToolApp.Models;
using ImageToolApp.ViewModels;
using ImageToolApp.Views;

namespace ImageToolApp.Controllers
{
    public class PreferencesController
    {
        private readonly Preferences mView;
        private readonly PreferencesViewModel mViewModel;

        public PreferencesController(Window owner)
        {
            mView = new Preferences();
            mViewModel = new PreferencesViewModel();
            RegisterCommands();
            mView.Owner = owner;
            mView.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            mView.DataContext = mViewModel;
        }

        private void RegisterCommands()
        {
            mViewModel.SaveCommand = UICommand.Regular(Save);
            mViewModel.CancelCommand = UICommand.Regular(Cancel);
            mViewModel.ChoosePathCommand = UICommand.Regular(ChoosePath);
        }

        private void ChoosePath()
        {
            var dialog = new FolderBrowserDialog
            {
                ShowNewFolderButton = true,
                SelectedPath = PreferencesModel.Instance.StandardPath
            };
            var dialogResult = dialog.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                mViewModel.StandardPath = dialog.SelectedPath;
            }
        }

        private void Cancel()
        {
            mView.DialogResult = false;
            mView.Close();
        }

        private void Save()
        {
            mView.DialogResult = true;
            mView.Close();
        }

        public bool OpenDialog()
        {
            var result = mView.ShowDialog();
            if (result.HasValue && result.Value)
            {
                PreferencesModel.Instance.SaveToConfig(mViewModel.Password, mViewModel.SelectedEncryptionMethod.ToString(), 
                    mViewModel.SelectedSteganographicMethod.ToString(), mViewModel.StandardPath);
                return true;
            }
            return false;
        }
    }
}