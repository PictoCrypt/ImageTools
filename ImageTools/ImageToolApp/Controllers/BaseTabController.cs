using System.Windows.Controls;
using ImageToolApp.Models;

namespace ImageToolApp.Controllers
{
    public abstract class BaseTabController<TView, TViewModel>
        where TView : UserControl
        where TViewModel : BaseTabViewModel, new()
    {
        private readonly TView mView;
        protected readonly TViewModel ViewModel;

        protected BaseTabController()
        {
            mView = CreateView();
            ViewModel = new TViewModel();
            mView.DataContext = ViewModel;
            RegisterCommands();
        }

        protected abstract TView CreateView();

        protected virtual void RegisterCommands()
        {
        }

        public TView GetView()
        {
            return mView;
        }
    }
}