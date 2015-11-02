using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FunctionLib.Cryptography;
using FunctionLib.Steganography;
using ImageToolApp.ViewModels;
using ImageToolApp.Views;
using MahApps.Metro.Controls;
using Microsoft.Win32;
using UserControlClassLibrary;
using Image = System.Windows.Controls.Image;

namespace ImageToolApp.Controllers
{
    public class EncryptController : BaseTabController<BaseTabViewModel>
    {
        private readonly List<Expander> mExpanders;

        public EncryptController(MainController mainController, string viewName, bool textBoxReadOnly) : base(mainController, viewName, textBoxReadOnly)
        {
            mExpanders = View.FindChildren<Expander>().Where(x => x.Content != null && x.Content.GetType() != typeof(Image)).ToList();
            foreach (var expander in mExpanders)
            {
                expander.Expanded += ExpanderOnExpanded;
                expander.Collapsed += ExpanderOnCollapsed;
            }
        }

        private bool mHandling;

        public override void UnregisterEvents()
        {
            base.UnregisterEvents();
            foreach (var expander in mExpanders)
            {
                expander.Expanded -= ExpanderOnExpanded;
                expander.Collapsed -= ExpanderOnCollapsed;
            }
        }

        private void ExpanderOnExpanded(object sender, RoutedEventArgs routedEventArgs)
        {
            if (!mHandling)
            {
                var expander = sender as Expander;
                mHandling = true;
                foreach (var exp in mExpanders)
                {
                    exp.IsExpanded = false;
                    var row = View.Grid.RowDefinitions[Grid.GetRow(exp)];
                    row.Height = GridLength.Auto;
                }

                expander.IsExpanded = true;
                View.Grid.RowDefinitions[Grid.GetRow(expander)].Height = new GridLength(1, GridUnitType.Star);
                routedEventArgs.Handled = true;
                mHandling = false;
            }
        }

        private void ExpanderOnCollapsed(object sender, RoutedEventArgs routedEventArgs)
        {
            foreach (var exp in mExpanders)
            {
                exp.IsExpanded = false;
                var row = View.Grid.RowDefinitions[Grid.GetRow(exp)];
                row.Height = GridLength.Auto;
            }
            routedEventArgs.Handled = true;
        }


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
            using (var handle = new HandleJobController(ProgressRing))
            {
                handle.Progress(() =>
                {
                    using (var bitmap = new Bitmap(ViewModel.ImagePath))
                    {
                        var text = ViewModel.Text;
                        if (ViewModel.EncryptedCheck)
                        {
                            text = SymmetricAlgorithmBase.Encrypt(this, ViewModel.SelectedEncryptionMethod, text,
                                ViewModel.Password);
                        }


                        var result = SteganographicAlgorithmBase.Encrypt(this, ViewModel.SelectedSteganographicMethod, bitmap,
                            text);
                        if (result != null)
                        {
                            var path = Path.GetTempFileName().Replace("tmp", "png");
                            result.Save(path);
                            ViewModel.ResultImagePath = path;
                        }
                    }
                });
            }
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