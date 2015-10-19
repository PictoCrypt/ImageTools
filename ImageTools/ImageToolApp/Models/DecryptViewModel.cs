namespace ImageToolApp.Models
{
    public class DecryptViewModel : BaseTabViewModel
    {
        private UICommand mDecryptCommand;
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
    }
}