using System.Collections.Generic;
using System.Linq;
using FunctionLib.Cryptography;
using ImageToolApp.Models;

namespace ImageToolApp.ViewModels
{
    public class EncryptionModel : BaseViewModel
    {
        private bool mIsEnabled;
        private string mPassword;
        private CryptographicAlgorithmImpl mAlgorithm;

        public EncryptionModel(string password, CryptographicAlgorithmImpl algorithm)
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
                OnPropertyChanged("Algorithm");
            }
        }

        public IList<CryptographicAlgorithmImpl> AlgorithmList { get { return Settings.Instance.EncryptionMethods; } } 
    }
}