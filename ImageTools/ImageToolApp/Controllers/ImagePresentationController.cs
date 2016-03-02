using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ImageToolApp.Models;
using ImageToolApp.ViewModels;
using ImageToolApp.Views;
using MahApps.Metro.Controls;
using Microsoft.Win32;
using UserControlClassLibrary;

namespace ImageToolApp.Controllers
{
    public class ImagePresentationController
    {
        private readonly ImagePresentation mView;

        public ImagePresentationController(string image, string text = "")
        {
            var viewModel = new ImagePresentationViewModel(image, text);
            RegisterCommands(viewModel);
            mView = new ImagePresentation {DataContext = viewModel};
            mView.ImageExpander.Expanded += ImageExpanderEvent;
            mView.ImageExpander.Collapsed += ImageExpanderEvent;
            mView.ShowDialog();
            UnregisterEvent();
        }

        private void RegisterCommands(ImagePresentationViewModel viewModel)
        {
            viewModel.SaveCommand = UICommand.Regular(() =>
            {
                var dialog = new SaveFileDialog
                {
                    Filter = "PNG Image|*.png|Bitmap Image|*.bmp",
                    InitialDirectory = Settings.Instance.StandardPath
                };
                var dialogResult = dialog.ShowDialog();
                if (dialogResult.HasValue && dialogResult.Value)
                {
                    var tmp = viewModel.Image;
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
            });
        }

        private void UnregisterEvent()
        {
            mView.ImageExpander.Expanded -= ImageExpanderEvent;
            mView.ImageExpander.Collapsed -= ImageExpanderEvent;
        }

        private void ImageExpanderEvent(object sender, RoutedEventArgs routedEventArgs)
        {
            var expander = sender as Expander;
            ResizeImageExpanderGrid(expander, expander.IsExpanded);
        }

        private void ResizeImageExpanderGrid(UIElement expander, bool expanded)
        {
            var grid = mView.FindChildren<Grid>().FirstOrDefault();
            var column = grid.ColumnDefinitions[Grid.GetColumn(expander)];
            column.Width = new GridLength(1.0, expanded ? GridUnitType.Star : GridUnitType.Auto);
        }
    }
}