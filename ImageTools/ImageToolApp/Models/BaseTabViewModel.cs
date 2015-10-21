﻿namespace ImageToolApp.Models
{
    public class BaseTabViewModel : BaseViewModel
    {
        private bool mEncryptedCheck;
        private string mPassword;

        private UICommand mTabActionCommand;

        private string mText;

        public BaseTabViewModel()
        {
            Password = GlobalViewModel.Password;
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
    }
}