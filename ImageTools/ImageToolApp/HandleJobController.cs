using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using MahApps.Metro.Controls;

namespace ImageToolApp
{
    public class HandleJobController : IDisposable
    {
        public readonly Window Window = Application.Current.MainWindow;

        public void Dispose()
        {
            //if (ProgressView != null)
            //{
            //    ProgressView.IsActive = false;
            //    ProgressView.IsLarge = false;
            //}
            Window.Cursor = Cursors.Arrow;
        }
        
        public void Progress(Action action)
        {
            Window.Cursor = Cursors.Wait;
            //if (ProgressView != null)
            //{
            //    ProgressView.Visibility = Visibility.Visible;
            //    ProgressView.IsActive = true;
            //    ProgressView.IsLarge = true;
            //}
            action.Invoke();
        }
    }
}