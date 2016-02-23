using System;
using System.Windows;
using ImageToolApp.Controllers;

namespace ImageToolApp
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        [STAThread]
        public static void Main()
        {
            var application = new App();
            application.InitializeComponent();

            // Global Exception Handler
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
            application.MainWindow = new MainWindow();
            var controller = new MainController((MainWindow) application.MainWindow);

            application.Run();
        }

        private static void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var exception = e.ExceptionObject as Exception;
            if (exception == null)
            {
                MessageBox.Show(Current.MainWindow,
                    "Ein unbehandelter Fehler ist aufgetreten.",
                    "Unbekannter Fehler",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            else
            {
                MessageBox.Show(Current.MainWindow,
                    exception.InnerException.ToString(),
                    "Fehler",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                //e.handled = true;
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            AppDomain.CurrentDomain.UnhandledException -= CurrentDomainOnUnhandledException;
            base.OnExit(e);
            Environment.Exit(0);
        }
    }
}