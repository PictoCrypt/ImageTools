using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using FunctionLib;
using UserControlClassLibrary;
using Color = System.Windows.Media.Color;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace ImageToolApp.ViewModels
{
    public class BaseTabViewModel : BaseViewModel
    {
        private bool mEncryptedCheck;
        private string mImagePath = string.Empty;
        private string mPassword;
        private string mResultImagePath = string.Empty;
        private EncryptionMethod mSelectedEncryptionMethod;
        private SteganographicMethod mSelectedSteganographicMethod;

        private UICommand mTabActionCommand;

        private string mText;
        private int mNumericUpDownValue = 3;
        private int mCurrentMaxCharacterLength;

        public BaseTabViewModel()
        {
            Password = SettingsModel.Password;
            SelectedSteganographicMethod = SettingsModel.SelectedSteganographicMethod;
            SelectedEncryptionMethod = SettingsModel.SelectedEncryptionMethod;
        }

        public int NumericUpDownValue
        {
            get { return mNumericUpDownValue; }
            set
            {
                if (value.Equals(mNumericUpDownValue))
                {
                    return;
                }
                mNumericUpDownValue = value;
                OnPropertyChanged("NumericUpDownValue");
            }
        }

        public UICommand TabActionCommand
        {
            get { return mTabActionCommand; }
            set
            {
                if (value.Equals(mTabActionCommand))
                {
                    return;
                }
                mTabActionCommand = value;
                OnPropertyChanged("TabActionCommand");
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
                OnPropertyChanged("SelectedEncryptionMethod");
            }
        }

        public ObservableCollection<EncryptionMethod> EncryptionMethods
        {
            get { return SettingsModel.EncryptionMethods; }
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
                OnPropertyChanged("SelectedSteganographicMethod");
            }
        }

        public ObservableCollection<SteganographicMethod> SteganographicMethods
        {
            get { return SettingsModel.SteganographicMethods; }
        }

        public string Text
        {
            get { return mText; }
            set
            {
                if (value.Equals(mText))
                {
                    return;
                }
                mText = value;
                OnPropertyChanged("Text");
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
                if (!string.IsNullOrEmpty(mPassword))
                {
                    EncryptedCheck = true;
                }
            }
        }

        public bool EncryptedCheck
        {
            get { return mEncryptedCheck || !string.IsNullOrEmpty(Password); }
            set
            {
                if (value.Equals(mEncryptedCheck))
                {
                    return;
                }
                mEncryptedCheck = value;
                OnPropertyChanged("EncryptedCheck");
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
    }
}