using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using FunctionLib;

namespace ImageToolApp.Models
{
    public class GlobalViewModel : BaseViewModel
    {
        private static GlobalViewModel mInstance;
        private string mImagePath;
        private string mPassword;
        private string mResultImagePath;
        private EncryptionMethod mSelectedEncryptionMethod;
        private SteganographicMethod mSelectedSteganographicMethod;
        private FrameworkElement mImageOrNullImage;

        public GlobalViewModel()
        {
            ReadAppConfigData();
        }

        public static GlobalViewModel Instance
        {
            get
            {
                if (mInstance == null)
                {
                    mInstance = new GlobalViewModel();
                }
                return mInstance;
            }
        }

        public string Password
        {
            get { return mPassword; }
            set
            {
                if (value.Equals(mPassword))
                {
                    return;
                }
                mPassword = value;
                OnPropertyChanged("Password");
            }
        }

        public string ImagePath
        {
            get
            {
                if (!string.IsNullOrEmpty(mImagePath))
                    return mImagePath;

                var myResourceDictionary = new ResourceDictionary
                {
                    Source = new Uri("/ImageToolApp;component/Resources/ResourceDictionary.xaml",
                        UriKind.RelativeOrAbsolute)
                };
                var result = myResourceDictionary["BrokenImage"].ToString();
                return result;
            }
            set
            {
                if (value.Equals(mImagePath))
                {
                    return;
                }
                mImagePath = value;
                OnPropertyChanged("ImagePath");
            }
        }

        public string ResultImagePath
        {
            get { return mResultImagePath; }
            set
            {
                if (value.Equals(mResultImagePath))
                {
                    return;
                }
                mResultImagePath = value;
                OnPropertyChanged("ResultImagePath");
            }
        }

        public EncryptionMethod SelectedEncryptionMethod
        {
            get { return mSelectedEncryptionMethod; }
            set
            {
                if (value.Equals(mSelectedEncryptionMethod))
                {
                    return;
                }
                mSelectedEncryptionMethod = value;
            }
        }

        public SteganographicMethod SelectedSteganographicMethod
        {
            get { return mSelectedSteganographicMethod; }
            set
            {
                if (value.Equals(mSelectedSteganographicMethod))
                {
                    return;
                }
                mSelectedSteganographicMethod = value;
            }
        }

        private void ReadAppConfigData()
        {
            var appPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var configFile = Path.Combine(appPath, "App.config");
            var configFileMap = new ExeConfigurationFileMap {ExeConfigFilename = configFile};
            var config = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);

            foreach (var key in config.AppSettings.Settings.AllKeys)
            {
                var value = config.AppSettings.Settings[key].Value;
                switch (key)
                {
                    case "Password":
                        Password = value;
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
    }
}