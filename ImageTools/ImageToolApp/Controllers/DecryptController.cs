using System;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Input;
using FunctionLib;
using FunctionLib.Cryptography;
using FunctionLib.Cryptography.Blowfish;
using FunctionLib.Cryptography.Twofish;
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
                switch (ViewModel.GlobalViewModel.SelectedEncryptionMethod)
                {
                    case EncryptionMethod.AES:
                        result = SymmetricAlgorithmBase.Decrypt(result, ViewModel.Password);
                        break;

                    case EncryptionMethod.DES:
                        result = SymmetricAlgorithmBase.Decrypt<DESCryptoServiceProvider>(result, ViewModel.Password);
                        break;
                    case EncryptionMethod.RC2:
                        result = SymmetricAlgorithmBase.Decrypt<RC2CryptoServiceProvider>(result, ViewModel.Password);
                        break;
                    case EncryptionMethod.Rijndael:
                        result = SymmetricAlgorithmBase.Decrypt<RijndaelManaged>(result, ViewModel.Password);
                        break;
                    case EncryptionMethod.TripleDES:
                        result = SymmetricAlgorithmBase.Decrypt<TripleDESCryptoServiceProvider>(result, ViewModel.Password);
                        break;

                    case EncryptionMethod.Twofish:
                        result = SymmetricAlgorithmBase.Decrypt<Twofish>(result, ViewModel.Password);
                        break;

                    case EncryptionMethod.Blowfish:
                        result = SymmetricAlgorithmBase.Encrypt<BlowfishAlgorithm>(result, ViewModel.Password);
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            ViewModel.Text = result;
            Application.Current.MainWindow.Cursor = Cursors.Arrow;
        }
    }
}