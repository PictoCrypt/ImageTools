using System;
using System.Configuration;
using FunctionLib;

namespace ImageToolApp.Models
{
    public class GlobalViewModel : BaseViewModel
    {
        private string mImagePath;
        private string mPassword;
        private EncryptionMethod mSelectedEncryptionMethod;
        private SteganographicMethod mSelectedSteganographicMethod;
        private string mResultImagePath;
        private static GlobalViewModel mInstance;

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

        private void ReadAppConfigData()
        {
            foreach (string key in ConfigurationManager.AppSettings)
            {
                var value = ConfigurationManager.AppSettings[key];
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
            get { return mImagePath; }
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
    }
}