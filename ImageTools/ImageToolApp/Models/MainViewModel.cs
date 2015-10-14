using System.Windows;

namespace ImageToolApp.Models
{
    public class MainViewModel : BaseViewModel
    {
        private FrameworkElement mEncryptView;
        private FrameworkElement mDecryptView;

        public MainViewModel(FrameworkElement encryptView, FrameworkElement decryptView)
        {
            EncryptView = encryptView;
            DecryptView = decryptView;
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
    }
}