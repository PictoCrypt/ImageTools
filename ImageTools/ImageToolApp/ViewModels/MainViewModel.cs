using System.Windows;
using UserControlClassLibrary;

namespace ImageToolApp.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private UICommand mChangedPixelsCommand;
        private UICommand mCloseAppCommand;
        private FrameworkElement mCurrentElement;
        private FrameworkElement mDecryptView;
        private FrameworkElement mEncryptView;
        private UICommand mOpenHelpCommand;
        private UICommand mOpenImageCommand;
        private UICommand mOpenTxtCommand;
        private bool mProgressActive;
        private UICommand mSaveImageCommand;
        private UICommand mSaveTxtCommand;
        private UICommand mSettingsCommand;

        public MainViewModel(FrameworkElement encryptView, FrameworkElement decryptView)
        {
            CurrentElement = encryptView;
            EncryptView = encryptView;
            DecryptView = decryptView;
        }

        public bool ProgressActive
        {
            get { return mProgressActive; }
            set
            {
                if (value.Equals(mProgressActive))
                {
                    return;
                }
                mProgressActive = value;
                OnPropertyChanged("ProgressActive");
            }
        }

        public bool EncryptSelected
        {
            set { CurrentElement = value ? EncryptView : DecryptView; }
        }

        public UICommand OpenImageCommand
        {
            get { return mOpenImageCommand; }
            set
            {
                if (value.Equals(mOpenImageCommand))
                {
                    return;
                }
                mOpenImageCommand = value;
                OnPropertyChanged("OpenImageCommand");
            }
        }

        public UICommand SaveImageCommand
        {
            get { return mSaveImageCommand; }
            set
            {
                if (value.Equals(mSaveImageCommand))
                {
                    return;
                }
                mSaveImageCommand = value;
                OnPropertyChanged("SaveImageCommand");
            }
        }

        public UICommand OpenTxtCommand
        {
            get { return mOpenTxtCommand; }
            set
            {
                if (value.Equals(mOpenTxtCommand))
                {
                    return;
                }
                mOpenTxtCommand = value;
                OnPropertyChanged("OpenTxtCommand");
            }
        }

        public UICommand SaveTxtCommand
        {
            get { return mSaveTxtCommand; }
            set
            {
                if (value.Equals(mSaveTxtCommand))
                {
                    return;
                }
                mSaveTxtCommand = value;
                OnPropertyChanged("SaveTxtCommand");
            }
        }

        public UICommand SettingsCommand
        {
            get { return mSettingsCommand; }
            set
            {
                if (value.Equals(mSettingsCommand))
                {
                    return;
                }
                mSettingsCommand = value;
                OnPropertyChanged("SettingsCommand");
            }
        }

        public UICommand CloseAppCommand
        {
            get { return mCloseAppCommand; }
            set
            {
                if (value.Equals(mCloseAppCommand))
                {
                    return;
                }
                mCloseAppCommand = value;
                OnPropertyChanged("CloseAppCommand");
            }
        }

        public UICommand OpenHelpCommand
        {
            get { return mOpenHelpCommand; }
            set
            {
                if (value.Equals(mOpenHelpCommand))
                {
                    return;
                }
                mOpenHelpCommand = value;
                OnPropertyChanged("OpenHelpCommand");
            }
        }

        public FrameworkElement EncryptView
        {
            get { return mEncryptView; }
            set
            {
                if (value.Equals(mEncryptView))
                {
                    return;
                }
                mEncryptView = value;
                OnPropertyChanged("EncryptView");
            }
        }

        public FrameworkElement DecryptView
        {
            get { return mDecryptView; }
            set
            {
                if (value.Equals(mDecryptView))
                {
                    return;
                }
                mDecryptView = value;
                OnPropertyChanged("DecryptView");
            }
        }

        public FrameworkElement CurrentElement
        {
            get { return mCurrentElement; }
            set
            {
                if (value.Equals(mCurrentElement))
                {
                    return;
                }
                mCurrentElement = value;
                OnPropertyChanged("CurrentElement");
            }
        }

        public UICommand ChangedPixelsCommand
        {
            get { return mChangedPixelsCommand; }
            set
            {
                if (value.Equals(mChangedPixelsCommand))
                {
                    return;
                }
                mChangedPixelsCommand = value;
                OnPropertyChanged("ChangedPixelsCommand");
            }
        }
    }
}