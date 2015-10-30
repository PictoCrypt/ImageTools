using ImageToolApp.Models;

namespace ImageToolApp.ViewModels
{
    public abstract class BaseViewModel : PropertyChangedModel
    {
        public SettingsModel SettingsModel
        {
            get { return SettingsModel.Instance; }
        }
    }
}