using System.ComponentModel;
using System.Runtime.CompilerServices;
using UserControlClassLibrary.Annotations;

namespace UserControlClassLibrary
{
    public class PropertyChangedModel : INotifyPropertyChanged
    {
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