using System.Windows;

namespace ImageToolApp.Models
{
    public class MainViewModel : BaseViewModel
    {
        private readonly FrameworkElement mDecryptView;
        private readonly FrameworkElement mEncryptView;
        private UICommand mCloseAppCommand;
        private FrameworkElement mCurrentElement;
        private UICommand mLoadImageCommand;
        private UICommand mPreferencesCommand;

        public MainViewModel(FrameworkElement encryptView, FrameworkElement decryptView)
        {
            mEncryptView = encryptView;
            mDecryptView = decryptView;
        }

        public UICommand LoadImageCommand
        {
            get { return mLoadImageCommand; }
            set
            {
                if (value.Equals(mLoadImageCommand))
                {
                    return;
                }
                mLoadImageCommand = value;
                OnPropertyChanged("LoadImageCommand");
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