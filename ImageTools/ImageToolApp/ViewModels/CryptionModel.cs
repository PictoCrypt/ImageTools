using System.Collections.Generic;
using System.Linq;
using System.Windows;
using FunctionLib.Cryptography;
using ImageToolApp.Models;
using ImageToolApp.Views;

namespace ImageToolApp.ViewModels
{
    public class CryptionModel : BaseViewModel
    {
        private CryptographicAlgorithmImpl mAlgorithm;

        private bool mIsEnabled;
        private string mPassword;
        private string mKeyFilePath;
        private Visibility mRsaVisible;

        public CryptionModel(string password, CryptographicAlgorithmImpl algorithm)
        {
            if (string.IsNullOrEmpty(password))
            {
                IsEnabled = false;
            }
            else
            {
                IsEnabled = true;
                Password = password;
                ConfirmationPassword = password;
            }

            Algorithm = algorithm ?? AlgorithmList.FirstOrDefault();
        }

        private Settings Settings
        {
            get { return Settings.Instance; }
        }

        public Visibility RsaVisible
        {
            get { return mRsaVisible; }
            set
            {
                if (value == mRsaVisible) return;
                mRsaVisible = value;
                OnPropertyChanged();
            }
        }

        public bool IsEnabled
        {
            get { return mIsEnabled; }
            set
            {
                if (value.Equals(mIsEnabled))
                {
                    return;
                }
                mIsEnabled = value;
                OnPropertyChanged("IsEnabled");
            }
        }

        public string KeyFilePath
        {
            get { return mKeyFilePath; }
            set
            {
                if (value == mKeyFilePath) return;
                mKeyFilePath = value;
                OnPropertyChanged("KeyFilePath");
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
                mIsEnabled = !string.IsNullOrEmpty(mPassword);
                OnPropertyChanged("Password");
            }
        }

        public string ConfirmationPassword { get; set; }

        public CryptographicAlgorithmImpl Algorithm
        {
            get { return mAlgorithm; }
            set
            {
                if (value.Equals(mAlgorithm))
                {
                    return;
                }
                mAlgorithm = value;
                if (mAlgorithm.GetType() == typeof(RsaAlgorithm))
                {
                    RsaVisible = Visibility.Visible;
                }
                else
                {
                    RsaVisible = Visibility.Collapsed;
                }
                OnPropertyChanged("Algorithm");
            }
        }

        public IList<CryptographicAlgorithmImpl> AlgorithmList
        {
            get { return Settings.EncryptionMethods; }
        }
    }
}