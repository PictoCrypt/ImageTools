using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Reflection;
using FunctionLib.Helper;

namespace ImageToolApp.Models
{
    public class SettingsModel
    {
        private static SettingsModel mInstance;

        public SettingsModel(IDictionary<string, Type> encryptionMethods = null,
            IList<Type> steganographicMethods = null)
        {
            LoadConfig();
            EncryptionMethods = encryptionMethods ??
                                new Dictionary<string, Type>(EncryptionMethodHelper.ImplementationList);
            SteganographicMethods = steganographicMethods ??
                                    new ObservableCollection<Type>(SteganographicMethodHelper.ImplementationList);
        }

        public IList<Type> SteganographicMethods { get; }

        public IDictionary<string, Type> EncryptionMethods { get; }

        public static SettingsModel Instance
        {
            get
            {
                if (mInstance == null)
                {
                    mInstance = new SettingsModel();
                }
                return mInstance;
            }
        }

        public string Password { get; private set; } = string.Empty;

        public Type SelectedEncryptionMethod { get; private set; }

        public Type SelectedSteganographicMethod { get; private set; }

        public string StandardPath { get; private set; }

        public void LoadConfig()
        {
            var configFileMap = new ExeConfigurationFileMap
            {
                ExeConfigFilename = Path.Combine(Constants.ExecutiongPath, "App.config")
            };
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
                        StandardPath = !string.IsNullOrEmpty(value)
                            ? value
                            : Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                        break;
                    case "SelectedEncryptionMethod":
                        Type encryptMethod;
                        EncryptionMethodHelper.ImplementationList.TryGetValue(value, out encryptMethod);
                        SelectedEncryptionMethod = encryptMethod;
                        break;
                    case "SelectedSteganographicMethod":
                        var steganoMethod = SteganographicMethodHelper.ImplementationList.Find(
                            x => x.ToString().Equals(value, StringComparison.OrdinalIgnoreCase));
                        SelectedSteganographicMethod = steganoMethod;
                        break;
                }
            }
        }

        public void SaveToConfig(string password, string encryptionMethod, string steganographicMethod,
            string standardPath)
        {
            var appPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var configFile = Path.Combine(appPath, "App.config");
            var configFileMap = new ExeConfigurationFileMap {ExeConfigFilename = configFile};
            var config = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);

            config.AppSettings.Settings["Password"].Value = password;
            config.AppSettings.Settings["SelectedEncryptionMethod"].Value = encryptionMethod;
            config.AppSettings.Settings["SelectedSteganographicMethod"].Value = steganographicMethod;
            config.AppSettings.Settings["StandardPath"].Value = standardPath;
            config.Save();
            mInstance = new SettingsModel(EncryptionMethods, SteganographicMethods);
        }
    }
}