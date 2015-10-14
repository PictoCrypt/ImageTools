using System.Drawing;
using System.IO;
using ImageToolApp.Models;
using ImageToolApp.Views;
using Microsoft.Win32;

namespace ImageToolApp.Controllers
{
    public class DecryptController
    {
        private readonly DecryptView mView;
        private readonly DecryptViewModel mViewModel;

        public DecryptController()
        {
            mView = new DecryptView();
            mViewModel = new DecryptViewModel();
            mView.DataContext = mViewModel;
            RegisterCommands();
        }

        private void RegisterCommands()
        {
            mViewModel.ChooseImageCommand = UICommand.Regular(ChooseImage);
            mViewModel.DecryptCommand = UICommand.Regular(Decrypt);
        }

        private void Decrypt()
        {
            var bitmap = new Bitmap(mViewModel.SourceImagePath);
            var result = ImageFunctionLib.ImageFunctionLib.DecryptText(bitmap);
            mViewModel.Text = result;
        }

        private void ChooseImage()
        {
            var dialog = new OpenFileDialog();
            dialog.ShowDialog();
            dialog.Multiselect = false;
            if (string.IsNullOrEmpty(dialog.FileName))
            {
                return;
            }
            mViewModel.SourceImagePath = dialog.FileName;
        }

        public DecryptView GetView()
        {
            return mView;
        }
    }
}