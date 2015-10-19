using System;
using System.IO;
using System.Windows.Controls;
using ImageToolApp.Models;
using Microsoft.Win32;

namespace ImageToolApp.Controllers
{
    public abstract class BaseTabController<TView, TViewModel> : IBaseTabController
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

        public void OpenImage()
        {
            var dialog = new OpenFileDialog();
            dialog.ShowDialog();
            dialog.Multiselect = false;
            if (string.IsNullOrEmpty(dialog.FileName))
            {
                return;
            }
            
            var tmp = Path.ChangeExtension(Path.GetTempFileName(), Path.GetExtension(dialog.FileName));
            File.Copy(dialog.FileName, tmp);
            ViewModel.GlobalViewModel.ImagePath = tmp;
        }
    }
}