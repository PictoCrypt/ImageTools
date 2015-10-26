using System;
using System.IO;
using System.Windows.Controls;
using FunctionLib;
using FunctionLib.Steganography;
using ImageToolApp.Models;
using Microsoft.Win32;

namespace ImageToolApp.Controllers
{
    public abstract class BaseTabController<TView, TViewModel> : IBaseTabController
        where TView : UserControl
        where TViewModel : BaseTabViewModel, new()
    {
        protected readonly TViewModel ViewModel;

        protected BaseTabController()
        {
            InitializeCryptings();
            View = CreateView();
            ViewModel = new TViewModel();
            View.DataContext = ViewModel;
            RegisterCommands();
        }

        //public Type SymmetricAlgorithmBase { get; private set; }

        public StegaCrypt StegaCrypt { get; set; }

        public TView View { get; }

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

        protected abstract TView CreateView();

        protected abstract void RegisterCommands();
        
        public void InitializeCryptings()
        {
            switch (GlobalViewModel.Instance.SelectedSteganographicMethod)
            {
                case SteganographicMethod.LSB:
                    StegaCrypt = new LeastSignificantBit();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}