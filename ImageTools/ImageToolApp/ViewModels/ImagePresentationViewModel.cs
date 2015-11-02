using UserControlClassLibrary;

namespace ImageToolApp.ViewModels
{
    public class ImagePresentationViewModel : BaseViewModel
    {
        private string mImage;
        private UICommand mSaveCommand;

        public ImagePresentationViewModel(string image)
        {
            Image = image;
        }

        public string Image
        {
            get { return mImage; }
            set
            {
                if (value.Equals(mImage))
                {
                    return;
                }
                mImage = value;
                OnPropertyChanged("Image");
            }
        }

        public UICommand SaveCommand
        {
            get { return mSaveCommand; }
            set
            {
                if (value.Equals(mSaveCommand))
                {
                    return;
                }
                mSaveCommand = value;
                OnPropertyChanged("SaveCommand");
            }
        }
    }
}