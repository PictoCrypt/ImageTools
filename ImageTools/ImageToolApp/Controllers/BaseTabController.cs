using ImageToolApp.Models;
using System.Windows.Controls;
using Microsoft.Win32;

namespace ImageToolApp.Controllers
{
    public abstract class BaseTabController<TView, TViewModel>
        where TView : UserControl
        where TViewModel : BaseTabViewModel, new()
    {
        protected TView View;
        protected readonly TViewModel ViewModel;

        protected BaseTabController()
        {
            View = CreateView();
            ViewModel = new TViewModel();
            View.DataContext = ViewModel;
            RegisterCommands();
        }

        protected abstract TView CreateView();

        protected virtual void RegisterCommands()
        {
            ViewModel.ChooseImageCommand = UICommand.Regular(ChooseImage);

        }

        protected void ChooseImage()
        {
            var dialog = new OpenFileDialog();
            dialog.ShowDialog();
            dialog.Multiselect = false;
            if (string.IsNullOrEmpty(dialog.FileName))
            {
                return;
            }
            ViewModel.SourceImagePath = dialog.FileName;
        }

        public TView GetView()
        {
            return View;
        }
    }
}