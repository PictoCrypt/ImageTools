using ImageToolApp.Models;
using UserControlClassLibrary;

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