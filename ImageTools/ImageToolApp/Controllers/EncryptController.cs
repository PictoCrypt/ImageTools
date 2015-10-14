using System;
using System.Drawing;
using System.IO;
using ImageToolApp.Models;
using ImageToolApp.Views;
using Microsoft.Win32;

namespace ImageToolApp.Controllers
{
    public class EncryptController
    {
        private readonly EncryptViewModel mViewModel;
        private EncryptView mView;

        public EncryptController()
        {
            mView = new EncryptView();
            mViewModel = new EncryptViewModel();
            mView.DataContext = mViewModel;
            RegisterCommands();
        }

        private void RegisterCommands()
        {
            mViewModel.ChooseImageCommand = UICommand.Regular(ChooseImage);
            mViewModel.SaveImageCommand = UICommand.Regular(SaveImage);
            mViewModel.EncryptCommand = UICommand.Regular(Encrypt);
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

        private void Encrypt()
        {
            var bitmap = new Bitmap(mViewModel.SourceImagePath);
            var result = ImageFunctionLib.ImageFunctionLib.Encrypt(bitmap, mViewModel.Text ?? "");
            if (result != null)
            {
                var path = Path.GetTempFileName().Replace("tmp", "jpg");
                result.Save(path);
                mViewModel.ResultImagePath = path;
            }
        }

        private void SaveImage()
        {
            var dialog = new SaveFileDialog();
            dialog.ShowDialog();
            var tmp = mViewModel.ResultImagePath;
            File.Copy(tmp, dialog.FileName);
            mViewModel.ResultImagePath = dialog.FileName;
        }

        public EncryptView GetView()
        {
            return mView;
        }
    }
}