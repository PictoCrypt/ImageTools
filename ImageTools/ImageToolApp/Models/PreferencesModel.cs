using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using FunctionLib;
using FunctionLib.Helper;

namespace ImageToolApp.Models
{
    public class PreferencesModel
    {
        private static PreferencesModel mInstance;

        public PreferencesModel(ObservableCollection<EncryptionMethod> encryptionMethods = null, ObservableCollection<SteganographicMethod> steganographicMethods = null)
        {
            LoadConfig();
            EncryptionMethods = encryptionMethods ?? new ObservableCollection<EncryptionMethod>(Enum.GetValues(typeof(EncryptionMethod))
                    .Cast<EncryptionMethod>());
            SteganographicMethods = steganographicMethods ?? new ObservableCollection<SteganographicMethod>(Enum.GetValues(typeof(SteganographicMethod))
                    .Cast<SteganographicMethod>());
        }

        public ObservableCollection<SteganographicMethod> SteganographicMethods { get; private set; }

        public ObservableCollection<EncryptionMethod> EncryptionMethods { get; private set; }

        public static PreferencesModel Instance
        {
            get
            {
                if (mInstance == null)
                {
                    mInstance = new PreferencesModel();
                }
                return mInstance;
            }
        }

        public string Password { get; private set; } = string.Empty;

        public EncryptionMethod SelectedEncryptionMethod { get; private set; }

        public SteganographicMethod SelectedSteganographicMethod { get; private set; }

        public string StandardPath { get; private set; }

        public void LoadConfig()
        {
            var configFileMap = new ExeConfigurationFileMap { ExeConfigFilename = Path.Combine(MethodHelper.ExecutiongPath, "App.config") };
            var config = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);

            foreach (var key in config.AppSettings.Settings.AllKeys)
            {
                var value = config.AppSettings.Settings[key].Value;
                switch (key)
                {
                    case "Password":
                        Password = value ?? string.Empty;
                        break;
                    case "StandardPath":
                        StandardPath = !string.IsNullOrEmpty(value) ? value : Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                        break;
                    case "SelectedEncryptionMethod":
                        EncryptionMethod encryptMethod;
                        Enum.TryParse(value, out encryptMethod);
                        SelectedEncryptionMethod = encryptMethod;
                        break;
                    case "SelectedSteganographicMethod":
                        SteganographicMethod steganoMethod;
                        Enum.TryParse(value, out steganoMethod);
                        SelectedSteganographicMethod = steganoMethod;
                        break;
                }
            }
        }

        public void SaveToConfig(string password, string encryptionMethod, string steganographicMethod, string standardPath)
        {
            var appPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var configFile = Path.Combine(appPath, "App.config");
            var configFileMap = new ExeConfigurationFileMap { ExeConfigFilename = configFile };
            var config = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);

            config.AppSettings.Settings["Password"].Value = password;
            config.AppSettings.Settings["SelectedEncryptionMethod"].Value = encryptionMethod;
            config.AppSettings.Settings["SelectedSteganographicMethod"].Value = steganographicMethod;
            config.AppSettings.Settings["StandardPath"].Value = standardPath;
            config.Save();
            mInstance = new PreferencesModel(EncryptionMethods, SteganographicMethods);
        }
    }
}