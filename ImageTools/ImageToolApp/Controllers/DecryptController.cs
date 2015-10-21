using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Input;
using FunctionLib.Cryptography;
using FunctionLib.Steganography;
using ImageToolApp.Models;
using ImageToolApp.Views;
using Microsoft.Win32;

namespace ImageToolApp.Controllers
{
    public class DecryptController : BaseTabController<DecryptView, DecryptViewModel>
    {
        protected override DecryptView CreateView()
        {
            return new DecryptView();
        }

        protected override void RegisterCommands()
        {
            ViewModel.TabActionCommand = UICommand.Regular(Decrypt);
        }

        public void SaveTxt()
        {
            var dialog = new SaveFileDialog {Filter = "Text File|*.txt"};
            var dialogResult = dialog.ShowDialog();
            if (dialogResult.HasValue && dialogResult.Value)
            {
                using (var stream = File.CreateText(dialog.FileName))
                {
                    stream.Write(ViewModel.Text); 
                }
            }
        }

        private void Decrypt()
        {
            Application.Current.MainWindow.Cursor = Cursors.Wait;
            string result;
            using (var bitmap = new Bitmap(ViewModel.GlobalViewModel.ImagePath))
            {
                result = StegaCrypt.DecryptText(bitmap);
            }

            if (ViewModel.EncryptedCheck)
            {
                result = Crypt.Decrypt(result, ViewModel.Password);
            }
            ViewModel.Text = result;
            Application.Current.MainWindow.Cursor = Cursors.Arrow;
        }
    }
}