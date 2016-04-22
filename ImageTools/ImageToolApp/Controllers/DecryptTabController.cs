using System.IO;
using FunctionLib.Cryptography;
using FunctionLib.Model;
using ImageToolApp.ViewModels;
using ImageToolApp.Views;
using Microsoft.Win32;
using UserControlClassLibrary;

namespace ImageToolApp.Controllers
{
    public class DecryptTabController : BaseTabController<DecryptTabViewModel, DecryptTabView>
    {
        protected override UICommand ActionCommand
        {
            get { return UICommand.Regular(Decrypt); }
        }

        public void SaveTxt()
        {
            var dialog = new SaveFileDialog {Filter = "Text File|*.txt"};
            var dialogResult = dialog.ShowDialog();
            if (dialogResult.HasValue && dialogResult.Value)
            {
                using (var stream = File.CreateText(dialog.FileName))
                {
                    stream.Write(ViewModel.Result);
                }
            }
        }

        private void Decrypt()
        {
            HandleJobController.Progress(() =>
            {
                //TODO Compression
                var algorithm = ViewModel.CryptionModel.Algorithm as RsaAlgorithm;
                DecodeModel model;
                if (algorithm != null)
                {
                    string key;
                    using (var sr = new StreamReader(ViewModel.CryptionModel.KeyFilePath))
                    {
                        key = sr.ReadToEnd();
                    }

                    model = new DecodeModel(ViewModel.ImagePath, ViewModel.CryptionModel.Algorithm,
                        key, ViewModel.SteganographicModel.Algorithm,
                        ViewModel.SteganographicModel.Compression, ViewModel.SteganographicModel.LsbIndicator);
                }
                else
                {
                    model = new DecodeModel(ViewModel.ImagePath, ViewModel.CryptionModel.Algorithm,
                        ViewModel.CryptionModel.Password, ViewModel.SteganographicModel.Algorithm,
                        ViewModel.SteganographicModel.Compression, ViewModel.SteganographicModel.LsbIndicator);
                }


                ViewModel.Result = model.Decode();
            });
        }
    }
}