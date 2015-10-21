using System.Configuration;
using System.Reflection;
using ImageToolApp.Models;
using ImageToolApp.Views;

namespace ImageToolApp.Controllers
{
    public class PreferencesController
    {
        private readonly Preferences mView;
        private readonly PreferencesViewModel mViewModel;

        public PreferencesController()
        {
            mView = new Preferences();
            mViewModel = new PreferencesViewModel(GlobalViewModel.Instance);
            RegisterCommands();
            mView.DataContext = mViewModel;
        }

        private void RegisterCommands()
        {
            mViewModel.SaveCommand = UICommand.Regular(Save);
            mViewModel.CancelCommand = UICommand.Regular(Cancel);
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
                SaveToConfig();
                mViewModel.Model.Password = mViewModel.Password;
                mViewModel.SelectedEncryptionMethod = mViewModel.SelectedEncryptionMethod;
                mViewModel.SelectedSteganographicMethod = mViewModel.SelectedSteganographicMethod;
                return true;
            }
            return false;
        }

        private void SaveToConfig()
        {
            var appPath = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var configFile = System.IO.Path.Combine(appPath, "App.config");
            var configFileMap = new ExeConfigurationFileMap {ExeConfigFilename = configFile};
            var config = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);

            config.AppSettings.Settings["Password"].Value = mViewModel.Password;
            config.AppSettings.Settings["SelectedEncryptionMethod"].Value = mViewModel.SelectedEncryptionMethod.ToString();
            config.AppSettings.Settings["SelectedSteganographicMethod"].Value = mViewModel.SelectedSteganographicMethod.ToString();
            config.Save();
        }
    }
}