using System.ComponentModel;
using System.Runtime.CompilerServices;
using ImageToolApp.Annotations;

namespace ImageToolApp.Models
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        public GlobalViewModel GlobalViewModel
        {
            get { return GlobalViewModel.Instance; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}