using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using FunctionLib;
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
            ViewModel.TabActionCommand = UICommand.Regular(Encrypt);
        }

        public void SaveImage()
        {
            var dialog = new SaveFileDialog { Filter = "Png Image|*.png|Bitmap Image|*.bmp" };
            var dialogResult = dialog.ShowDialog();
            if (dialogResult.HasValue && dialogResult.Value)
            {
                var tmp = ViewModel.GlobalViewModel.ResultImagePath;
                var bmp = new Bitmap(tmp);

                if (File.Exists(dialog.FileName))
                {
                    File.Delete(dialog.FileName);
                }
                ViewModel.GlobalViewModel.ResultImagePath = dialog.FileName;

                switch (dialog.FilterIndex)
                {
                    case 0:
                        bmp.Save(dialog.FileName, ImageFormat.Png);
                        break;
                    case 1:
                        bmp.Save(dialog.FileName, ImageFormat.Bmp);
                        break;
                }
                bmp.Dispose();
            }
        }
        
        public override void OpenImage()
        {
            var dialog = new OpenFileDialog();
            dialog.ShowDialog();
            dialog.Multiselect = false;
            if (string.IsNullOrEmpty(dialog.FileName))
            {
                return;
            }
            ViewModel.GlobalViewModel.ImagePath = dialog.FileName;
        }

        public void OpenTxt()
        {
            throw new System.NotImplementedException();
        }

        private void Encrypt()
        {
            var bitmap = new Bitmap(ViewModel.GlobalViewModel.ImagePath);
            var text = ViewModel.Text;
            if (ViewModel.EncryptedCheck)
            {
                text = Crypto.Encrypt(ViewModel.Text ?? "", ViewModel.Password);
                
            }
            var result = LeastSignificantBit.Encrypt(bitmap, text);
            if (result != null)
            {
                var path = Path.GetTempFileName().Replace("tmp", "png");
                result.Save(path);
                ViewModel.GlobalViewModel.ResultImagePath = path;
            }
        }
    }
}