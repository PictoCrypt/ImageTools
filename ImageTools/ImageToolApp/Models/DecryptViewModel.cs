namespace ImageToolApp.Models
{
    public class DecryptViewModel : BaseTabViewModel
    {
        private UICommand mDecryptCommand;
        private UICommand mDecryptTextCommand;

        public UICommand DecryptCommand
        {
            get { return mDecryptCommand; }
            set
            {
                if (value.Equals(mDecryptCommand))
                {
                    return;
                }
                mDecryptCommand = value;
                OnPropertyChanged("DecryptCommand");
            }
        }

        public UICommand DecryptTextCommand
        {
            get { return mDecryptTextCommand; }
            set
            {
                if (value.Equals(mDecryptTextCommand))
                {
                    return;
                }
                mDecryptTextCommand = value;
                OnPropertyChanged("DecryptTextCommand");
            }
        }
    }
}