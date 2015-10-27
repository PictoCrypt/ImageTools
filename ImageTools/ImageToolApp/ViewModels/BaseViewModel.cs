using ImageToolApp.Models;

namespace ImageToolApp.ViewModels
{
    public abstract class BaseViewModel : PropertyChangedModel
    {
        public PreferencesModel PreferencesModel
        {
            get { return PreferencesModel.Instance; }
        }
    }
}