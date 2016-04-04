using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using FunctionLib.Helper;
using FunctionLib.Model;
using FunctionLib.Steganography.LSB;
using ImageToolApp.ViewModels;
using ImageToolApp.Views;
using MahApps.Metro.Controls;
using Microsoft.Win32;
using UserControlClassLibrary;
using UserControlClassLibrary.DocumentChooser;
using UserControlClassLibrary.PathChooser;
using Image = System.Windows.Controls.Image;

namespace ImageToolApp.Controllers
{
    public class EncryptTabController : BaseTabController<EncryptTabViewModel, EncryptTabView>
    {
        private readonly List<Expander> mExpanders;

        private bool mExpanderAlreadyHandled;

        public EncryptTabController()
        {
            mExpanders =
                View.FindChildren<Expander>()
                    .Where(x => x.Content != null && x.Content.GetType() != typeof (Image))
                    .ToList();
            foreach (var expander in mExpanders)
            {
                expander.Expanded += ExpanderOnExpanded;
                expander.Collapsed += ExpanderOnCollapsed;
            }
        }

        protected override UICommand ActionCommand
        {
            get { return UICommand.Regular(Encrypt); }
        }

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
            if (!mExpanderAlreadyHandled)
            {
                var expander = sender as Expander;
                mExpanderAlreadyHandled = true;
                foreach (var exp in mExpanders)
                {
                    exp.IsExpanded = false;
                    var row = View.Grid.RowDefinitions[Grid.GetRow(exp)];
                    row.Height = GridLength.Auto;
                }

                expander.IsExpanded = true;
                View.Grid.RowDefinitions[Grid.GetRow(expander)].Height = new GridLength(1, GridUnitType.Star);
                routedEventArgs.Handled = true;
                mExpanderAlreadyHandled = false;
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

        public void SaveImage()
        {
            var dialog = new SaveFileDialog
            {
                Filter = ConvertHelper.GenerateFilter(ViewModel.SteganographicModel.Algorithm.PossibleImageFormats)
            };
            var dialogResult = dialog.ShowDialog();
            if (dialogResult.HasValue && dialogResult.Value)
            {
                if (File.Exists(dialog.FileName))
                {
                    File.Delete(dialog.FileName);
                }

                File.Move(ViewModel.Result, dialog.FileName);
                ViewModel.Result = dialog.FileName;
            }
        }

        public void OpenTxt()
        {
            var dialog = new OpenFileDialog
            {
                Filter = "Text File|*.txt",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
            };
            var dialogResult = dialog.ShowDialog();
            if (dialogResult.HasValue && dialogResult.Value)
            {
                ViewModel.Text = File.ReadAllText(dialog.FileName);
            }
        }

        private void Encrypt()
        {
            HandleJobController.Progress(() =>
            {
                var message = GetCurrentMessage();
                //TODO compression
                var model = new EncodeModel(ViewModel.ImagePath, message, ViewModel.CryptionModel.Algorithm,
                    ViewModel.CryptionModel.Password, ViewModel.SteganographicModel.Algorithm,
                    ViewModel.SteganographicModel.Compression,
                    ViewModel.SteganographicModel.LsbIndicator);

                var result = model.Encode();
                ViewModel.Result = result;
            });
        }

        private string GetCurrentMessage()
        {
            var result = string.Empty;
            object expanderContent = null;
            if (mExpanders != null && mExpanders.Count > 0)
            {
                expanderContent = Application.Current.Dispatcher.Invoke(() =>
                {
                    var current = mExpanders.FirstOrDefault(x => x.IsExpanded);
                    return current != null ? current.Content : null;
                });
            }

            if (expanderContent == null)
            {
                result = ViewModel.Text;
            }
            else
            {
                if (expanderContent is TextBox)
                {
                    result = ViewModel.Text;
                }
                else if (expanderContent is PathChooser)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        var content = expanderContent as PathChooser;
                        result = (content.DataContext as PathChooserViewModel).Path;
                    });
                }
                else
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        var content = expanderContent as DocumentChooser;
                        result = (content.DataContext as DocumentChooserViewModel).Path;
                    });
                }
            }
            return result;
        }


        public void ChangedPixels()
        {
            var algorithm = ViewModel.SteganographicModel.Algorithm as LsbAlgorithmBase;
            if (algorithm != null)
            {
                var path = algorithm.ChangeColor(ViewModel.Result, Color.Red);
                var controller = new ImagePresentationController(path,
                    string.Format("{0} Pixel", algorithm.ChangedPixels.Count));
            }
        }
    }
}