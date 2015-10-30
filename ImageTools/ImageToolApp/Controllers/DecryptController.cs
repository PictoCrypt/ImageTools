using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Input;
using FunctionLib.Cryptography;
using FunctionLib.Steganography;
using ImageToolApp.ViewModels;
using Microsoft.Win32;

namespace ImageToolApp.Controllers
{
    public class DecryptController : BaseTabController<BaseTabViewModel>
    {
        public DecryptController(string viewName, bool textBoxReadOnly) : base(viewName, textBoxReadOnly)
        {
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
            using (var bitmap = new Bitmap(ViewModel.ImagePath))
            {
                result = SteganographicAlgorithmBase.Decrypt(this, ViewModel.SelectedSteganographicMethod, bitmap);
            }

            if (ViewModel.EncryptedCheck)
            {
                result = SymmetricAlgorithmBase.Decrypt(this, ViewModel.SelectedEncryptionMethod, result, ViewModel.Password);
            }

            ViewModel.Text = result;
            Application.Current.MainWindow.Cursor = Cursors.Arrow;
        }
    }
}