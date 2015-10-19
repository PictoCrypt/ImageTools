using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using ImageFunctionLib;
using ImageToolApp.Models;
using ImageToolApp.Views;
using Microsoft.Win32;

namespace ImageToolApp.Controllers
{
    public class EncryptController : BaseTabController<EncryptView, EncryptViewModel>
    {
        protected override EncryptView CreateView()
        {
            return new EncryptView();
        }

        protected override void RegisterCommands()
        {
            base.RegisterCommands();
            ViewModel.SaveImageCommand = UICommand.Regular(SaveImage);
            ViewModel.EncryptCommand = UICommand.Regular(Encrypt);
            ViewModel.EncryptWithAesCommand = UICommand.Regular(EncryptWithAes);
        }

        private void EncryptWithAes()
        {
            var result = AESEncryption.Encrypt(ViewModel.Text, ViewModel.AesPassword);
            ViewModel.Text = result;
        }

        private void Encrypt()
        {
            var bitmap = new Bitmap(ViewModel.SourceImagePath);
            var result = LeastSignificantBit.Encrypt(bitmap, ViewModel.Text ?? "");
            if (result != null)
            {
                var path = Path.GetTempFileName().Replace("tmp", "jpeg");
                result.Save(path);
                ViewModel.ResultImagePath = path;
            }
        }

        private void SaveImage()
        {
            var dialog = new SaveFileDialog {Filter = "Png Image|*.png|Bitmap Image|*.bmp"};
            var dialogResult = dialog.ShowDialog();
            if (dialogResult.HasValue && dialogResult.Value)
            {
                var tmp = ViewModel.ResultImagePath;
                var bmp = new Bitmap(tmp);
                ViewModel.ResultImagePath = dialog.FileName;

                switch (dialog.FilterIndex)
                {
                    case 0:
                        bmp.Save(dialog.FileName, ImageFormat.Png);
                        break;
                    case 1:
                        bmp.Save(dialog.FileName, ImageFormat.Bmp);
                        break;
                }
            }
        }
    }
}