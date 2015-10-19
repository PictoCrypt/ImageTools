using System.Drawing;
using System.IO;
using FunctionLib;
using ImageToolApp.Models;
using ImageToolApp.Views;

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

        private void Encrypt()
        {
            var bitmap = new Bitmap(ViewModel.GlobalViewModel.ImagePath);
            var cryptedText = Crypto.Encrypt(ViewModel.Text ?? "", ViewModel.Password);
            var result = LeastSignificantBit.Encrypt(bitmap, cryptedText);
            if (result != null)
            {
                var path = Path.GetTempFileName().Replace("tmp", "jpeg");
                result.Save(path);
                ViewModel.GlobalViewModel.ResultImagePath = path;
            }
        }

        //private void SaveImage()
        //{
        //    var dialog = new SaveFileDialog {Filter = "Png Image|*.png|Bitmap Image|*.bmp"};
        //    var dialogResult = dialog.ShowDialog();
        //    if (dialogResult.HasValue && dialogResult.Value)
        //    {
        //        var tmp = ViewModel.ResultImagePath;
        //        var bmp = new Bitmap(tmp);

        //        if (File.Exists(dialog.FileName))
        //        {
        //            File.Delete(dialog.FileName);
        //        }
        //        ViewModel.ResultImagePath = dialog.FileName;

        //        switch (dialog.FilterIndex)
        //        {
        //            case 0:
        //                bmp.Save(dialog.FileName, ImageFormat.Png);
        //                break;
        //            case 1:
        //                bmp.Save(dialog.FileName, ImageFormat.Bmp);
        //                break;
        //        }
        //        bmp.Dispose();
        //    }
        //}
    }
}