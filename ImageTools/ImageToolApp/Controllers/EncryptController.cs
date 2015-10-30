using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Input;
using FunctionLib;
using FunctionLib.Cryptography;
using FunctionLib.Cryptography.Blowfish;
using FunctionLib.Cryptography.Twofish;
using FunctionLib.Steganography;
using ImageToolApp.ViewModels;
using ImageToolApp.Views;
using Microsoft.Win32;

namespace ImageToolApp.Controllers
{
    public class EncryptController : BaseTabController<EncryptView, BaseTabViewModel>
    {
        protected override void RegisterCommands()
        {
            ViewModel.TabActionCommand = UICommand.Regular(Encrypt);
        }

        public void SaveImage()
        {
            var dialog = new SaveFileDialog {Filter = "Png Image|*.png|Bitmap Image|*.bmp"};
            var dialogResult = dialog.ShowDialog();
            if (dialogResult.HasValue && dialogResult.Value)
            {
                var tmp = ViewModel.ResultImagePath;
                using (var bmp = new Bitmap(tmp))
                {
                    if (File.Exists(dialog.FileName))
                    {
                        File.Delete(dialog.FileName);
                    }
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

        public void OpenTxt()
        {
            var dialog = new OpenFileDialog {Filter = "Text File|*.txt"};
            var dialogResult = dialog.ShowDialog();
            if (dialogResult.HasValue && dialogResult.Value)
            {
                ViewModel.Text = File.ReadAllText(dialog.FileName);
            }
        }

        private void Encrypt()
        {
            Application.Current.MainWindow.Cursor = Cursors.Wait;
            using (var bitmap = new Bitmap(ViewModel.ImagePath))
            {
                var text = ViewModel.Text;
                if (ViewModel.EncryptedCheck)
                {
                    text = SymmetricAlgorithmBase.Encrypt(this, ViewModel.SelectedEncryptionMethod, text, ViewModel.Password);
                }


                var result = SteganographicAlgorithmBase.Encrypt(this, ViewModel.SelectedSteganographicMethod, bitmap, text);
                if (result != null)
                {
                    var path = Path.GetTempFileName().Replace("tmp", "png");
                    result.Save(path);
                    ViewModel.ResultImagePath = path;
                }
            }
            Application.Current.MainWindow.Cursor = Cursors.Arrow;
        }

        public void ChangedPixels()
        {
            var path = SteganographicAlgorithmBase.ChangeColor(ViewModel.ResultImagePath, Color.Red);
            var view = new ImagePresentation();
            var viewModel = new ImagePresentationViewModel(path)
            {
                SaveCommand = UICommand.Regular(() =>
                {
                    var dialog = new SaveFileDialog
                    {
                        Filter = "Png Image|*.png|Bitmap Image|*.bmp",
                        InitialDirectory = ViewModel.SettingsModel.StandardPath
                    };
                    var dialogResult = dialog.ShowDialog();
                    if (dialogResult.HasValue && dialogResult.Value)
                    {
                        var tmp = path;
                        using (var bmp = new Bitmap(tmp))
                        {
                            if (File.Exists(dialog.FileName))
                            {
                                File.Delete(dialog.FileName);
                            }

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
                })
            };
            view.DataContext = viewModel;
            view.Show();
        }
    }
}