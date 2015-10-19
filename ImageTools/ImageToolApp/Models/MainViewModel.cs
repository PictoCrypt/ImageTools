using System.Windows;

namespace ImageToolApp.Models
{
    public class MainViewModel : BaseViewModel
    {
        private readonly FrameworkElement mDecryptView;
        private readonly FrameworkElement mEncryptView;
        private UICommand mCloseAppCommand;
        private FrameworkElement mCurrentElement;
        private UICommand mPreferencesCommand;
        private UICommand mOpenImageCommand;
        private UICommand mSaveImageCommand;
        private UICommand mOpenTxtCommand;
        private UICommand mSaveTxtCommand;

        public MainViewModel(FrameworkElement encryptView, FrameworkElement decryptView)
        {
            mEncryptView = encryptView;
            mDecryptView = decryptView;
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

        public UICommand PreferencesCommand
        {
            get { return mPreferencesCommand; }
            set
            {
                if (value.Equals(mPreferencesCommand))
                {
                    return;
                }
                mPreferencesCommand = value;
                OnPropertyChanged("PreferencesCommand");
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

        public int SelectedRibbonTabIndex
        {
            set
            {
                switch (value)
                {
                    case 0:
                        CurrentElement = mEncryptView;
                        break;
                    case 1:
                        CurrentElement = mDecryptView;
                        break;
                }
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
    }
}