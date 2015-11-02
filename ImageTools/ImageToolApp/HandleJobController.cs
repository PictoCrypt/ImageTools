using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using MahApps.Metro.Controls;

namespace ImageToolApp
{
    public class HandleJobController : IDisposable
    {
        public Window UiWindow { get; set; }
        public ProgressRing ProgressView { get; set; }

        public HandleJobController(ProgressRing progressRing) : this(null, progressRing)
        {   
        }
        
        public HandleJobController(Window mainWindow = null, ProgressRing progressBar = null)
        {
            if (mainWindow != null)
            {
                UiWindow = mainWindow;
            }
            else
            {
                UiWindow = Application.Current.MainWindow;
            }
            if (progressBar != null)
            {
                ProgressView = progressBar;
            }
        }


        public void Dispose()
        {
            if (ProgressView != null)
            {
                ProgressView.IsActive = false;
                ProgressView.IsLarge = false;
            }
            UiWindow.Cursor = Cursors.Arrow;
        }
        
        public void Progress(Action action)
        {
            UiWindow.Cursor = Cursors.Wait;
            if (ProgressView != null)
            {
                ProgressView.Visibility = Visibility.Visible;
                ProgressView.IsActive = true;
                ProgressView.IsLarge = true;
            }
            action.Invoke();
        }
    }
}